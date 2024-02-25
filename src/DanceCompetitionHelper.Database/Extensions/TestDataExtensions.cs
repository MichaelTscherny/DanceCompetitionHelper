using Microsoft.EntityFrameworkCore;

namespace DanceCompetitionHelper.Database.Extensions
{
    public static class TestDataExtensions
    {
        public static async Task AddTestData(
            this DanceCompetitionHelperDbContext dbCtx,
            CancellationToken cancellationToken)
        {
            var useTrans = await dbCtx.BeginTransactionAsync(
                cancellationToken)
                ?? throw new ArgumentNullException(
                    "dbTrans");

            try
            {
                await AddSmallCompetitionsAsync(
                    dbCtx,
                    cancellationToken);
                await AddFathCompetitionAsync(
                    dbCtx,
                    cancellationToken);

                await dbCtx.SaveChangesAsync(
                    cancellationToken);
                await useTrans.CommitAsync(
                    cancellationToken);
            }
            catch (Exception exc)
            {
                Console.WriteLine(
                    exc);
                await (useTrans?.RollbackAsync(
                    cancellationToken) ?? Task.CompletedTask);
            }
        }

        private static async Task AddSmallCompetitionsAsync(
            DanceCompetitionHelperDbContext dbCtx,
            CancellationToken cancellationToken)
        {
            // we only add data if we do not have any...
            if (await dbCtx.Competitions
                .TagWith(
                    nameof(AddSmallCompetitionsAsync) + "(db)[0]")
                .CountAsync(
                    cancellationToken) >= 1)
            {
                return;
            }

            // ---------------------------------------------
            // Test-Comp 01
            var newComp01 = dbCtx.Competitions.Add(
                new Tables.Competition()
                {
                    Organization = Enum.OrganizationEnum.Oetsv,
                    CompetitionName = "Test Comp 01",
                    OrgCompetitionId = "org-Id 01",
                    CompetitionDate = DateTime.Now.AddDays(2),
                    CompetitionInfo = "just an info",
                    Comment = "yet another comp",
                });

            // ----
            var newAdjPanel0101 = dbCtx.AdjudicatorPanels.Add(
                new Tables.AdjudicatorPanel()
                {
                    Competition = newComp01.Entity,
                    Name = "Panel 1-1",
                });
            dbCtx.Adjudicators.Add(
                new Tables.Adjudicator()
                {
                    AdjudicatorPanel = newAdjPanel0101.Entity,
                    Name = "Adj 01-01",
                    Abbreviation = "01-01",
                });
            dbCtx.Adjudicators.Add(
                new Tables.Adjudicator()
                {
                    AdjudicatorPanel = newAdjPanel0101.Entity,
                    Name = "Adj 01-02",
                    Abbreviation = "01-02",
                });
            dbCtx.Adjudicators.Add(
                new Tables.Adjudicator()
                {
                    AdjudicatorPanel = newAdjPanel0101.Entity,
                    Name = "Adj 01-03",
                    Abbreviation = "01-03",
                });
            dbCtx.Adjudicators.Add(
                new Tables.Adjudicator()
                {
                    AdjudicatorPanel = newAdjPanel0101.Entity,
                    Name = "Adj 01-04",
                    Abbreviation = "01-04",
                });
            dbCtx.Adjudicators.Add(
                new Tables.Adjudicator()
                {
                    AdjudicatorPanel = newAdjPanel0101.Entity,
                    Name = "Adj 01-05",
                    Abbreviation = "01-05",
                });

            // ----
            var newCompClass0101 = dbCtx.CompetitionClasses.Add(
                new Tables.CompetitionClass()
                {
                    Competition = newComp01.Entity,
                    CompetitionClassName = "Allg. Sta D",
                    AdjudicatorPanel = newAdjPanel0101.Entity,
                    OrgClassId = "1",
                    Discipline = "Sta",
                    AgeClass = "Allg",
                    AgeGroup = null,
                    Class = "D",
                    MinStartsForPromotion = 10,
                    MinPointsForPromotion = 900,
                    PointsForFirst = 100,
                    ExtraManualStarter = 0,
                    Comment = "a comment",
                });
            // ----
            var newCompVanues0101 = dbCtx.CompetitionVenues.Add(
                new Tables.CompetitionVenue()
                {
                    Competition = newComp01.Entity,
                    Name = "Main Floor",
                    LengthInMeter = 10,
                    WidthInMeter = 8,
                    Comment = "nice floor",
                });
            // ----
            dbCtx.Participants.Add(
                new Tables.Participant()
                {
                    Competition = newComp01.Entity,
                    CompetitionClass = newCompClass0101.Entity,
                    StartNumber = 1,
                    NamePartA = "Dancer 01-A",
                    OrgIdPartA = "100",
                    OrgPointsPartA = 100,
                    OrgStartsPartA = 1,
                    OrgAlreadyPromotedPartA = true,
                    OrgAlreadyPromotedInfoPartA = "just a test!",
                    NamePartB = "Dancer 01-B",
                    OrgIdPartB = "101",
                    ClubName = "Club-01",
                    OrgIdClub = "110",
                });
            dbCtx.Participants.Add(
                new Tables.Participant()
                {
                    Competition = newComp01.Entity,
                    CompetitionClass = newCompClass0101.Entity,
                    StartNumber = 2,
                    NamePartA = "Dancer 02-A",
                    OrgIdPartA = "200",
                    OrgPointsPartA = 200,
                    OrgStartsPartA = 2,
                    NamePartB = "Dancer 02-B",
                    OrgIdPartB = "201",
                    ClubName = "Club-02",
                    OrgIdClub = "210",
                });

            var newCompClass0102 = dbCtx.CompetitionClasses.Add(
                new Tables.CompetitionClass()
                {
                    Competition = newComp01.Entity,
                    CompetitionClassName = "Allg. Sta C",
                    AdjudicatorPanel = newAdjPanel0101.Entity,
                    OrgClassId = "2",
                    Discipline = "Sta",
                    AgeClass = "Allg",
                    AgeGroup = null,
                    Class = "C",
                    MinStartsForPromotion = 10,
                    MinPointsForPromotion = 1200,
                    PointsForFirst = 100,
                    ExtraManualStarter = 0,
                    Comment = "don't mind",
                });
            // ----
            dbCtx.Participants.Add(
                new Tables.Participant()
                {
                    Competition = newComp01.Entity,
                    CompetitionClass = newCompClass0102.Entity,
                    StartNumber = 3,
                    NamePartA = "Dancer 03-A",
                    OrgIdPartA = "300",
                    OrgPointsPartA = 300,
                    OrgStartsPartA = 3,
                    NamePartB = "Dancer 03-B",
                    OrgIdPartB = "301",
                    ClubName = "Club-03",
                    OrgIdClub = "310",
                });

            // ---------------------------------------------
            // Test-Comp 02
            var newComp02 = dbCtx.Competitions.Add(
                new Tables.Competition()
                {
                    Organization = Enum.OrganizationEnum.Oetsv,
                    CompetitionName = "Test Comp 02",
                    OrgCompetitionId = "org-Id 02",
                    CompetitionDate = DateTime.Now.AddDays(4),
                    CompetitionInfo = "mixed info",
                    Comment = "too much to do...",
                });

            // ----
            var newAdjPanel0201 = dbCtx.AdjudicatorPanels.Add(
                new Tables.AdjudicatorPanel()
                {
                    Competition = newComp02.Entity,
                    Name = "Panel 2-1",
                });
            dbCtx.Adjudicators.Add(
                new Tables.Adjudicator()
                {
                    AdjudicatorPanel = newAdjPanel0201.Entity,
                    Name = "Adj 02-01",
                    Abbreviation = "02-01",
                });
            dbCtx.Adjudicators.Add(
                new Tables.Adjudicator()
                {
                    AdjudicatorPanel = newAdjPanel0201.Entity,
                    Name = "Adj 02-02",
                    Abbreviation = "02-02",
                });
            dbCtx.Adjudicators.Add(
                new Tables.Adjudicator()
                {
                    AdjudicatorPanel = newAdjPanel0201.Entity,
                    Name = "Adj 02-03",
                    Abbreviation = "02-03",
                });
            dbCtx.Adjudicators.Add(
                new Tables.Adjudicator()
                {
                    AdjudicatorPanel = newAdjPanel0201.Entity,
                    Name = "Adj 02-04",
                    Abbreviation = "02-04",
                });
            dbCtx.Adjudicators.Add(
                new Tables.Adjudicator()
                {
                    AdjudicatorPanel = newAdjPanel0201.Entity,
                    Name = "Adj 02-05",
                    Abbreviation = "02-05",
                });

            // ----
            var newCompClass0201 = dbCtx.CompetitionClasses.Add(
                new Tables.CompetitionClass()
                {
                    Competition = newComp02.Entity,
                    CompetitionClassName = "Allg. La D",
                    AdjudicatorPanel = newAdjPanel0201.Entity,
                    OrgClassId = "1",
                    Discipline = "La",
                    AgeClass = "Allg",
                    AgeGroup = null,
                    Class = "D",
                    MinStartsForPromotion = 10,
                    MinPointsForPromotion = 900,
                    PointsForFirst = 150,
                    ExtraManualStarter = 0,
                    Comment = "no comment",
                });
            // ----
            var newCompVanues0201 = dbCtx.CompetitionVenues.Add(
                new Tables.CompetitionVenue()
                {
                    Competition = newComp02.Entity,
                    Name = "Main Floor",
                    LengthInMeter = 15,
                    WidthInMeter = 10,
                    Comment = "'house' floor",
                });
            // ----
            dbCtx.Participants.Add(
                new Tables.Participant()
                {
                    Competition = newComp02.Entity,
                    CompetitionClass = newCompClass0201.Entity,
                    StartNumber = 1,
                    NamePartA = "Dancer 01-A",
                    OrgIdPartA = "1",
                    OrgPointsPartA = 100,
                    OrgStartsPartA = 1,
                });
            dbCtx.Participants.Add(
                new Tables.Participant()
                {
                    Competition = newComp02.Entity,
                    CompetitionClass = newCompClass0201.Entity,
                    StartNumber = 3,
                    NamePartA = "Dancer 03-A",
                    OrgIdPartA = "3",
                    OrgPointsPartA = 200,
                    OrgStartsPartA = 2,
                });

            var newCompClass0202 = dbCtx.CompetitionClasses.Add(
                new Tables.CompetitionClass()
                {
                    Competition = newComp02.Entity,
                    CompetitionClassName = "Allg. La C",
                    AdjudicatorPanel = newAdjPanel0201.Entity,
                    OrgClassId = "2",
                    Discipline = "La",
                    AgeClass = "Allg",
                    AgeGroup = null,
                    Class = "C",
                    MinStartsForPromotion = 10,
                    MinPointsForPromotion = 1200,
                    PointsForFirst = 150,
                    ExtraManualStarter = 0,
                    Comment = null,
                });
            // ----
            dbCtx.Participants.Add(
                new Tables.Participant()
                {
                    Competition = newComp02.Entity,
                    CompetitionClass = newCompClass0202.Entity,
                    StartNumber = 4,
                    NamePartA = "Dancer 04-A",
                    OrgIdPartA = "4",
                    OrgPointsPartA = 400,
                    OrgStartsPartA = 4,
                });
            dbCtx.Participants.Add(
                new Tables.Participant()
                {
                    Competition = newComp02.Entity,
                    CompetitionClass = newCompClass0202.Entity,
                    StartNumber = 5,
                    NamePartA = "Dancer 05-A",
                    OrgIdPartA = "5",
                });

            var newCompClass0203 = dbCtx.CompetitionClasses.Add(
                new Tables.CompetitionClass()
                {
                    Competition = newComp02.Entity,
                    CompetitionClassName = "Allg. La B",
                    AdjudicatorPanel = newAdjPanel0201.Entity,
                    OrgClassId = "3",
                    Discipline = "La",
                    AgeClass = "Allg",
                    AgeGroup = null,
                    Class = "B",
                    MinStartsForPromotion = 10,
                    MinPointsForPromotion = 1200,
                    PointsForFirst = 150,
                    ExtraManualStarter = 0,
                    Comment = "what?",
                });
            // ----
            dbCtx.Participants.Add(
                new Tables.Participant()
                {
                    Competition = newComp02.Entity,
                    CompetitionClass = newCompClass0203.Entity,
                    StartNumber = 1,
                    NamePartA = "Dancer 01-A",
                    OrgIdPartA = "1",
                    OrgPointsPartA = 100,
                    OrgStartsPartA = 1,
                });
            dbCtx.Participants.Add(
                new Tables.Participant()
                {
                    Competition = newComp02.Entity,
                    CompetitionClass = newCompClass0203.Entity,
                    StartNumber = 2,
                    NamePartA = "Dancer 02-A",
                    OrgIdPartA = "2",
                    OrgPointsPartA = 200,
                    OrgStartsPartA = 2,
                });
            dbCtx.Participants.Add(
                new Tables.Participant()
                {
                    Competition = newComp02.Entity,
                    CompetitionClass = newCompClass0203.Entity,
                    StartNumber = 3,
                    NamePartA = "Dancer 03-A",
                    OrgIdPartA = "3",
                    OrgPointsPartA = 300,
                    OrgStartsPartA = 3,
                });

            var newCompClass0204 = dbCtx.CompetitionClasses.Add(
                new Tables.CompetitionClass()
                {
                    Competition = newComp02.Entity,
                    CompetitionClassName = "Allg. La A",
                    AdjudicatorPanel = newAdjPanel0201.Entity,
                    OrgClassId = "4",
                    Discipline = "La",
                    AgeClass = "Allg",
                    AgeGroup = null,
                    Class = "A",
                    MinStartsForPromotion = 10,
                    MinPointsForPromotion = 1800,
                    PointsForFirst = 150,
                    ExtraManualStarter = 0,
                    Comment = null,
                });
            // ----
            dbCtx.Participants.Add(
                new Tables.Participant()
                {
                    Competition = newComp02.Entity,
                    CompetitionClass = newCompClass0204.Entity,
                    StartNumber = 5,
                    NamePartA = "Dancer 05-A",
                    OrgIdPartA = "5",
                    OrgPointsPartA = 500,
                    OrgStartsPartA = 5,
                });

            dbCtx.SaveChanges();
        }

        private static async Task AddFathCompetitionAsync(
            DanceCompetitionHelperDbContext dbCtx,
            CancellationToken cancellationToken)
        {
            const string fathCompName = "Big-Fat-Competition";

            var foundFatComp = await dbCtx.Competitions
                .TagWith(
                    nameof(AddFathCompetitionAsync) + "(db)[0]")
                .FirstOrDefaultAsync(
                    x => x.CompetitionName == fathCompName,
                    cancellationToken);

            if (foundFatComp != null)
            {
                return;
            }
        }
    }
}