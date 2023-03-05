using DanceCompetitionHelper.Config;
using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.OrgImpl.Oetsv.WorkData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;

namespace DanceCompetitionHelper.OrgImpl.Oetsv
{
    public class OetsvCompetitionImporter : ICompetitionImporter
    {
        private readonly ILogger<OetsvCompetitionImporter> _logger;
        private readonly ImporterSettings _importerSettings;

        public string Oranizer { get; private set; } = default!;
        public string OrgCompetitionId { get; set; } = default!;
        public string CompetitionName { get; private set; } = default!;
        public string CompetitionLocation { get; private set; } = default!;
        public string? CompetitionAddress { get; private set; }
        public DateTime CompetitionDate { get; private set; }
        public string? CompetitionType { get; private set; } = default!;

        public List<CompetitionClassImport> CompetitionClasses { get; } = new List<CompetitionClassImport>();
        public List<string> MastersOfCeremony { get; } = new List<string>();
        public List<string> Chairperson { get; } = new List<string>();
        public List<string> Assessors { get; } = new List<string>();
        public List<string> Adjudicators { get; } = new List<string>();
        public List<CompetitionParticipantImport> Participants { get; } = new List<CompetitionParticipantImport>();

        #region Constants...

        public Encoding HtmlEncodingCompetition { get; set; } = Encoding.GetEncoding(1252); // Encoding.UTF8;
        public Encoding HtmlEncodingParticipants { get; set; } = Encoding.GetEncoding(1252);

        public const int PartExcelRegOrgCompId = 0;
        public const int PartExcelRegOrgCompClassId = 1;
        public const int PartExcelRegStartNumber = 2;
        public const int PartExcelRegPartALastName = 3;
        public const int PartExcelRegPartAFirstName = 4;
        public const int PartExcelRegPartAOrgId = 16;
        public const int PartExcelRegPartBLastName = 5;
        public const int PartExcelRegPartBFirstName = 6;
        public const int PartExcelRegPartBOrgId = 17;
        public const int PartExcelRegClubName = 7;
        public const int PartExcelRegClubOrgId = 18;
        public const int PartExcelRegState = 8;
        public const int PartExcelRegStateAbbr = 9;
        public const int PartExcelRegDiscipline = 11;
        public const int PartExcelRegAgeClass = 12;
        public const int PartExcelRegAgeGroup = 13;
        public const int PartExcelRegClass = 14;

        public const int PartExcelOrgCurrentClassSta = 21;
        public const int PartExcelOrgPointsSta = 19;
        public const int PartExcelOrgStartsSta = 20;
        public const int PartExcelOrgMinPointsForPromotionSta = 22;
        public const int PartExcelOrgMinStartsForPromotionSta = 23;

        public const int PartExcelOrgCurrentClassLa = 27;
        public const int PartExcelOrgPointsLa = 25;
        public const int PartExcelOrgStartsLa = 26;
        public const int PartExcelOrgMinPointsForPromotionLa = 28;
        public const int PartExcelOrgMinStartsForPromotionLa = 29;

        public const string ExpectedDownloadContentType = "text/csv";

        #endregion // Constants...

        static OetsvCompetitionImporter()
        {
            Encoding.RegisterProvider(
                CodePagesEncodingProvider.Instance);
        }

        public OetsvCompetitionImporter(
            ILogger<OetsvCompetitionImporter> logger,
            ImporterSettings importerSettings)
        {
            _logger = logger ?? throw new ArgumentNullException(
                nameof(logger));
            _importerSettings = importerSettings ?? throw new ArgumentNullException(
                nameof(importerSettings));
        }

        public List<string> ImportOrUpdateByFile(
            DanceCompetitionHelperDbContext dbCtx,
            string? fullPathCompetition,
            string? fullPathCompetitionClasses,
            string? fullPathParticipants)
        {
            var retErrors = new List<string>();

            try
            {
                retErrors.AddRange(
                    ExtractData(
                        fullPathCompetition,
                        fullPathParticipants));
                retErrors.AddRange(
                    ImportOrUpdateDatabase(
                        dbCtx));
            }
            catch (Exception exc)
            {
                var errorStr = string.Format(
                    "Error during '{0}': {1}",
                    nameof(ImportOrUpdateByFile),
                    exc.Message);

                _logger.LogError(
                    exc,
                    errorStr);
            }

            return retErrors;
        }

        public List<string> ImportOrUpdateByUrl(
            DanceCompetitionHelperDbContext dbCtx,
            Uri? uriCompetition,
            Uri? uriCompetitionClasses,
            Uri? uriParticipants)
        {
            var retErrors = new List<string>();
            try
            {
                var fullPathCompetitionCsv = DownloadFile(
                    "comp",
                    uriCompetition,
                    HtmlEncodingCompetition);
                var fullPathParticipantsCsv = DownloadFile(
                    "part",
                    uriParticipants,
                    HtmlEncodingParticipants);

                retErrors.AddRange(
                    ImportOrUpdateByFile(
                        dbCtx,
                        fullPathCompetitionCsv,
                        null,
                        fullPathParticipantsCsv));
            }
            catch (Exception exc)
            {
                var errorStr = string.Format(
                    "Error during '{0}': {1}",
                    nameof(ImportOrUpdateByUrl),
                    exc.Message);

                _logger.LogError(
                    exc,
                    errorStr);
            }

            return retErrors;
        }

        public Uri GetCompetitioUriForOrgId(
            int orgId)
        {
            return new Uri(
                string.Format(
                    "https://www.oetsv.at/modules/ext-data/turniere/oetsv_turniere_{0}.csv",
                    orgId
                        .ToString(
                            "D0")
                        .PadLeft(
                            4,
                            '0')));
        }

        public Uri GetParticipantsUriForOrgId(
            int orgId)
        {
            return new Uri(
                string.Format(
                    "https://www.oetsv.at/downloads-dynamisch/nennungen/startliste_{0}_app.csv",
                    orgId
                        .ToString(
                            "D0")
                        .PadLeft(
                            4,
                            '0')));
        }

        public string? DownloadFile(
            string fileType,
            Uri? downloadUri,
            Encoding encoding)
        {
            if (downloadUri == null)
            {
                return null;
            }

            var retFilePath = Path.GetTempFileName();

            if (string.IsNullOrEmpty(
                _importerSettings.DownloadFolder) == false)
            {
                Directory.CreateDirectory(
                    _importerSettings.DownloadFolder);

                retFilePath = Path.Combine(
                    _importerSettings.DownloadFolder,
                    string.Format(
                        "{0}_{1}_download.csv",
                        DateTime.Now.ToString(
                            "yyyyMMdd_HHmmss"),
                        fileType));
            }

            using var useFileStream = File.OpenWrite(
                retFilePath);

            try
            {
                using var httpClient = new HttpClient();

                var getResp = httpClient
                    .GetAsync(
                        downloadUri)
                    .GetAwaiter()
                    .GetResult();

                if (getResp.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(
                        string.Format(
                            "StatuCode {0}",
                            getResp.StatusCode));
                }

                if (ExpectedDownloadContentType != getResp.Content.Headers.ContentType?.MediaType)
                {
                    throw new Exception(
                        string.Format(
                            "Invalid ContentType expected/received '{0}'/'{1}'",
                            ExpectedDownloadContentType,
                            getResp.Content.Headers.ContentType));
                }

                var fullContent = getResp.Content
                    .ReadAsByteArrayAsync()
                    .GetAwaiter()
                    .GetResult();

                useFileStream.Write(
                    Encoding.Default.GetBytes(
                        encoding
                            .GetString(
                                fullContent)));

                useFileStream.Flush();

                return retFilePath;
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during downloading '{Uri}': {Message}",
                    downloadUri,
                    exc.Message);

                useFileStream.Flush();
                useFileStream.Close();

                File.Delete(
                    retFilePath);
            }

            return null;
        }

        public List<string> ExtractData(
            string? fullPathCompetition,
            string? fullPathParticipants)
        {
            var retErrors = new List<string>();

            // ----------------------------------------
            if (string.IsNullOrEmpty(
                fullPathCompetition)
                || File.Exists(
                    fullPathCompetition) == false)
            {
                return retErrors;
            }

            foreach (var curLine in File.ReadAllLines(
                fullPathCompetition))
            {
                var lineItems = curLine.Split(
                    new[] { ";" },
                    StringSplitOptions.TrimEntries);

                if (lineItems.Length >= 2)
                {
                    switch (lineItems[0].ToUpperInvariant())
                    {
                        case "TSD":
                            ExtractCompetitionInfo(
                                lineItems);
                            break;

                        case "TKL":
                            ExtractCompetitionClasses(
                                lineItems);
                            break;

                        case "FUNKT":
                            ExtractOfficials(
                                lineItems);
                            break;
                    }
                }
            }

            // ----------------------------------------
            if (string.IsNullOrEmpty(
                fullPathParticipants)
                || File.Exists(
                    fullPathParticipants) == false)
            {
                return retErrors;
            }

            foreach (var curLine in File.ReadAllLines(
                fullPathParticipants))
            {
                ExtractCompetitionParticipants(
                    curLine.Split(
                        new[] { ";" },
                        StringSplitOptions.TrimEntries));
            }

            return retErrors;
        }

        public void ExtractCompetitionInfo(
            string[] competitionInfo)
        {
            if (competitionInfo.Length < 2)
            {
                return;
            }

            var orgInfo = competitionInfo[1].Split(
                    new[] { "=" },
                    StringSplitOptions.TrimEntries);

            if (orgInfo.Length < 2)
            {
                return;
            }

            var orgInfoType = orgInfo[0].ToUpperInvariant();
            var orgInfoValue = orgInfo[1];

            switch (orgInfoType)
            {
                case "TKNR":
                    OrgCompetitionId = orgInfoValue;
                    break;

                case "ORGANISATOR":
                    Oranizer = orgInfoValue;
                    break;

                case "TURNIERBEZEICHNUNG":
                    CompetitionName = orgInfoValue;
                    break;

                case "LOKATION":
                    CompetitionLocation = orgInfoValue;
                    break;

                case "ORT":
                    CompetitionAddress = orgInfoValue;
                    break;

                case "DATUM":
                    CompetitionDate = ParseDateStrings(
                        orgInfoValue);
                    break;

                case "TURNIERART":
                    var useCompetitionType = orgInfoValue;
                    var firstBlank = useCompetitionType.IndexOf(' ');

                    if (firstBlank >= 1)
                    {
                        useCompetitionType = useCompetitionType.Substring(
                            0,
                            firstBlank);
                    }

                    CompetitionType = OetsvConstants.CompetitionType.ToCompetitionType(
                        useCompetitionType.Trim());
                    break;

            }
        }

        public void ExtractCompetitionClasses(
            string[] competitionInfo)
        {
            if (competitionInfo.Length < 10)
            {
                return;
            }

            if (string.IsNullOrEmpty(OrgCompetitionId))
            {
                OrgCompetitionId = competitionInfo[1].Trim();
            }

            CompetitionClasses.Add(
                new CompetitionClassImport()
                {
                    OrgClassIdRaw = competitionInfo[2],
                    OrgClassId = competitionInfo[2].Trim(),

                    AgeClassRaw = competitionInfo[3],
                    AgeClass = OetsvConstants.AgeClasses.ToAgeClasses(
                        competitionInfo[3]),

                    AgeGroupRaw = competitionInfo[4],
                    AgeGroup = OetsvConstants.AgeGroups.ToAgeGroup(
                        competitionInfo[4]),

                    ClassRaw = competitionInfo[5],
                    Class = OetsvConstants.Classes.ToClasses(
                        competitionInfo[5]),

                    DisciplineRaw = competitionInfo[6],
                    Discipline = OetsvConstants.Disciplines.ToDisciplines(
                        competitionInfo[6]),

                    NameRaw = competitionInfo[7],
                    Name = competitionInfo[7]
                        .Trim(
                            '*')
                        .Trim(),
                });
        }

        public void ExtractOfficials(
            string[] competitionInfo)
        {
            if (competitionInfo.Length < 2)
            {
                return;
            }

            var officialsInfo = competitionInfo[1].Split(
                new[] { "=" },
                StringSplitOptions.TrimEntries);

            if (officialsInfo.Length < 2)
            {
                return;
            }

            var offInfoType = officialsInfo[0].ToUpperInvariant();
            var offInfoValue = officialsInfo[1];

            switch (offInfoType)
            {
                case "TL":
                    MastersOfCeremony.Add(
                        offInfoValue);
                    break;

                case "BS":
                    Assessors.Add(
                        offInfoValue);
                    break;

                case "WR":
                    Adjudicators.Add(
                        offInfoValue);
                    break;
            }
        }

        public void ExtractCompetitionParticipants(
            string[] participantInfo)
        {
            if (participantInfo.Length < 32
                || OrgCompetitionId != participantInfo[PartExcelRegOrgCompId])
            {
                return;
            }

            var addPartImport = new CompetitionParticipantImport()
            {
                RegOrgClassIdRaw = participantInfo[PartExcelRegOrgCompClassId],
                RegOrgClassId = participantInfo[PartExcelRegOrgCompClassId].Trim(),

                RegStartNumberRaw = participantInfo[PartExcelRegStartNumber],
                RegStartNumber = int.Parse(
                    participantInfo[PartExcelRegStartNumber]),

                RegPartAFirstNameRaw = participantInfo[PartExcelRegPartAFirstName]?.Trim(),
                RegPartALastNameRaw = participantInfo[PartExcelRegPartALastName]?.Trim(),

                RegPartAOrgIdRaw = participantInfo[PartExcelRegPartAOrgId],
                RegPartAOrgId = participantInfo[PartExcelRegPartAOrgId].Trim(),

                RegPartBFirstNameRaw = participantInfo[PartExcelRegPartBFirstName]?.Trim(),
                RegPartBLastNameRaw = participantInfo[PartExcelRegPartBLastName]?.Trim(),

                RegPartBOrgIdRaw = participantInfo[PartExcelRegPartBOrgId],
                RegPartBOrgId = participantInfo[PartExcelRegPartBOrgId].Trim(),

                RegClubNameRaw = participantInfo[PartExcelRegClubName],
                RegClubName = participantInfo[PartExcelRegClubName].Trim(),

                RegClubOrgIdRaw = participantInfo[PartExcelRegClubOrgId],
                RegClubOrgId = participantInfo[PartExcelRegClubOrgId].Trim(),

                RegStateRaw = participantInfo[PartExcelRegState],
                RegState = participantInfo[PartExcelRegState].Trim(),

                RegStateAbbrRaw = participantInfo[PartExcelRegStateAbbr],
                RegStateAbbr = participantInfo[PartExcelRegStateAbbr].Trim(),

                RegDisciplineRaw = participantInfo[PartExcelRegDiscipline],
                RegDiscipline = OetsvConstants.Disciplines.ToDisciplines(
                    participantInfo[PartExcelRegDiscipline]),

                RegClassRaw = participantInfo[PartExcelRegClass],
                RegClass = OetsvConstants.Classes.ToClasses(
                    participantInfo[PartExcelRegClass]),

                RegAgeClassRaw = participantInfo[PartExcelRegAgeClass],
                RegAgeClass = OetsvConstants.AgeClasses.ToAgeClasses(
                    participantInfo[PartExcelRegAgeClass]),

                RegAgeGroupRaw = participantInfo[PartExcelRegAgeGroup],
                RegAgeGroup = OetsvConstants.AgeGroups.ToAgeGroup(
                    participantInfo[PartExcelRegAgeGroup]),
            };

            addPartImport.RegPartAName = CombineFirstLastName(
                addPartImport.RegPartAFirstNameRaw,
                addPartImport.RegPartALastNameRaw);
            addPartImport.RegPartBName = CombineFirstLastName(
                addPartImport.RegPartBFirstNameRaw,
                addPartImport.RegPartBLastNameRaw);

            switch (addPartImport.RegDiscipline)
            {
                case OetsvConstants.Disciplines.Sta:
                    addPartImport.OrgCurrentClassRaw = participantInfo[PartExcelOrgCurrentClassSta];
                    addPartImport.OrgCurrentClass = OetsvConstants.Classes.ToClasses(
                        participantInfo[PartExcelOrgCurrentClassSta]);

                    addPartImport.OrgPointsRaw = participantInfo[PartExcelOrgPointsSta];
                    addPartImport.OrgPoints = double.Parse(
                        participantInfo[PartExcelOrgPointsSta]);

                    addPartImport.OrgStartsRaw = participantInfo[PartExcelOrgStartsSta];
                    addPartImport.OrgStarts = int.Parse(
                        participantInfo[PartExcelOrgStartsSta]);

                    addPartImport.OrgMinPointsForPromotionRaw = participantInfo[PartExcelOrgMinPointsForPromotionSta];
                    addPartImport.OrgMinPointsForPromotion = double.Parse(
                        participantInfo[PartExcelOrgMinPointsForPromotionSta]);

                    addPartImport.OrgMinStartsForPromotionRaw = participantInfo[PartExcelOrgMinStartsForPromotionSta];
                    addPartImport.OrgMinStartsForPromotion = int.Parse(
                        participantInfo[PartExcelOrgMinStartsForPromotionSta]);

                    break;

                case OetsvConstants.Disciplines.La:
                    addPartImport.OrgCurrentClassRaw = participantInfo[PartExcelOrgCurrentClassLa];
                    addPartImport.OrgCurrentClass = OetsvConstants.Classes.ToClasses(
                        participantInfo[PartExcelOrgCurrentClassLa]);

                    addPartImport.OrgPointsRaw = participantInfo[PartExcelOrgPointsLa];
                    addPartImport.OrgPoints = double.Parse(
                        participantInfo[PartExcelOrgPointsLa]);

                    addPartImport.OrgStartsRaw = participantInfo[PartExcelOrgStartsLa];
                    addPartImport.OrgStarts = int.Parse(
                        participantInfo[PartExcelOrgStartsLa]);

                    addPartImport.OrgMinPointsForPromotionRaw = participantInfo[PartExcelOrgMinPointsForPromotionLa];
                    addPartImport.OrgMinPointsForPromotion = int.Parse(
                        participantInfo[PartExcelOrgMinPointsForPromotionLa]);

                    addPartImport.OrgMinStartsForPromotionRaw = participantInfo[PartExcelOrgMinStartsForPromotionLa];
                    addPartImport.OrgMinStartsForPromotion = int.Parse(
                        participantInfo[PartExcelOrgMinStartsForPromotionLa]);

                    break;
            }

            if (addPartImport.OrgCurrentClass != addPartImport.OrgCurrentClassRaw)
            {
                var gotIt = true;
            }

            Participants.Add(
                addPartImport);
        }

        public List<string> ImportOrUpdateDatabase(
            DanceCompetitionHelperDbContext dbCtx)
        {
            var retErrors = new List<string>();

            var foundComp = dbCtx.Competitions
                .TagWith(
                    nameof(ImportOrUpdateDatabase) + "[01]")
                .FirstOrDefault(
                    x => x.OrgCompetitionId == OrgCompetitionId);

            if (foundComp == null)
            {
                foundComp = dbCtx.Competitions.Add(
                    new Competition()
                    {
                        Organization = OrganizationEnum.Oetsv,
                        OrgCompetitionId = OrgCompetitionId,
                        CompetitionName = CompetitionName,
                        CompetitionInfo = string.Format(
                            "{0} {1} {2}",
                            CompetitionType,
                            CompetitionLocation,
                            CompetitionAddress),
                        CompetitionDate = CompetitionDate,
                    })
                    .Entity;
            }
            else
            {
                _logger.LogInformation(
                    "Update existing {CompName}: {foundComp}",
                    nameof(Competition),
                    foundComp);

                foundComp.CompetitionName = string.Format(
                    "{0} ({1})",
                    CompetitionName,
                    CompetitionType);
                foundComp.CompetitionInfo = string.Format(
                    "{0}, {1}",
                    CompetitionAddress,
                    CompetitionLocation);
                foundComp.CompetitionDate = CompetitionDate;
            }

            var foundAdjPanel = dbCtx.AdjudicatorPanels
                .TagWith(
                    nameof(ImportOrUpdateDatabase) + "[02]")
                .FirstOrDefault(
                    x => x.CompetitionId == foundComp.CompetitionId);

            if (foundAdjPanel == null)
            {
                foundAdjPanel = dbCtx.AdjudicatorPanels.Add(
                    new AdjudicatorPanel()
                    {
                        Competition = foundComp,
                        Name = "Panel 1",
                    }).Entity;

                var adjAbbr = 'A';
                foreach (var curAdj in Adjudicators)
                {
                    dbCtx.Adjudicators.Add(
                        new Adjudicator()
                        {
                            AdjudicatorPanel = foundAdjPanel,
                            Name = curAdj,
                            Abbreviation = adjAbbr.ToString(),
                        });

                    adjAbbr++;
                }
            }
            else
            {
                // TOOD: how to update those?.. -> add missing ones...
            }

            var allCompClasses = dbCtx.CompetitionClasses
                .TagWith(
                    nameof(ImportOrUpdateDatabase) + "[03]")
                .Where(
                    x => x.CompetitionId == foundComp.CompetitionId)
                .ToDictionary(
                    x => x.OrgClassId);

            foreach (var curImportCompClass in CompetitionClasses)
            {
                if (string.IsNullOrEmpty(
                    curImportCompClass.OrgClassId))
                {
                    var logString = string.Format(
                        "{0} '{1}' is invalid/missing! Ignore {2}",
                        nameof(curImportCompClass.OrgClassId),
                        curImportCompClass.OrgClassId,
                        curImportCompClass);

                    retErrors.Add(
                        logString);

                    _logger.LogWarning(
                        logString);

                    continue;
                }

                if (allCompClasses.TryGetValue(
                    curImportCompClass.OrgClassId,
                    out var foundCompetitionClass))
                {
                    _logger.LogInformation(
                        "Update existing {CompClassName}: {foundCompetitionClass}",
                        nameof(CompetitionClass),
                        foundCompetitionClass);

                    foundCompetitionClass.CompetitionClassName = curImportCompClass.Name ?? foundCompetitionClass.CompetitionClassName;
                    foundCompetitionClass.Discipline = curImportCompClass.Discipline ?? foundCompetitionClass.Discipline;
                    foundCompetitionClass.AgeClass = curImportCompClass.AgeClass ?? foundCompetitionClass.AgeClass;
                    foundCompetitionClass.AgeGroup = curImportCompClass.AgeGroup ?? foundCompetitionClass.AgeGroup;
                    foundCompetitionClass.Class = curImportCompClass.Class ?? foundCompetitionClass.Class;
                }
                else
                {
                    var useClassName = (string.IsNullOrEmpty(curImportCompClass.Name)
                        ? string.Format(
                            "{0} {1} {2} {3} {4}",
                            curImportCompClass.AgeClass,
                            curImportCompClass.AgeClass,
                            curImportCompClass.AgeGroup,
                            curImportCompClass.Discipline,
                            curImportCompClass.Class)
                        : curImportCompClass.Name)
                            .Replace(
                                "  ",
                                " ");

                    var newCompClass = dbCtx.CompetitionClasses.Add(
                        new CompetitionClass()
                        {
                            Competition = foundComp,
                            OrgClassId = curImportCompClass.OrgClassId,
                            AdjudicatorPanel = foundAdjPanel,
                            CompetitionClassName = useClassName,
                            Discipline = curImportCompClass.Discipline,
                            AgeClass = curImportCompClass.AgeClass,
                            AgeGroup = curImportCompClass.AgeGroup,
                            Class = curImportCompClass.Class,
                            MinPointsForPromotion = OetsvConstants.Classes.GetMinPointsForPromotion(
                                curImportCompClass.Discipline,
                                curImportCompClass.AgeClass,
                                curImportCompClass.AgeGroup,
                                curImportCompClass.Class),
                            MinStartsForPromotion = OetsvConstants.Classes.GetMinStartsForPromotion(
                                curImportCompClass.Discipline,
                                curImportCompClass.AgeClass,
                                curImportCompClass.AgeGroup,
                                curImportCompClass.Class),
                            PointsForFirst = OetsvConstants.CompetitionType.GetPointsForWinning(
                                CompetitionType),
                        })
                        .Entity;

                    allCompClasses[newCompClass.OrgClassId] = newCompClass;
                }
            }

            var allParticipantsByImpStr = dbCtx.Participants
                .TagWith(
                    nameof(ImportOrUpdateDatabase) + "[04]")
                .Include(
                    x => x.CompetitionClass)
                .Where(
                    x => x.CompetitionId == foundComp.CompetitionId)
                .ToDictionary(
                    x => string.Format(
                        "{0}/{1}",
                        x.CompetitionClass.OrgClassId,
                        GetParticipantImportString(
                            x)));

            foreach (var curImportPart in Participants)
            {
                var curPartImpString = string.Format(
                    "{0}/{1}",
                    curImportPart.RegOrgClassId,
                    GetParticipantImportString(
                        curImportPart));

                if (allCompClasses.TryGetValue(
                    curImportPart.RegOrgClassId ?? string.Empty,
                    out var useCompClass) == false)
                {
                    var logString = string.Format(
                        "Unable to find {0} '{1}' of Import-Participant {2}",
                        nameof(CompetitionClass.OrgClassId),
                        curImportPart.RegOrgClassId,
                        curImportPart);

                    retErrors.Add(
                        logString);
                    _logger.LogWarning(
                        logString);

                    continue;
                }

                if (allParticipantsByImpStr.TryGetValue(
                    curPartImpString,
                    out var existingParticipant))
                {
                    // TODO: update!
                }
                else
                {
                    var newParticipant = dbCtx.Participants.Add(
                        new Participant()
                        {
                            Competition = foundComp,
                            CompetitionClass = useCompClass,
                            StartNumber = curImportPart.RegStartNumber ?? 0,
                            NamePartA = curImportPart.RegPartAName ?? "??",
                            OrgIdPartA = curImportPart.RegPartAOrgId,
                            NamePartB = curImportPart.RegPartBName ?? "??",
                            OrgIdPartB = curImportPart.RegPartBOrgId,

                            ClubName = curImportPart.RegClubName,
                            OrgIdClub = curImportPart.RegClubOrgId,

                            OrgPointsPartA = curImportPart.OrgPoints ?? 0,
                            OrgStartsPartA = curImportPart.OrgStarts ?? 0,
                        })
                        .Entity;

                    if (newParticipant.OrgPointsPartA >= useCompClass.MinPointsForPromotion
                        && newParticipant.OrgStartsPartA >= useCompClass.MinStartsForPromotion)
                    {
                        newParticipant.OrgAlreadyPromotedPartA = true;
                        newParticipant.OrgAlreadyPromotedInfoPartA = string.Format(
                            "Points and starts exeeds Promotion Limits");
                    }

                    var checkAlreadyPromoted = true;
                    if (useCompClass.Class == OetsvConstants.Classes.Amateur
                        && curImportPart.RegClass == OetsvConstants.Classes.GirlsOnly)
                    {
                        // girls only... ignore this...
                        checkAlreadyPromoted = false;
                    }

                    if (checkAlreadyPromoted
                        && (curImportPart.OrgCurrentClass != useCompClass.Class
                        || curImportPart.OrgCurrentClass != curImportPart.RegClass))
                    {
                        newParticipant.OrgAlreadyPromotedPartA = true;
                        newParticipant.OrgAlreadyPromotedInfoPartA = string.Format(
                            "Registration Class '{0}' != Import/Org Class: '{1}' ({2})",
                            curImportPart.RegClass,
                            curImportPart.OrgCurrentClass,
                            curImportPart.OrgCurrentClassRaw);
                    }

                    allParticipantsByImpStr[curPartImpString] = newParticipant;
                }
            }

            return retErrors;
        }

        private string GetParticipantImportString(
            Participant participant)
        {
            return string.Join(
                "/",
                participant.NamePartA,
                participant.OrgIdPartA,
                participant.NamePartB,
                participant.OrgIdPartB,
                participant.ClubName,
                participant.OrgIdClub);
        }

        private string GetParticipantImportString(
            CompetitionParticipantImport participantImport)
        {
            return string.Join(
                "/",
                participantImport.RegPartAName,
                participantImport.RegPartAOrgId,
                participantImport.RegPartBName,
                participantImport.RegPartBOrgId,
                participantImport.RegClubName,
                participantImport.RegClubOrgId);
        }

        public DateTime ParseDateStrings(
            string? dateString)
        {
            if (string.IsNullOrEmpty(
                dateString))
            {
                return DateTime.MinValue;
            }

            return DateTime.ParseExact(
                dateString,
                "yyyy-M-d",
                null);
        }

        public string CombineFirstLastName(
            string? firstName,
            string? lastName)
        {
            return string
                .Format(
                    "{0} {1}",
                    firstName?.Trim(),
                    lastName?.Trim())
                .Trim();

        }
    }
}
