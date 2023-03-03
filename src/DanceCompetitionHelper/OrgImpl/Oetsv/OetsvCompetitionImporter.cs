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

        public Encoding HtmlEncodingCompetition { get; set; } = Encoding.UTF8;
        public Encoding HtmlEncodingParticipants { get; set; } = Encoding.GetEncoding(1252);

        public const int PartExcelOrgCompId = 0;
        public const int PartExcelOrgCompClassId = 1;
        public const int PartExcelStartNumber = 2;
        public const int PartExcelPart01LastName = 3;
        public const int PartExcelPart01FirstName = 4;
        public const int PartExcelPart01OrgId = 16;
        public const int PartExcelPart02LastName = 5;
        public const int PartExcelPart02FirstName = 6;
        public const int PartExcelPart02OrgId = 17;
        public const int PartExcelClubName = 7;
        public const int PartExcelClubOrgId = 18;
        public const int PartExcelState = 8;
        public const int PartExcelStateAbbr = 9;
        public const int PartExcelDiscipline = 11;
        public const int PartExcelAgeClass = 12;
        public const int PartExcelAgeGroup = 13;
        public const int PartExcelClass = 14;

        public const int PartExcelClassOrgSta = 21;
        public const int PartExcelOrgPointsSta = 19;
        public const int PartExcelOrgStartsSta = 20;
        public const int PartExcelOrgMinPointsForPromotionSta = 22;
        public const int PartExcelOrgMinStartsForPromotionSta = 23;

        public const int PartExcelClassOrgLa = 27;
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
            ILogger<OetsvCompetitionImporter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(
                nameof(logger));
        }

        public void ImportOrUpdateByFile(
            DanceCompetitionHelperDbContext dbCtx,
            string? fullPathCompetition,
            string? fullPathCompetitionClasses,
            string? fullPathParticipants)
        {
            ExtractData(
                fullPathCompetition,
                fullPathParticipants);
            ImportOrUpdateDatabase(
                dbCtx);
        }

        public void ImportOrUpdateByUrl(
            DanceCompetitionHelperDbContext dbCtx,
            Uri? uriCompetition,
            Uri? uriCompetitionClasses,
            Uri? uriParticipants)
        {
            // TODO: should we save at special folder for
            // "possible checks"/debugging?..
            var fullPathCompetitionCsv = DownloadFile(
                uriCompetition,
                HtmlEncodingCompetition);
            var fullPathParticipantsCsv = DownloadFile(
                uriParticipants,
                HtmlEncodingParticipants);

            try
            {
                ImportOrUpdateByFile(
                    dbCtx,
                    fullPathCompetitionCsv,
                    null,
                    fullPathParticipantsCsv);
            }
            finally
            {
                if (string.IsNullOrEmpty(
                    fullPathCompetitionCsv) == false
                    && File.Exists(
                        fullPathCompetitionCsv))
                {
                    File.Delete(
                        fullPathCompetitionCsv);
                }

                if (string.IsNullOrEmpty(
                    fullPathParticipantsCsv) == false
                    && File.Exists(
                        fullPathParticipantsCsv))
                {
                    File.Delete(
                        fullPathParticipantsCsv);
                }
            }
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
            Uri? downloadUri,
            Encoding encoding)
        {
            if (downloadUri == null)
            {
                return null;
            }

            var retFilePath = Path.GetTempFileName();
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

        public void ExtractData(
            string? fullPathCompetition,
            string? fullPathParticipants)
        {
            // ----------------------------------------
            if (string.IsNullOrEmpty(
                fullPathCompetition)
                || File.Exists(
                    fullPathCompetition) == false)
            {
                return;
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
                return;
            }

            foreach (var curLine in File.ReadAllLines(
                fullPathParticipants))
            {
                ExtractCompetitionParticipants(
                    curLine.Split(
                        new[] { ";" },
                        StringSplitOptions.TrimEntries));
            }
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
            if (competitionInfo.Length < 3)
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
                || OrgCompetitionId != participantInfo[PartExcelOrgCompId])
            {
                return;
            }

            var addPartImport = new CompetitionParticipantImport()
            {
                OrgClassIdRaw = participantInfo[PartExcelOrgCompClassId],
                OrgClassId = participantInfo[PartExcelOrgCompClassId].Trim(),

                StartNumberRaw = participantInfo[PartExcelStartNumber],
                StartNumber = int.Parse(
                    participantInfo[PartExcelStartNumber]),

                Part01FirstNameRaw = participantInfo[PartExcelPart01FirstName],
                Part01LastNameRaw = participantInfo[PartExcelPart01LastName].Trim(),

                Part02FirstNameRaw = participantInfo[PartExcelPart02FirstName],
                Part02LastNameRaw = participantInfo[PartExcelPart02LastName],

                ClubNameRaw = participantInfo[PartExcelClubName],
                ClubName = participantInfo[PartExcelClubName].Trim(),

                ClubOrgIdRaw = participantInfo[PartExcelClubOrgId],
                ClubOrgId = participantInfo[PartExcelClubOrgId].Trim(),

                StateRaw = participantInfo[PartExcelState],
                State = participantInfo[PartExcelState].Trim(),

                StateAbbrRaw = participantInfo[PartExcelStateAbbr],
                StateAbbr = participantInfo[PartExcelStateAbbr].Trim(),

                DisciplineRaw = participantInfo[PartExcelDiscipline],
                Discipline = OetsvConstants.Disciplines.ToDisciplines(
                    participantInfo[PartExcelDiscipline]),

                ClassRaw = participantInfo[PartExcelClass],
                Class = OetsvConstants.Classes.ToClasses(
                    participantInfo[PartExcelClass]),

                AgeClassRaw = participantInfo[PartExcelAgeClass],
                AgeClass = OetsvConstants.AgeClasses.ToAgeClasses(
                    participantInfo[PartExcelAgeClass]),

                AgeGroupRaw = participantInfo[PartExcelAgeGroup],
                AgeGroup = OetsvConstants.AgeGroups.ToAgeGroup(
                    participantInfo[PartExcelAgeGroup]),
            };

            addPartImport.Part01Name = CombineFirstLastName(
                addPartImport.Part01FirstNameRaw,
                addPartImport.Part01LastNameRaw);
            addPartImport.Part02Name = CombineFirstLastName(
                addPartImport.Part02FirstNameRaw,
                addPartImport.Part02LastNameRaw);

            switch (addPartImport.Discipline)
            {
                case OetsvConstants.Disciplines.Sta:
                    addPartImport.ClassOrgRaw = participantInfo[PartExcelClassOrgSta];
                    addPartImport.ClassOrg = OetsvConstants.Classes.ToClasses(
                        participantInfo[PartExcelClassOrgSta]);

                    addPartImport.OrgPointsRaw = participantInfo[PartExcelOrgPointsSta];
                    addPartImport.OrgPoints = int.Parse(
                        participantInfo[PartExcelOrgPointsSta]);

                    addPartImport.OrgStartsRaw = participantInfo[PartExcelOrgStartsSta];
                    addPartImport.OrgStarts = int.Parse(
                        participantInfo[PartExcelOrgStartsSta]);

                    addPartImport.MinPointsForPromotionRaw = participantInfo[PartExcelOrgMinPointsForPromotionSta];
                    addPartImport.MinPointsForPromotion = int.Parse(
                        participantInfo[PartExcelOrgMinPointsForPromotionSta]);

                    addPartImport.MinStartsForPromotionRaw = participantInfo[PartExcelOrgMinStartsForPromotionSta];
                    addPartImport.MinStartsForPromotion = int.Parse(
                        participantInfo[PartExcelOrgMinStartsForPromotionSta]);

                    break;

                case OetsvConstants.Disciplines.La:
                    addPartImport.ClassOrgRaw = participantInfo[PartExcelClassOrgLa];
                    addPartImport.ClassOrg = OetsvConstants.Classes.ToClasses(
                        participantInfo[PartExcelClassOrgLa]);

                    addPartImport.OrgPointsRaw = participantInfo[PartExcelOrgPointsLa];
                    addPartImport.OrgPoints = int.Parse(
                        participantInfo[PartExcelOrgPointsLa]);

                    addPartImport.OrgStartsRaw = participantInfo[PartExcelOrgStartsLa];
                    addPartImport.OrgStarts = int.Parse(
                        participantInfo[PartExcelOrgStartsLa]);

                    addPartImport.MinPointsForPromotionRaw = participantInfo[PartExcelOrgMinPointsForPromotionLa];
                    addPartImport.MinPointsForPromotion = int.Parse(
                        participantInfo[PartExcelOrgMinPointsForPromotionLa]);

                    addPartImport.MinStartsForPromotionRaw = participantInfo[PartExcelOrgMinStartsForPromotionLa];
                    addPartImport.MinStartsForPromotion = int.Parse(
                        participantInfo[PartExcelOrgMinStartsForPromotionLa]);

                    break;
            }

            Participants.Add(
                addPartImport);
        }

        public void ImportOrUpdateDatabase(
            DanceCompetitionHelperDbContext dbCtx)
        {
            var foundComp = dbCtx.Competitions
                .TagWith(
                    nameof(ImportOrUpdateDatabase) + "-01")
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
                    nameof(ImportOrUpdateDatabase) + "-02")
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
                    nameof(ImportOrUpdateDatabase) + "-03")
                .Where(
                    x => x.CompetitionId == foundComp.CompetitionId)
                .ToDictionary(
                    x => x.OrgClassId);

            foreach (var curImportCompClass in CompetitionClasses)
            {
                if (string.IsNullOrEmpty(
                    curImportCompClass.OrgClassId))
                {
                    _logger.LogWarning(
                        "{PropertyName} '{OrgClassId}' is invalid/missing! Ignore {ImportCompetitionClass}",
                        nameof(curImportCompClass.OrgClassId),
                        curImportCompClass.OrgClassId,
                        curImportCompClass);

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
                    nameof(ImportOrUpdateDatabase) + "-04")
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
                    curImportPart.OrgClassId,
                    GetParticipantImportString(
                        curImportPart));

            }
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
                participantImport.Part01Name,
                participantImport.Part01OrgId,
                participantImport.Part02Name,
                participantImport.Part02OrgId,
                participantImport.ClubName,
                participantImport.ClubOrgId);
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
