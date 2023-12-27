using DanceCompetitionHelper.Config;
using DanceCompetitionHelper.Database;
using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Extensions;
using DanceCompetitionHelper.OrgImpl.Oetsv.WorkData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Net;
using System.Text;

namespace DanceCompetitionHelper.OrgImpl.Oetsv
{
    public class OetsvCompetitionImporter : ICompetitionImporter
    {
        private readonly ILogger<OetsvCompetitionImporter> _logger;
        private readonly ImporterSettings _importerSettings;

        public DanceCompetitionHelper? DanceCompetitionHelper { get; set; }
        public DanceCompetitionHelperDbContext? DbCtx => DanceCompetitionHelper?.GetDbCtx();

        public static readonly List<Color> CompetitionClassColors = new List<Color>()
        {
            // 0
            Color.AliceBlue,
            Color.AntiqueWhite,
            Color.Aquamarine,
            Color.BlueViolet,
            Color.CadetBlue,
            Color.Coral,
            Color.Cyan,
            Color.DarkGoldenrod,
            Color.DarkGreen,
            Color.DarkOrange,
            // 10
            Color.LightSlateGray,
            Color.MediumSeaGreen,
            Color.Pink,
            Color.RosyBrown,
            Color.White,
            Color.Silver,
            Color.OrangeRed,
            Color.DarkSalmon,
            Color.Gold,
            Color.Honeydew,
        };

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

        public bool FindFollowUpClasses { get; set; }

        #region Constants...

        public Encoding HtmlEncodingCompetition { get; set; } = Encoding.GetEncoding(1252); // Encoding.UTF8;
        public Encoding HtmlEncodingParticipants { get; set; } = Encoding.GetEncoding(1252);

        public const int CompExcelOrgClassId = 2;
        public const int CompExcelAgeClass = 3;
        public const int CompExcelAgeGroup = 4;
        public const int CompExcelClass = 5;
        public const int CompExcelDiscipline = 6;
        public const int CompExcelName = 7;
        public const int CompExcelDances = 8;

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
            string orgCompetitionId,
            string? fullPathCompetition,
            string? fullPathCompetitionClasses,
            string? fullPathParticipants)
        {
            var retWorkInfo = new List<string>();

            try
            {
                retWorkInfo.AddRange(
                    ExtractData(
                        fullPathCompetition,
                        fullPathParticipants));
                retWorkInfo.AddRange(
                    ImportOrUpdateDatabase());
            }
            catch (Exception exc)
            {
                var errorStr = string.Format(
                    "Error during '{0}': {1}",
                    nameof(ImportOrUpdateByFile),
                    exc.Message);

                retWorkInfo.Add(
                    errorStr);
                _logger.LogError(
                    exc,
                    errorStr);
            }

            return retWorkInfo;
        }

        public List<string> ImportOrUpdateByUrl(
            string orgCompetitionId,
            Uri? uriUpdate,
            Uri? uriCompetition,
            Uri? uriCompetitionClasses,
            Uri? uriParticipants)
        {
            var retWorkInfo = new List<string>();
            try
            {
                CallUpdateUri(
                    uriUpdate);

                var fullPathCompetitionCsv = DownloadFile(
                    "comp",
                    orgCompetitionId,
                    uriCompetition,
                    HtmlEncodingCompetition);
                var fullPathParticipantsCsv = DownloadFile(
                    "part",
                    orgCompetitionId,
                    uriParticipants,
                    HtmlEncodingParticipants);

                retWorkInfo.AddRange(
                    ImportOrUpdateByFile(
                        orgCompetitionId,
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

                retWorkInfo.Add(
                    errorStr);
                _logger.LogError(
                    exc,
                    errorStr);
            }

            return retWorkInfo;
        }

        public Uri GetUpdateUriForOrgId(
            int orgId)
        {
            return new Uri(
                string.Format(
                    "https://www.tanzsportverband.at/portal/nennungen/starterliste-xls.php?lang=DE&TASNr={0}",
                    orgId
                        .ToString(
                            "D0")
                        .PadLeft(
                            4,
                            '0')));
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

        public void CallUpdateUri(
            Uri? updateUri)
        {
            if (updateUri == null)
            {
                return;
            }

            try
            {
                using var httpClient = new HttpClient()
                {
                    Timeout = TimeSpan.FromMinutes(1),
                };

                var retTask = httpClient
                    .GetAsync(
                        updateUri);
                retTask.Wait();
                var getResp = retTask.Result;

                if (getResp.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(
                        string.Format(
                            "StatuCode {0}",
                            getResp.StatusCode));
                }
            }
            catch (Exception exc)
            {
                _logger.LogError(
                    exc,
                    "Error during updating '{Uri}': {Message}",
                    updateUri,
                    exc.Message);

                throw;
            }
        }

        public string? DownloadFile(
            string fileType,
            string orgCompId,
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
                        "{0}_{1}_{2}_download.csv",
                        DateTime.Now.ToString(
                            "yyyyMMdd_HHmmss"),
                        orgCompId,
                        fileType));
            }

            using var useFileStream = File.OpenWrite(
                retFilePath);

            try
            {
                using var httpClient = new HttpClient();

                var retTask = httpClient
                    .GetAsync(
                        downloadUri);
                retTask.Wait();
                var getResp = retTask.Result;

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
            var retWorkInfo = new List<string>();

            // ----------------------------------------
            if (File.Exists(
                fullPathCompetition))
            {
                foreach (var curLine in File.ReadAllLines(
                    fullPathCompetition))
                {
                    var methodName = string.Empty;

                    try
                    {

                        var lineItems = curLine.Split(
                            new[] { ";" },
                            StringSplitOptions.TrimEntries);

                        if (lineItems.Length >= 2)
                        {
                            switch (lineItems[0].ToUpperInvariant())
                            {
                                case "TSD":
                                    methodName = nameof(ExtractCompetitionInfo);
                                    ExtractCompetitionInfo(
                                        lineItems);
                                    break;

                                case "TKL":
                                    methodName = nameof(ExtractCompetitionClasses);
                                    ExtractCompetitionClasses(
                                        lineItems);
                                    break;

                                case "FUNKT":
                                    methodName = nameof(ExtractOfficials);
                                    ExtractOfficials(
                                        lineItems);
                                    break;
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        var logStr = string.Format(
                            "Error during '{0}' for '{1}': {2}",
                            methodName,
                            curLine,
                            exc.Message);

                        _logger.LogError(
                            exc,
                            logStr);

                        retWorkInfo.Add(
                            logStr);
                    }
                }
            }

            // ----------------------------------------
            if (File.Exists(
                fullPathParticipants))
            {
                foreach (var curLine in File.ReadAllLines(
                    fullPathParticipants))
                {
                    try
                    {
                        ExtractCompetitionParticipants(
                            curLine.Split(
                                new[] { ";" },
                                StringSplitOptions.TrimEntries));
                    }
                    catch (Exception exc)
                    {
                        var logStr = string.Format(
                            "Error during '{0}' for '{1}': {2}",
                            nameof(ExtractCompetitionParticipants),
                            curLine,
                            exc.Message);

                        _logger.LogError(
                            exc,
                            logStr);

                        retWorkInfo.Add(
                            logStr);
                    }
                }
            }

            return retWorkInfo;
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
                    OrgClassIdRaw = competitionInfo[CompExcelOrgClassId],
                    // for sorting
                    OrgClassId = competitionInfo[CompExcelOrgClassId].Trim().PadLeft(3, '0'),

                    AgeClassRaw = competitionInfo[CompExcelAgeClass],
                    AgeClass = OetsvConstants.AgeClasses.ToAgeClasses(
                        competitionInfo[CompExcelAgeClass]),

                    AgeGroupRaw = competitionInfo[CompExcelAgeGroup],
                    AgeGroup = OetsvConstants.AgeGroups.ToAgeGroup(
                        competitionInfo[CompExcelAgeGroup]),

                    ClassRaw = competitionInfo[CompExcelClass],
                    Class = OetsvConstants.Classes.ToClasses(
                        competitionInfo[CompExcelClass]),

                    DisciplineRaw = competitionInfo[CompExcelDiscipline],
                    Discipline = OetsvConstants.Disciplines.ToDisciplines(
                        competitionInfo[CompExcelDiscipline]),

                    NameRaw = competitionInfo[CompExcelName],
                    Name = competitionInfo[CompExcelName]
                        .Trim(
                            '*')
                        .Trim(),

                    DancesRaw = competitionInfo[CompExcelDances] ?? string.Empty,
                    Dances = competitionInfo[CompExcelDances]
                        ?.Trim()
                         ?? string.Empty,
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
            if (string.IsNullOrEmpty(OrgCompetitionId))
            {
                OrgCompetitionId = participantInfo[PartExcelRegOrgCompId].Trim();
            }

            if (participantInfo.Length < 32
                || OrgCompetitionId != participantInfo[PartExcelRegOrgCompId])
            {
                return;
            }

            var addPartImport = new CompetitionParticipantImport()
            {
                RegOrgClassIdRaw = participantInfo[PartExcelRegOrgCompClassId],
                // for sorting
                RegOrgClassId = participantInfo[PartExcelRegOrgCompClassId].Trim().PadLeft(3, '0'),

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

            Participants.Add(
                addPartImport);
        }

        public List<string> ImportOrUpdateDatabase()
        {
            var retWorkInfo = new List<string>();
            var dbCtx = DbCtx ?? throw new ArgumentNullException(
                nameof(DbCtx));

            var foundComp = DbCtx.Competitions
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

                _logger.LogInformation(
                    "Added {CompName}: {foundComp}",
                    nameof(Competition),
                    foundComp);
                retWorkInfo.Add(
                    string.Format(
                        "Added {0}: {1}",
                        nameof(Competition),
                        foundComp));
            }
            else
            {
                _logger.LogInformation(
                    "Update existing {CompName}:  {foundComp}",
                    nameof(Competition),
                    foundComp);
                retWorkInfo.Add(
                    string.Format(
                        "Update existing {0}:  {1}",
                        nameof(Competition),
                        foundComp));

                foundComp.CompetitionName = CompetitionName;
                foundComp.CompetitionInfo = string.Format(
                    "{0} {1} {2}",
                    CompetitionType,
                    CompetitionLocation,
                    CompetitionAddress);
                foundComp.CompetitionDate = CompetitionDate;

                _logger.LogInformation(
                    "Updated existing {CompName}: {foundComp}",
                    nameof(Competition),
                    foundComp);
            }

            var foundAdjPanels = dbCtx.AdjudicatorPanels
                .TagWith(
                    nameof(ImportOrUpdateDatabase) + "[02]")
                .Where(
                    x => x.CompetitionId == foundComp.CompetitionId)
                .ToList();

            if (foundAdjPanels.Count <= 0)
            {
                var newAdjPanel = dbCtx.AdjudicatorPanels.Add(
                    new AdjudicatorPanel()
                    {
                        Competition = foundComp,
                        Name = "Panel 1",
                    }).Entity;
                foundAdjPanels.Add(
                    newAdjPanel);

                _logger.LogInformation(
                    "Added {AdjPanName}: {newAdjPanel}",
                    nameof(AdjudicatorPanel),
                    newAdjPanel);
                retWorkInfo.Add(
                    string.Format(
                        "Added {0}: {1}",
                        nameof(AdjudicatorPanel),
                        newAdjPanel));

                var adjAbbr = 'A';
                foreach (var curAdj in Adjudicators)
                {
                    var newAdj = dbCtx.Adjudicators.Add(
                        new Adjudicator()
                        {
                            AdjudicatorPanel = newAdjPanel,
                            Name = curAdj,
                            Abbreviation = adjAbbr.ToString(),
                        }).Entity;

                    _logger.LogInformation(
                        "Added {AdjudicatorName}: {newAdj}",
                        nameof(Adjudicator),
                        newAdj);
                    retWorkInfo.Add(
                        string.Format(
                            "Added {0}: {1}",
                            nameof(Adjudicator),
                            newAdj));

                    adjAbbr++;
                }
            }
            else
            {
                if (foundAdjPanels.Count > 1)
                {
                    _logger.LogWarning(
                        "Unable to Update multiple {AdjPanName}s",
                        nameof(AdjudicatorPanel));
                    retWorkInfo.Add(
                        string.Format(
                            "!! Unable to Update multiple {0}s",
                            nameof(AdjudicatorPanel)));
                }
                else
                {
                    var curAdjPanel = foundAdjPanels[0];

                    _logger.LogInformation(
                        "Update existing {AdjPanName}:  {foundComp}",
                        nameof(AdjudicatorPanel),
                        curAdjPanel);
                    retWorkInfo.Add(
                        string.Format(
                            "Update existing {0}: {1}",
                            nameof(AdjudicatorPanel),
                            curAdjPanel));

                    var useUpdateDate = DateTime.Now.ToString(
                        "yyyy.MM.dd HH:mm:ss");
                    var adjAbbr = 'A';
                    foreach (var curAdj in Adjudicators)
                    {
                        var logString01 = "Update ";
                        var logString02 = "Updated ";
                        var curAdjudicator = dbCtx.Adjudicators.FirstOrDefault(
                            x => x.AdjudicatorPanelId == curAdjPanel.AdjudicatorPanelId
                            && x.Abbreviation == adjAbbr.ToString());

                        if (curAdjudicator == null)
                        {
                            logString01 = "Add ";
                            logString02 = "Added ";
                            curAdjudicator = dbCtx.Adjudicators.Add(
                                new Adjudicator()
                                {
                                    AdjudicatorPanel = curAdjPanel,
                                    Name = curAdj,
                                    Abbreviation = adjAbbr.ToString(),
                                }).Entity;
                        }

                        _logger.LogInformation(
                            "{op} {AdjPanName}:  {foundComp}",
                            logString01,
                            nameof(Adjudicator),
                            curAdjPanel);
                        retWorkInfo.Add(
                            string.Format(
                                "{0} {1}: {2}",
                                logString01,
                                nameof(Adjudicator),
                                curAdjPanel));

                        curAdjudicator.Name = curAdj;
                        curAdjudicator.Comment += string.Format(
                            "Updated at {0}",
                            useUpdateDate);

                        adjAbbr++;

                        _logger.LogInformation(
                            "{op} {AdjPanName}: {foundComp}",
                            logString02,
                            nameof(Adjudicator),
                            curAdjPanel);
                    }
                }
            }

            var createdDatabaseNames = new HashSet<string>();
            var allCompClasses = dbCtx.CompetitionClasses
                .TagWith(
                    nameof(ImportOrUpdateDatabase) + "[03]")
                .Where(
                    x => x.CompetitionId == foundComp.CompetitionId)
                .ToDictionary(
                    x => x.OrgClassId);
            var allCreatedCompClasses = new HashSet<CompetitionClass>();
            var useColorIdx = 0;

            // TODO: check "missing"/"deleted" participants 
            foreach (var curImportCompClass in CompetitionClasses)
            {
                if (string.IsNullOrEmpty(
                    curImportCompClass.OrgClassId))
                {
                    _logger.LogWarning(
                        "{OrgClassIdName} '{OrgClassId}' is invalid/missing! Ignore {curImportCompClass}",
                        nameof(curImportCompClass.OrgClassId),
                        curImportCompClass.OrgClassId,
                        curImportCompClass);
                    retWorkInfo.Add(
                        string.Format(
                            "{0} '{1}' is invalid/missing! Ignore {2}",
                            nameof(curImportCompClass.OrgClassId),
                            curImportCompClass.OrgClassId,
                        curImportCompClass));

                    continue;
                }

                if (allCompClasses.TryGetValue(
                    curImportCompClass.OrgClassId,
                    out var foundCompetitionClass))
                {
                    _logger.LogInformation(
                        "Update existing {CompClassName}:  {foundCompetitionClass}",
                        nameof(CompetitionClass),
                        foundCompetitionClass);
                    retWorkInfo.Add(
                        string.Format(
                            "Update existing {0}:  {1}",
                            nameof(CompetitionClass),
                            foundCompetitionClass));

                    var useClassName = curImportCompClass.GetClassName() ?? foundCompetitionClass.CompetitionClassName;

                    // we need a "unique" name...
                    while (createdDatabaseNames.Contains(
                        useClassName))
                    {
                        useClassName += " " + curImportCompClass.OrgClassId;
                    }

                    createdDatabaseNames.Add(
                        useClassName);

                    foundCompetitionClass.CompetitionClassName = useClassName;
                    foundCompetitionClass.Discipline = curImportCompClass.Discipline ?? foundCompetitionClass.Discipline;
                    foundCompetitionClass.AgeClass = curImportCompClass.AgeClass ?? foundCompetitionClass.AgeClass;
                    foundCompetitionClass.AgeGroup = curImportCompClass.AgeGroup ?? foundCompetitionClass.AgeGroup;
                    foundCompetitionClass.Class = curImportCompClass.Class ?? foundCompetitionClass.Class;

                    _logger.LogInformation(
                        "Updated existing {CompClassName}: {foundCompetitionClass}",
                        nameof(CompetitionClass),
                        foundCompetitionClass);
                }
                else
                {
                    var useClassName = curImportCompClass.GetClassName();

                    // we need a "unique" name...
                    while (createdDatabaseNames.Contains(
                        useClassName))
                    {
                        useClassName += " " + curImportCompClass.OrgClassId;
                    }

                    createdDatabaseNames.Add(
                        useClassName);

                    // let's add it...
                    var createdCompetitionClass = dbCtx.CompetitionClasses.Add(
                        new CompetitionClass()
                        {
                            Competition = foundComp,
                            OrgClassId = curImportCompClass.OrgClassId,
                            AdjudicatorPanel = foundAdjPanels.First(),
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
                            CompetitionColor =
                                CompetitionClassColors[useColorIdx++ % CompetitionClassColors.Count].ToRgbHexString(),
                        })
                        .Entity;

                    allCompClasses[createdCompetitionClass.OrgClassId] = createdCompetitionClass;
                    allCreatedCompClasses.Add(
                        createdCompetitionClass);

                    _logger.LogInformation(
                        "Added {CompClassName}: {createdCompetitionClass}",
                        nameof(CompetitionClass),
                        createdCompetitionClass);
                    retWorkInfo.Add(
                        string.Format(
                            "Added {0}: {1}",
                            nameof(CompetitionClass),
                            createdCompetitionClass));
                }
            }

            // process "follow up"
            if (FindFollowUpClasses)
            {
                ICompetitonClassChecker? compClassChecker = DanceCompetitionHelper?.GetCompetitonClassChecker(
                    foundComp);

                if (compClassChecker != null)
                {
                    foreach (var curNewClass in allCreatedCompClasses)
                    {
                        curNewClass.FollowUpCompetitionClass = (compClassChecker?
                            .GetHigherClassifications(
                                curNewClass,
                                allCompClasses.Values)
                                ?? Enumerable.Empty<CompetitionClass>())
                            .FirstOrDefault();
                    }
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
                    _logger.LogWarning(
                        "Unable to find {OrgClassId} '{RegOrgClassId}' of Import-Participant {curImportPart}",
                        nameof(CompetitionClass.OrgClassId),
                        curImportPart.RegOrgClassId,
                        curImportPart);
                    retWorkInfo.Add(
                        string.Format(
                            "Unable to find {0} '{1}' of Import-Participant {2}",
                            nameof(CompetitionClass.OrgClassId),
                            curImportPart.RegOrgClassId,
                            curImportPart));

                    continue;
                }


                // TODO: check "missing"/"deleted" participants 
                var isUpdate = false;
                if (allParticipantsByImpStr.TryGetValue(
                    curPartImpString,
                    out var useParticipant))
                {
                    _logger.LogInformation(
                        "Update existing {PartName}:  {useParticipant}",
                        nameof(Participant),
                        useParticipant);
                    retWorkInfo.Add(
                        string.Format(
                            "Update existing {0}:  {1}",
                            nameof(Participant),
                            useParticipant));

                    useParticipant.StartNumber = curImportPart.RegStartNumber ?? 0;
                    useParticipant.OrgPointsPartA = curImportPart.OrgPoints ?? 0;
                    useParticipant.OrgStartsPartA = curImportPart.OrgStarts ?? 0;

                    isUpdate = true;
                }
                else
                {
                    useParticipant = dbCtx.Participants.Add(
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

                    allParticipantsByImpStr[curPartImpString] = useParticipant;
                }

                if (useParticipant.OrgPointsPartA >= useCompClass.MinPointsForPromotion
                    && useParticipant.OrgStartsPartA >= useCompClass.MinStartsForPromotion)
                {
                    useParticipant.OrgAlreadyPromotedPartA = true;
                    useParticipant.OrgAlreadyPromotedInfoPartA = string.Format(
                        "Points and starts exeeds Promotion Limits");
                }

                var checkAlreadyPromoted = true;
                if (useCompClass.Class == OetsvConstants.Classes.Amateur
                    && curImportPart.RegClass == OetsvConstants.Classes.GirlsOnly)
                {
                    // girls only... ignore this...
                    checkAlreadyPromoted = false;
                }

                // None OeTSV members
                if (OetsvConstants.Participants.CheckValidOetsvParticipantOrgId(
                    curImportPart.RegPartAOrgId) == false
                    || OetsvConstants.Participants.CheckValidOetsvParticipantOrgId(
                        curImportPart.RegPartBOrgId) == false)
                {
                    checkAlreadyPromoted = false;
                }

                if (checkAlreadyPromoted
                    && (curImportPart.OrgCurrentClass != useCompClass.Class
                    || curImportPart.OrgCurrentClass != curImportPart.RegClass))
                {
                    useParticipant.OrgAlreadyPromotedPartA = true;
                    useParticipant.OrgAlreadyPromotedInfoPartA = string.Format(
                        "Registration Class '{0}' != Import/Org Class: '{1}' ({2})",
                        curImportPart.RegClass,
                        curImportPart.OrgCurrentClass,
                        curImportPart.OrgCurrentClassRaw);
                }

                if (isUpdate)
                {
                    _logger.LogInformation(
                        "Updated existing {PartName}: {useParticipant}",
                        nameof(Participant),
                        useParticipant);
                    retWorkInfo.Add(
                        string.Format(
                            "Updated existing {0}: {1}",
                            nameof(Participant),
                            useParticipant));
                }
                else
                {
                    _logger.LogInformation(
                        "Added {PartName}: {useParticipant}",
                        nameof(Participant),
                        useParticipant);
                    retWorkInfo.Add(
                        string.Format(
                            "Added {0}: {1}",
                            nameof(Participant),
                            useParticipant));
                }
            }

            return retWorkInfo;
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
