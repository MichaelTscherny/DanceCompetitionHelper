using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using NUnit.Framework;
using TechTalk.SpecFlow.Assist;

namespace DanceCompetitionHelper.Database.Test.Bindings
{
    [Binding]
    public sealed class Then : BindingBase
    {
        public Then(
            ScenarioContext scenarioContext)
            : base(
                  scenarioContext)
        {
        }

        #region Dance Competition Helper Database

        [Then(@"following Competitions exists in ""([^""]*)""")]
        public void ThenFollowingCompetitionsExistsIn(
            string danceCompHelperDb,
            Table table)
        {
            var checkComps = table.CreateSet<CompetitionPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();

            try
            {
                foreach (var toChk in checkComps)
                {
                    var foundComp = useDb.Competitions.FirstOrDefault(
                        x => x.CompetitionName == toChk.CompetitionName);

                    Assert.That(
                        foundComp,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Competition),
                        toChk);

                    Assert.Multiple(() =>
                    {
                        Assert.That(
                            foundComp.CompetitionName,
                            Is.EqualTo(
                                toChk.CompetitionName),
                            "{0}: {1}",
                            toChk,
                            nameof(foundComp.CompetitionName));
                        Assert.That(
                            foundComp.Organization,
                            Is.EqualTo(
                                toChk.Organization),
                            "{0}: {1}",
                            toChk,
                            nameof(foundComp.Organization));
                        Assert.That(
                            foundComp.OrgCompetitionId,
                            Is.EqualTo(
                                toChk.OrgCompetitionId),
                            "{0}: {1}",
                            toChk,
                            nameof(foundComp.OrgCompetitionId));
                        Assert.That(
                            foundComp.CompetitionInfo,
                            Is.EqualTo(
                                toChk.CompetitionInfo),
                            "{0}: {1}",
                            toChk,
                            nameof(foundComp.CompetitionInfo));
                        Assert.That(
                            foundComp.CompetitionDate,
                            Is.EqualTo(
                                toChk.CompetitionDate ?? UseNow),
                            "{0}: {1}",
                            toChk,
                            nameof(foundComp.CompetitionDate));
                    });
                }
            }
            finally
            {
                dbTrans.Rollback();
            }
        }

        [Then(@"following Competition Classes exists in ""([^""]*)""")]
        public void ThenFollowingCompetitionClassesExistsIn(
            string danceCompHelperDb,
            Table table)
        {
            var checkCompClasses = table.CreateSet<CompetitionClassPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();
            try
            {
                foreach (var curChk in checkCompClasses)
                {
                    var foundComp = useDb.Competitions.FirstOrDefault(
                        x => x.CompetitionName == curChk.CompetitionName);

                    Assert.That(
                        foundComp,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Competition),
                        curChk);

                    var foundCompClass = useDb.CompetitionClasses.FirstOrDefault(
                        x => x.Competition == foundComp
                        && x.OrgClassId == curChk.OrgClassId);

                    Assert.That(
                        foundCompClass,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(CompetitionClass),
                        curChk);

                    Assert.Multiple(() =>
                    {
                        Assert.That(
                            foundCompClass.CompetitionId,
                            Is.EqualTo(
                                foundComp.CompetitionId),
                            "{0}: {1}",
                            curChk,
                            nameof(foundComp.CompetitionId));
                        Assert.That(
                            foundCompClass.OrgClassId,
                            Is.EqualTo(
                                curChk.OrgClassId),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.OrgClassId));
                        Assert.That(
                            foundCompClass.Discipline,
                            Is.EqualTo(
                                curChk.Discipline),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.Discipline));
                        Assert.That(
                            foundCompClass.AgeClass,
                            Is.EqualTo(
                                curChk.AgeClass),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.AgeClass));
                        Assert.That(
                            foundCompClass.AgeGroup,
                            Is.EqualTo(
                                curChk.AgeGroup),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.AgeGroup));
                        Assert.That(
                            foundCompClass.Class,
                            Is.EqualTo(
                                curChk.Class),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.Class));

                        Assert.That(
                            foundCompClass.MinStartsForPromotion,
                            Is.EqualTo(
                                curChk.MinStartsForPromotion),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.MinStartsForPromotion));
                        Assert.That(
                            foundCompClass.MinPointsForPromotion,
                            Is.EqualTo(
                                curChk.MinPointsForPromotion),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.MinPointsForPromotion));
                    });
                }
            }
            finally
            {
                dbTrans.Rollback();
            }
        }

        [Then(@"following Competition Classes Histroy exists in ""([^""]*)""")]
        public void ThenFollowingCompetitionClassesHistoryExistsIn(
            string danceCompHelperDb,
            Table table)
        {
            var checkCompClassesHist = table.CreateSet<CompetitionClassHistoryPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();
            try
            {
                foreach (var curChkHist in checkCompClassesHist)
                {
                    var foundComp = useDb.Competitions.FirstOrDefault(
                        x => x.CompetitionName == curChkHist.CompetitionName);

                    Assert.That(
                        foundComp,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Competition),
                        curChkHist);

                    var foundCompClassHist = useDb.CompetitionClassesHistory.FirstOrDefault(
                        x => x.Competition == foundComp
                        && x.CompetitionClassName == curChkHist.CompetitionClassName);

                    Assert.That(
                        foundCompClassHist,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(CompetitionClass),
                        curChkHist);

                    Assert.Multiple(() =>
                    {
                        Assert.That(
                            foundCompClassHist.CompetitionId,
                            Is.EqualTo(
                                foundComp.CompetitionId),
                            "{0}: {1}",
                            curChkHist,
                            nameof(foundComp.CompetitionId));
                        Assert.That(
                            foundCompClassHist.CompetitionClassId,
                            Is.EqualTo(
                                foundCompClassHist.CompetitionClassId),
                            "{0}: {1}",
                            curChkHist,
                            nameof(foundComp.CompetitionId));
                        Assert.That(
                            foundCompClassHist.Version,
                            Is.EqualTo(
                                curChkHist.Version),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.Version));
                        Assert.That(
                            foundCompClassHist.OrgClassId,
                            Is.EqualTo(
                                curChkHist.OrgClassId),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.OrgClassId));
                        Assert.That(
                            foundCompClassHist.Discipline,
                            Is.EqualTo(
                                curChkHist.Discipline),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.Discipline));
                        Assert.That(
                            foundCompClassHist.AgeClass,
                            Is.EqualTo(
                                curChkHist.AgeClass),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.AgeClass));
                        Assert.That(
                            foundCompClassHist.AgeGroup,
                            Is.EqualTo(
                                curChkHist.AgeGroup),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.AgeGroup));
                        Assert.That(
                            foundCompClassHist.Class,
                            Is.EqualTo(
                                curChkHist.Class),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.Class));

                        Assert.That(
                            foundCompClassHist.MinStartsForPromotion,
                            Is.EqualTo(
                                curChkHist.MinStartsForPromotion),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.MinStartsForPromotion));
                        Assert.That(
                            foundCompClassHist.MinPointsForPromotion,
                            Is.EqualTo(
                                curChkHist.MinPointsForPromotion),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.MinPointsForPromotion));
                    });
                }
            }
            finally
            {
                dbTrans.Rollback();
            }
        }

        [Then(@"following Participants exists in ""([^""]*)""")]
        public void ThenFollowingPartitipantsExistsIn(
            string danceCompHelperDb,
            Table table)
        {
            var checkParticipants = table.CreateSet<ParticipantPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();
            try
            {
                foreach (var curChk in checkParticipants)
                {
                    var foundComp = useDb.Competitions.FirstOrDefault(
                        x => x.CompetitionName == curChk.CompetitionName);

                    Assert.That(
                        foundComp,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Competition),
                        curChk);

                    var foundCompClass = useDb.CompetitionClasses.FirstOrDefault(
                        x => x.Competition == foundComp
                        && x.CompetitionClassName == curChk.CompetitionClassName);

                    Assert.That(
                        foundCompClass,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(CompetitionClass),
                        curChk);

                    var foundParticipant = useDb.Participants.FirstOrDefault(
                        x => x.Competition == foundComp
                        && x.CompetitionClass == foundCompClass
                        && x.NamePartA == curChk.NamePartA
                        && x.NamePartB == curChk.NamePartB);

                    Assert.That(
                        foundParticipant,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Participant),
                        curChk);

                    Assert.Multiple(() =>
                    {
                        Assert.That(
                            foundParticipant.CompetitionId,
                            Is.EqualTo(
                                foundComp.CompetitionId),
                            "{0}: {1}",
                            curChk,
                            nameof(foundComp.CompetitionId));
                        Assert.That(
                            foundParticipant.CompetitionClassId,
                            Is.EqualTo(
                                foundCompClass.CompetitionClassId),
                            "{0}: {1}",
                            curChk,
                            nameof(foundCompClass.CompetitionClassId));

                        Assert.That(
                            foundParticipant.StartNumber,
                            Is.EqualTo(
                                curChk.StartNumber),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.StartNumber));
                        Assert.That(
                            foundParticipant.NamePartA,
                            Is.EqualTo(
                                curChk.NamePartA),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.NamePartA));
                        Assert.That(
                            foundParticipant.OrgIdPartA,
                            Is.EqualTo(
                                curChk.OrgIdPartA),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.OrgIdPartA));
                        Assert.That(
                            foundParticipant.NamePartB,
                            Is.EqualTo(
                                curChk.NamePartB),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.NamePartB));
                        Assert.That(
                            foundParticipant.OrgIdPartB,
                            Is.EqualTo(
                                curChk.OrgIdPartB),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.OrgIdPartB));

                        Assert.That(
                            foundParticipant.OrgIdClub,
                            Is.EqualTo(
                                curChk.OrgIdClub),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.OrgIdClub));

                        Assert.That(
                            foundParticipant.OrgPointsPartA,
                            Is.EqualTo(
                                curChk.OrgPointsPartA),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.OrgPointsPartA));
                        Assert.That(
                            foundParticipant.OrgStartsPartA,
                            Is.EqualTo(
                                curChk.OrgStartsPartA),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.OrgStartsPartA));

                        Assert.That(
                            foundParticipant.OrgPointsPartB,
                            Is.EqualTo(
                                curChk.OrgPointsPartB),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.OrgPointsPartB));
                        Assert.That(
                            foundParticipant.OrgStartsPartB,
                            Is.EqualTo(
                                curChk.OrgStartsPartB),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.OrgStartsPartB));
                    });
                }
            }
            finally
            {
                dbTrans.Rollback();
            }
        }

        [Then(@"following Participants History exists in ""([^""]*)""")]
        public void ThenFollowingPartitipantsHistoryExistsIn(
            string danceCompHelperDb,
            Table table)
        {
            var checkParticipantsHist = table.CreateSet<ParticipantHistoryPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();
            try
            {
                foreach (var curChkHist in checkParticipantsHist)
                {
                    var foundComp = useDb.Competitions.FirstOrDefault(
                        x => x.CompetitionName == curChkHist.CompetitionName);

                    Assert.That(
                        foundComp,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Competition),
                        curChkHist);

                    var foundCompClass = useDb.CompetitionClasses.FirstOrDefault(
                        x => x.Competition == foundComp
                        && x.CompetitionClassName == curChkHist.CompetitionClassName);

                    Assert.That(
                        foundCompClass,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(CompetitionClass),
                        curChkHist);

                    var foundParticipant = useDb.ParticipantsHistory.FirstOrDefault(
                        x => x.Competition == foundComp
                        && x.CompetitionClass == foundCompClass
                        && x.NamePartA == curChkHist.NamePartA
                        && x.NamePartB == curChkHist.NamePartB);

                    Assert.That(
                        foundParticipant,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Participant),
                        curChkHist);

                    Assert.Multiple(() =>
                    {
                        Assert.That(
                            foundParticipant.CompetitionId,
                            Is.EqualTo(
                                foundComp.CompetitionId),
                            "{0}: {1}",
                            curChkHist,
                            nameof(foundComp.CompetitionId));
                        Assert.That(
                            foundParticipant.CompetitionClassId,
                            Is.EqualTo(
                                foundCompClass.CompetitionClassId),
                            "{0}: {1}",
                            curChkHist,
                            nameof(foundCompClass.CompetitionClassId));

                        Assert.That(
                            foundParticipant.Version,
                            Is.EqualTo(
                                curChkHist.Version),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.Version));
                        Assert.That(
                            foundParticipant.StartNumber,
                            Is.EqualTo(
                                curChkHist.StartNumber),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.StartNumber));
                        Assert.That(
                            foundParticipant.NamePartA,
                            Is.EqualTo(
                                curChkHist.NamePartA),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.NamePartA));
                        Assert.That(
                            foundParticipant.OrgIdPartA,
                            Is.EqualTo(
                                curChkHist.OrgIdPartA),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.OrgIdPartA));
                        Assert.That(
                            foundParticipant.NamePartB,
                            Is.EqualTo(
                                curChkHist.NamePartB),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.NamePartB));
                        Assert.That(
                            foundParticipant.OrgIdPartB,
                            Is.EqualTo(
                                curChkHist.OrgIdPartB),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.OrgIdPartB));

                        Assert.That(
                            foundParticipant.OrgIdClub,
                            Is.EqualTo(
                                curChkHist.OrgIdClub),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.OrgIdClub));

                        Assert.That(
                            foundParticipant.OrgPointsPartA,
                            Is.EqualTo(
                                curChkHist.OrgPointsPartA),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.OrgPointsPartA));
                        Assert.That(
                            foundParticipant.OrgStartsPartA,
                            Is.EqualTo(
                                curChkHist.OrgStartsPartA),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.OrgStartsPartA));

                        Assert.That(
                            foundParticipant.OrgPointsPartB,
                            Is.EqualTo(
                                curChkHist.OrgPointsPartB),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.OrgPointsPartB));
                        Assert.That(
                            foundParticipant.OrgStartsPartB,
                            Is.EqualTo(
                                curChkHist.OrgStartsPartB),
                            "{0}: {1}",
                            curChkHist,
                            nameof(curChkHist.OrgStartsPartB));
                    });
                }
            }
            finally
            {
                dbTrans.Rollback();
            }
        }

        #endregion Dance Competition Helper Database

        #region Dance Competition Helper

        [Then(@"following DanceCompetitionHelper ""([^""]*)"" counts exists")]
        public void ThenFollowingDanceCompetitionHelperCountsExists(
            string danceCompHelper,
            Table table)
        {
            var checkCounts = table.CreateSet<DanceCompHelperCountsPoco>();
            var useDanceCompHelper = GetDanceCompetitionHelper(
                danceCompHelper);

            foreach (var curChk in checkCounts)
            {
                Assert.Multiple(() =>
                {
                    var compId = useDanceCompHelper.GetCompetition(
                        curChk.CompetitionName);

                    Assert.That(
                        compId,
                        Is.Not.Null,
                        "Nothign found for '{0}' '{1}' (1)",
                        nameof(curChk.CompetitionName),
                        curChk.CompetitionName);
                    Assert.That(
                        compId.HasValue,
                        Is.True,
                        "Nothign found for '{0}' '{1}' (1)",
                        nameof(curChk.CompetitionName),
                        curChk.CompetitionName);

                    Assert.That(
                        useDanceCompHelper.GetCompetitionClasses(
                            compId)
                            .Count(),
                        Is.EqualTo(curChk.CountClasses),
                        "Count CompClasses");
                    Assert.That(
                        useDanceCompHelper.GetParticipants(
                            compId,
                            null)
                            .Count(),
                        Is.EqualTo(curChk.CountParticipants),
                        "Count Participtans");
                });
            }
        }

        [Then(@"following multiple starts exists in Competition ""([^""]*)"" of DanceCompetitionHelper ""([^""]*)""")]
        public void ThenFollowingMultipleStartsExistsInCompetitionDanceCompetitionHelper(
            string competitionName,
            string danceCompHelper,
            Table table)
        {
            var multiStartParticipants = table.CreateSet<ParticipantPoco>();
            var useDanceCompHelper = GetDanceCompetitionHelper(
                danceCompHelper);

            var useComp = useDanceCompHelper.GetCompetition(
                competitionName);

            Assert.That(
                useComp,
                Is.Not.Null,
                "{0} '{1}' not found!",
                nameof(Competition),
                competitionName);
            Assert.That(
                useComp.HasValue,
                Is.True,
                "{0} '{1}' not found",
                nameof(Competition),
                competitionName);

            var curMultiStarter = useDanceCompHelper.GetMultipleStarter(
                useComp.Value)
                .ToList();

            foreach (var chkMultiStart in multiStartParticipants)
            {
                chkMultiStart.CompetitionName = competitionName;
                var foundStarter = false;

                foreach (var curMuSt in curMultiStarter)
                {
                    var foundPart = curMuSt.Participants.FirstOrDefault(
                        x => x.NamePartA == chkMultiStart.NamePartA
                        && x.OrgIdPartA == chkMultiStart.OrgIdPartA
                        && x.NamePartB == chkMultiStart.NamePartB
                        && x.OrgIdPartB == chkMultiStart.OrgIdPartB
                        && x.ClubName == chkMultiStart.ClubName
                        && x.OrgIdClub == chkMultiStart.OrgIdClub
                        && x.StartNumber == chkMultiStart.StartNumber);

                    if (foundPart == null)
                    {
                        continue;
                    }

                    var foundClass = curMuSt.CompetitionClasses.FirstOrDefault(
                        x => x.CompetitionClassName == chkMultiStart.CompetitionClassName);

                    if (foundClass == null)
                    {
                        continue;
                    }

                    foundStarter = true;
                }

                Assert.That(
                    foundStarter,
                    Is.True,
                    "Multiple Start '{0}' not found!",
                    chkMultiStart);
            }
        }

        [Then(@"none multiple starts exists in Competition ""([^""]*)"" of DanceCompetitionHelper ""([^""]*)""")]
        public void ThenNoneMultipleStartsExistsInCompetitionDanceCompetitionHelper(
            string competitionName,
            string danceCompHelper)
        {
            var useDanceCompHelper = GetDanceCompetitionHelper(
                danceCompHelper);

            var useComp = useDanceCompHelper.GetCompetition(
                competitionName);

            Assert.That(
                useComp,
                Is.Not.Null,
                "{0} '{1}' not found!",
                nameof(Competition),
                competitionName);
            Assert.That(
                useComp.HasValue,
                Is.True,
                "{0} '{1}' not found",
                nameof(Competition),
                competitionName);

            var curMultiStarter = useDanceCompHelper.GetMultipleStarter(
                useComp.Value)
                .ToList();

            Assert.That(
                curMultiStarter.Count,
                Is.EqualTo(0),
                "Found '{0}' multiple starts instead of none!",
                curMultiStarter.Count);
        }

        #endregion Dance Competition Helper

        [AfterScenario]
        public void AfterScenario()
        {
            DisposeAllCreated();
        }
    }
}
