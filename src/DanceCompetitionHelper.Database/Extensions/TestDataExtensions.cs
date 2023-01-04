namespace DanceCompetitionHelper.Database.Extensions
{
    public static class TestDataExtensions
    {
        public static void AddTestData(
            this DanceCompetitionHelperDbContext dbCtx)
        {
            var useTrans = dbCtx.BeginTransaction();

            try
            {
                // we only add data if we do not have any...
                if (dbCtx.Competitions.Count() >= 1)
                {
                    useTrans.Rollback();
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
                    });

                var newCompClass0101 = dbCtx.CompetitionClasses.Add(
                    new Tables.CompetitionClass()
                    {
                        Competition = newComp01.Entity,
                        CompetitionClassName = "Allg. Sta D",
                        OrgClassId = "1",
                        Discipline = "Sta",
                        AgeClass = "Allg",
                        AgeGroup = null,
                        Class = "D",
                        MinStartsForPromotion = 10,
                        MinPointsForPromotion = 900,
                        PointsForWinning = 100,
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
                        OrgClassId = "2",
                        Discipline = "Sta",
                        AgeClass = "Allg",
                        AgeGroup = null,
                        Class = "C",
                        MinStartsForPromotion = 10,
                        MinPointsForPromotion = 1200,
                        PointsForWinning = 100,
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
                    });

                var newCompClass0201 = dbCtx.CompetitionClasses.Add(
                    new Tables.CompetitionClass()
                    {
                        Competition = newComp02.Entity,
                        CompetitionClassName = "Allg. La D",
                        OrgClassId = "1",
                        Discipline = "La",
                        AgeClass = "Allg",
                        AgeGroup = null,
                        Class = "D",
                        MinStartsForPromotion = 10,
                        MinPointsForPromotion = 900,
                        PointsForWinning = 150,
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
                        OrgClassId = "2",
                        Discipline = "La",
                        AgeClass = "Allg",
                        AgeGroup = null,
                        Class = "C",
                        MinStartsForPromotion = 10,
                        MinPointsForPromotion = 1200,
                        PointsForWinning = 150,
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
                        OrgClassId = "3",
                        Discipline = "La",
                        AgeClass = "Allg",
                        AgeGroup = null,
                        Class = "B",
                        MinStartsForPromotion = 10,
                        MinPointsForPromotion = 1200,
                        PointsForWinning = 150,
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
                        OrgClassId = "4",
                        Discipline = "La",
                        AgeClass = "Allg",
                        AgeGroup = null,
                        Class = "A",
                        MinStartsForPromotion = 10,
                        MinPointsForPromotion = 1800,
                        PointsForWinning = 150,
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
                useTrans.Commit();
            }
            catch
            {
                useTrans.Rollback();
            }
        }
    }
}
