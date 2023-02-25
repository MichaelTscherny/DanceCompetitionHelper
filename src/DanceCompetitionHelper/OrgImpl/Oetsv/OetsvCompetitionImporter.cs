using DanceCompetitionHelper.Database;
using Microsoft.Extensions.Logging;

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

        public List<string> CompetitionClasses { get; } = new List<string>();
        public List<string> MastersOfCeremony { get; } = new List<string>();
        public List<string> Chairperson { get; } = new List<string>();
        public List<string> Assessors { get; } = new List<string>();
        public List<string> Adjudicators { get; } = new List<string>();

        public OetsvCompetitionImporter(
            ILogger<OetsvCompetitionImporter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(
                nameof(logger));
        }

        public void ImportByFile(
            DanceCompetitionHelperDbContext dbCtx,
            string fullPath)
        {
            ExtractData(
                fullPath);
            ImportToDatabase();
        }

        public void ImportByUrl(
            DanceCompetitionHelperDbContext dbCtx,
            Uri fromUri)
        {

        }

        public void ExtractData(
            string fullPath)
        {
            foreach (var curLine in File.ReadLines(
                fullPath))
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
                    CompetitionDate = DateTime.ParseExact(
                        orgInfoValue,
                        "yyyy-M-d",
                        null);
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
                OrgCompetitionId = competitionInfo[1];
            }

            CompetitionClasses.Add(
                string.Join(
                    ";",
                    competitionInfo[3],
                    competitionInfo[4],
                    competitionInfo[5],
                    competitionInfo[6],
                    competitionInfo[7]));
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

        public void ImportToDatabase()
        {

        }
    }
}
