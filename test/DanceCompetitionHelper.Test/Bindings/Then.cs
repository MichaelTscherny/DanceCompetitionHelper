using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using DanceCompetitionHelper.Info;
using DanceCompetitionHelper.OrgImpl.Oetsv;
using DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper;

namespace DanceCompetitionHelper.Test.Bindings
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
                    var foundComp = GetCompetition(
                        useDb,
                        toChk.CompetitionName);

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
                    var foundComp = GetCompetition(
                        useDb,
                        curChk.CompetitionName);

                    Assert.That(
                        foundComp,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Competition),
                        curChk);

                    var foundCompClass = GetCompetitionClass(
                        useDb,
                        foundComp.CompetitionId,
                        curChk.CompetitionClassName);

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

        [Then(@"following Adjudicator Panels exists in ""([^""]*)""")]
        public void ThenFollowingAdjudicatorPanelsExistsIn(
            string danceCompHelperDb,
            Table table)
        {
            var checkAdjPanels = table.CreateSet<AdjudicatorPanelPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();
            try
            {
                foreach (var curChk in checkAdjPanels)
                {
                    var foundComp = GetCompetition(
                        useDb,
                        curChk.CompetitionName);

                    Assert.That(
                        foundComp,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Competition),
                        curChk);

                    var foundAdjPanel = GetAdjudicatorPanel(
                        useDb,
                        foundComp.CompetitionId,
                        curChk.Name);

                    Assert.That(
                        foundAdjPanel,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(AdjudicatorPanel),
                        curChk);

                    Assert.Multiple(() =>
                    {
                        Assert.That(
                            foundAdjPanel.CompetitionId,
                            Is.EqualTo(
                                foundComp.CompetitionId),
                            "{0}: {1}",
                            curChk,
                            nameof(foundComp.CompetitionId));
                        Assert.That(
                            foundAdjPanel.Name,
                            Is.EqualTo(
                                curChk.Name),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.Name));
                        Assert.That(
                            foundAdjPanel.Comment,
                            Is.Null.Or.Empty.Or.EqualTo(
                                curChk.Comment),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.Comment));
                    });
                }
            }
            finally
            {
                dbTrans.Rollback();
            }
        }

        [Then(@"following Adjudicators exists in ""([^""]*)""")]
        public void ThenFollowingAdjudicatorsExistsIn(
            string danceCompHelperDb,
            Table table)
        {
            var checkAdjs = table.CreateSet<AdjudicatorPoco>();
            var useDb = GetDanceCompetitionHelperDbContext(
                danceCompHelperDb);

            using var dbTrans = useDb.BeginTransaction();
            try
            {
                foreach (var curChk in checkAdjs)
                {
                    var foundComp = GetCompetition(
                        useDb,
                        curChk.CompetitionName);

                    Assert.That(
                        foundComp,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Competition),
                        curChk);

                    var foundAdjPanel = GetAdjudicatorPanel(
                        useDb,
                        foundComp.CompetitionId,
                        curChk.AdjudicatorPanelName);

                    Assert.That(
                        foundAdjPanel,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(AdjudicatorPanel),
                        curChk);

                    var foundAdj = useDb.Adjudicators.FirstOrDefault(
                        x => x.AdjudicatorPanelId == foundAdjPanel.AdjudicatorPanelId
                        && x.Name == curChk.Name);

                    Assert.That(
                        foundAdj,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Adjudicator),
                        curChk);

                    Assert.Multiple(() =>
                    {
                        Assert.That(
                            foundAdj.AdjudicatorPanelId,
                            Is.EqualTo(
                                foundAdjPanel.AdjudicatorPanelId),
                            "{0}: {1}",
                            curChk,
                            nameof(foundAdjPanel.AdjudicatorPanelId));
                        Assert.That(
                            foundAdj.Abbreviation,
                            Is.EqualTo(
                                curChk.Abbreviation),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.Abbreviation));
                        Assert.That(
                            foundAdj.Name,
                            Is.EqualTo(
                                curChk.Name),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.Name));
                        Assert.That(
                            foundAdj.Comment,
                            Is.Null.Or.Empty.Or.EqualTo(
                                curChk.Comment),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.Comment));
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
                    var foundComp = GetCompetition(
                        useDb,
                        curChkHist.CompetitionName);

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
                            foundCompClassHist.CompetitionClassHistoryId,
                            Is.EqualTo(
                                foundCompClassHist.CompetitionClassHistoryId),
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
                    var foundComp = GetCompetition(
                        useDb,
                        curChk.CompetitionName);

                    Assert.That(
                        foundComp,
                        Is.Not.Null,
                        "{0} '{1}' not found!",
                        nameof(Competition),
                        curChk);

                    var foundCompClass = GetCompetitionClass(
                        useDb,
                        foundComp.CompetitionId,
                        curChk.CompetitionClassName);

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
                            foundParticipant.MinStartsForPromotionPartA,
                            Is.EqualTo(
                                curChk.MinStartsForPromotionPartA),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.MinStartsForPromotionPartA));

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
                        Assert.That(
                            foundParticipant.MinStartsForPromotionPartB,
                            Is.EqualTo(
                                curChk.MinStartsForPromotionPartB),
                            "{0}: {1}",
                            curChk,
                            nameof(curChk.MinStartsForPromotionPartB));
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
                    var foundComp = GetCompetition(
                        useDb,
                        curChkHist.CompetitionName);

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

                    var foundParticipant = useDb.ParticipantsHistory.FirstOrDefault(
                        x => x.Competition == foundComp
                        && x.CompetitionClassHistory == foundCompClassHist
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
                            foundParticipant.CompetitionClassHistoryId,
                            Is.EqualTo(
                                foundCompClassHist.CompetitionClassHistoryId),
                            "{0}: {1}",
                            curChkHist,
                            nameof(foundCompClassHist.CompetitionClassHistoryId));

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

        [Then(@"following Competitions exists in DanceCompetitionHelper ""([^""]*)""")]
        public void ThenFollowingCompetitionExistsInDanceCompetitionHelper(
            string danceCompHelper,
            Table table)
        {
            var checkComps = table.CreateSet<CompetitionPoco>();
            var useDanceCompHelper = GetDanceCompetitionHelper(
                danceCompHelper);

            foreach (var chkCmp in checkComps)
            {
                var foundComp = useDanceCompHelper.GetCompetition(
                    useDanceCompHelper.GetCompetition(
                        chkCmp.CompetitionName));

                Assert.That(
                    foundComp,
                    Is.Not.Null,
                    "{0} '{1}' not found!",
                    nameof(Competition),
                    chkCmp.CompetitionName);

                var compLogString = string.Format(
                    "{0} '{1}'",
                    nameof(Competition),
                    foundComp.CompetitionName);

                Assert.Multiple(() =>
                {
                    if (foundComp == null)
                    {
                        throw new ArgumentNullException();
                    }

                    Assert.That(
                        foundComp.CompetitionName,
                        Is.EqualTo(
                            chkCmp.CompetitionName),
                        "{0}: {1}",
                        compLogString,
                        nameof(Competition.CompetitionName));
                    Assert.That(
                        foundComp.Organization,
                        Is.EqualTo(
                            chkCmp.Organization),
                        "{0}: {1}",
                        compLogString,
                        nameof(Competition.Organization));

                    if (chkCmp.OrgCompetitionId != null)
                    {
                        Assert.That(
                            foundComp.OrgCompetitionId,
                            Is.EqualTo(
                                chkCmp.OrgCompetitionId),
                            "{0}: {1}",
                            compLogString,
                            nameof(Competition.OrgCompetitionId));
                    }

                    if (chkCmp.CompetitionInfo != null)
                    {
                        Assert.That(
                            foundComp.CompetitionInfo,
                            Is.EqualTo(
                                chkCmp.CompetitionInfo),
                            "{0}: {1}",
                            compLogString,
                            nameof(Competition.CompetitionInfo));
                    }

                    if (chkCmp.CompetitionDate.HasValue)
                    {
                        Assert.That(
                            foundComp.CompetitionDate,
                            Is.EqualTo(
                                chkCmp.CompetitionDate),
                            "{0}: {1}",
                            compLogString,
                            nameof(Competition.CompetitionDate));
                    }
                });
            }
        }

        [Then(@"following Classes exists in Competitions of DanceCompetitionHelper ""([^""]*)""")]
        public void ThenFollowingClassesExistsInCompetitionsOfDanceCompetitionHelper(
            string danceCompHelper,
            Table table)
        {
            var checkCompClass = table.CreateSet<CompetitionClassPoco>();
            var useDanceCompHelper = GetDanceCompetitionHelper(
                danceCompHelper);

            var compClassesByCompId = new Dictionary<Guid, List<CompetitionClass>>();

            foreach (var chkCompClass in checkCompClass)
            {
                var useComp = useDanceCompHelper.GetCompetition(
                    chkCompClass.CompetitionName);

                Assert.That(
                    useComp,
                    Is.Not.Null,
                    "{0} '{1}' not found!",
                    nameof(Competition),
                    chkCompClass.CompetitionName);
                Assert.That(
                    useComp.HasValue,
                    Is.True,
                    "{0} '{1}' not found",
                    nameof(Competition),
                    chkCompClass.CompetitionName);

                var useCompId = useComp.Value;
                if (compClassesByCompId.TryGetValue(
                    useCompId,
                    out var foundCompClasses) == false)
                {
                    foundCompClasses = useDanceCompHelper
                        .GetCompetitionClasses(
                            useCompId,
                            true)
                        .ToList();
                    compClassesByCompId[useCompId] = foundCompClasses;
                }

                var foundCompClass = foundCompClasses.FirstOrDefault(
                    x => x.CompetitionClassName == chkCompClass.CompetitionClassName);

                var compClassLogString = string.Format(
                    "{0} '{1}'",
                    nameof(CompetitionClass),
                    chkCompClass.CompetitionClassName);

                Assert.Multiple(
                    () =>
                    {
                        Assert.That(
                            foundCompClass,
                            Is.Not.Null,
                            "{0} not found",
                            compClassLogString);
                        Assert.That(
                            foundCompClass?.DisplayInfo,
                            Is.Not.Null,
                            "{0} not invalid - {1} missing",
                            compClassLogString,
                            nameof(CompetitionClass.DisplayInfo));
                        Assert.That(
                            foundCompClass?.DisplayInfo?.ExtraParticipants,
                            Is.Not.Null,
                            "{0} not invalid - {1} missing",
                            compClassLogString,
                            nameof(CompetitionClass.DisplayInfo.ExtraParticipants));
                    });

                Assert.Multiple(() =>
                {
                    if (foundCompClass == null)
                    {
                        throw new ArgumentNullException();
                    }

                    Assert.That(
                        foundCompClass.OrgClassId,
                        Is.EqualTo(
                            chkCompClass.OrgClassId),
                        "{0}: {1}",
                        compClassLogString,
                        nameof(CompetitionClass.OrgClassId));

                    if (chkCompClass.Discipline != null)
                    {
                        Assert.That(
                            foundCompClass.Discipline,
                            Is.EqualTo(
                                OetsvConstants.Disciplines.ToDisciplines(
                                    chkCompClass.Discipline)),
                            "{0}: {1}",
                            compClassLogString,
                            nameof(CompetitionClass.Discipline));
                    }

                    if (chkCompClass.AgeClass != null)
                    {
                        Assert.That(
                            foundCompClass.AgeClass,
                            Is.EqualTo(
                                OetsvConstants.AgeClasses.ToAgeClasses(
                                    chkCompClass.AgeClass)),
                            "{0}: {1}",
                            compClassLogString,
                            nameof(CompetitionClass.AgeClass));
                    }

                    if (chkCompClass.AgeGroup != null)
                    {
                        Assert.That(
                            foundCompClass.AgeGroup,
                            Is.EqualTo(
                                OetsvConstants.AgeGroups.ToAgeGroup(
                                    chkCompClass.AgeGroup)),
                            "{0}: {1}",
                            compClassLogString,
                            nameof(CompetitionClass.AgeGroup));
                    }

                    if (chkCompClass.Class != null)
                    {
                        Assert.That(
                            foundCompClass.Class,
                            Is.EqualTo(
                                OetsvConstants.Classes.ToClasses(
                                    chkCompClass.Class)),
                            "{0}: {1}",
                            compClassLogString,
                            nameof(CompetitionClass.Class));
                    }

                    if (chkCompClass.MinPointsForPromotion.HasValue)
                    {
                        Assert.That(
                            foundCompClass.MinPointsForPromotion,
                            Is.EqualTo(
                                chkCompClass.MinPointsForPromotion),
                            "{0}: {1}",
                            compClassLogString,
                            nameof(CompetitionClass.MinPointsForPromotion));
                    }

                    if (chkCompClass.MinStartsForPromotion.HasValue)
                    {
                        Assert.That(
                            foundCompClass.MinStartsForPromotion,
                            Is.EqualTo(
                                chkCompClass.MinStartsForPromotion),
                            "{0}: {1}",
                            compClassLogString,
                            nameof(CompetitionClass.MinStartsForPromotion));
                    }

                    if (chkCompClass.PointsForFirst.HasValue)
                    {
                        Assert.That(
                            foundCompClass.PointsForFirst,
                            Is.EqualTo(
                                chkCompClass.PointsForFirst),
                            "{0}: {1}",
                            compClassLogString,
                            nameof(CompetitionClass.PointsForFirst));
                    }
                });

                Assert.Multiple(() =>
                {
                    if (foundCompClass == null
                        || foundCompClass.DisplayInfo == null
                        || foundCompClass.DisplayInfo.ExtraParticipants == null)
                    {
                        throw new ArgumentNullException();
                    }

                    Assert.That(
                        foundCompClass.DisplayInfo.CountParticipants,
                        Is.EqualTo(
                            chkCompClass.CountParticipants),
                        "{0}: {1}",
                        compClassLogString,
                        nameof(CompetitionClassDisplayInfo.CountParticipants));

                    Assert.That(
                        foundCompClass.DisplayInfo.ExtraParticipants.ByWinning,
                        Is.EqualTo(
                            chkCompClass.ExtraPartByWinning),
                        "{0}: {1}",
                        compClassLogString,
                        nameof(ExtraParticipants.ByWinning));
                    Assert.That(
                        foundCompClass.DisplayInfo.ExtraParticipants.ByWinningInfo,
                        Is.Null.Or.Empty.Or.EqualTo(
                            chkCompClass.ExtraPartByWinningInfo),
                        "{0}: {1}",
                        compClassLogString,
                        nameof(ExtraParticipants.ByWinningInfo));
                    Assert.That(
                        foundCompClass.DisplayInfo.ExtraParticipants.ByPromotion,
                        Is.EqualTo(
                            chkCompClass.ExtraPartByPromotion),
                        "{0}: {1}",
                        compClassLogString,
                        nameof(ExtraParticipants.ByPromotion));
                    Assert.That(
                        foundCompClass.DisplayInfo.ExtraParticipants.ByPromotionInfo,
                        Is.Null.Or.Empty.Or.EqualTo(
                            chkCompClass.ExtraPartByPromotionInfo),
                        "{0}: {1}",
                        compClassLogString,
                        nameof(ExtraParticipants.ByPromotionInfo));

                });
            }
        }

        [Then(@"following Counts exists in Competitions of DanceCompetitionHelper ""([^""]*)""")]
        public void ThenFollowingCountsExistsInDanceCompetitionsOfCompetitionHelper(
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

        [Then(@"following multiple starts exists in Competitions of DanceCompetitionHelper ""([^""]*)""")]
        public void ThenFollowingMultipleStartsExistsInCompetitionsDanceCompetitionHelper(
            string danceCompHelper,
            Table table)
        {
            var multiStartParticipants = table.CreateSet<ParticipantPoco>();
            var useDanceCompHelper = GetDanceCompetitionHelper(
                danceCompHelper);

            var cachedMultipleStarters = new Dictionary<Guid, List<MultipleStarter>>();

            foreach (var chkMultiStart in multiStartParticipants)
            {
                var useComp = useDanceCompHelper.GetCompetition(
                    chkMultiStart.CompetitionName);

                Assert.That(
                    useComp,
                    Is.Not.Null,
                    "{0} '{1}' not found!",
                    nameof(Competition),
                    chkMultiStart.CompetitionName);
                Assert.That(
                    useComp.HasValue,
                    Is.True,
                    "{0} '{1}' not found",
                    nameof(Competition),
                    chkMultiStart.CompetitionName);

                var useCompId = useComp.Value;
                if (cachedMultipleStarters.TryGetValue(
                    useCompId,
                    out var curMultiStarter) == false)
                {
                    curMultiStarter = useDanceCompHelper
                        .GetMultipleStarter(
                            useCompId)
                        .ToList();

                    cachedMultipleStarters[useCompId] = curMultiStarter;
                }

                chkMultiStart.CompetitionName = chkMultiStart.CompetitionName;
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

        [Then(@"none multiple starts exists in Competitions of DanceCompetitionHelper ""([^""]*)""")]
        public void ThenNoneMultipleStartsExistsInCompetitionDanceCompetitionHelper(
            string danceCompHelper,
            Table table)
        {
            var multiStartParticipants = table.CreateSet<ParticipantPoco>();
            var useDanceCompHelper = GetDanceCompetitionHelper(
                    danceCompHelper);

            foreach (var chkMultipleStart in multiStartParticipants)
            {
                var useComp = useDanceCompHelper.GetCompetition(
                    chkMultipleStart.CompetitionName);

                Assert.That(
                    useComp,
                    Is.Not.Null,
                    "{0} '{1}' not found!",
                    nameof(Competition),
                    chkMultipleStart.CompetitionName);
                Assert.That(
                    useComp.HasValue,
                    Is.True,
                    "{0} '{1}' not found",
                    nameof(Competition),
                    chkMultipleStart.CompetitionName);

                var useCompId = useComp.Value;
                var curMultiStarter = useDanceCompHelper
                    .GetMultipleStarter(
                        useCompId)
                    .ToList();

                Assert.That(
                    curMultiStarter.Count,
                    Is.EqualTo(0),
                    "Found '{0}' multiple starts in '{1}' instead of none!",
                    curMultiStarter.Count,
                    chkMultipleStart.CompetitionName);
            }
        }

        [Then(@"following Participants exists in Competitions of DanceCompetitionHelper ""([^""]*)""")]
        public void ThenFollowingParticipantsExistsInCompetitionOfDanceCompetitionHelper(
            string danceCompHelper,
            Table table)
        {
            var checkParticipants = table.CreateSet<ParticipantPoco>();
            var useDanceCompHelper = GetDanceCompetitionHelper(
                danceCompHelper);

            var participtansByCompId = new Dictionary<Guid, List<Participant>>();

            foreach (var chkPart in checkParticipants)
            {
                var useComp = useDanceCompHelper.GetCompetition(
                    chkPart.CompetitionName);

                Assert.That(
                    useComp,
                    Is.Not.Null,
                    "{0} '{1}' not found!",
                    nameof(Competition),
                    chkPart.CompetitionName);
                Assert.That(
                    useComp.HasValue,
                    Is.True,
                    "{0} '{1}' not found",
                    nameof(Competition),
                    chkPart.CompetitionName);

                var useCompId = useComp.Value;
                if (participtansByCompId.TryGetValue(
                    useCompId,
                    out var foundParticipants) == false)
                {
                    foundParticipants = useDanceCompHelper
                        .GetParticipants(
                            useCompId,
                            null,
                            true)
                        .ToList();

                    participtansByCompId[useCompId] = foundParticipants;
                }

                var foundPart = foundParticipants.FirstOrDefault(
                    x => x.NamePartA == chkPart.NamePartA
                    && x.NamePartB == chkPart.NamePartB
                    && x.StartNumber == chkPart.StartNumber
                    && x.CompetitionClass.CompetitionClassName == chkPart.CompetitionClassName);

                var partLogString = string.Format(
                    "{0} '{1}'/'{2}' #{3} ({4})",
                    nameof(Participant),
                    chkPart.NamePartA,
                    chkPart.NamePartB,
                    chkPart.StartNumber,
                    chkPart.CompetitionClassName);

                Assert.Multiple(() =>
                {
                    Assert.That(
                        foundPart,
                        Is.Not.Null,
                        "{0} not found",
                        partLogString);
                    Assert.That(
                        foundPart?.DisplayInfo,
                        Is.Not.Null,
                        "{0} not invalid - {1} missing",
                        partLogString,
                        nameof(Participant.DisplayInfo));
                    Assert.That(
                        foundPart?.DisplayInfo?.PromotionInfo,
                        Is.Not.Null,
                        "{0} not invalid - {1}.{2} missing",
                        partLogString,
                        nameof(Participant.DisplayInfo),
                        nameof(ParticipantDisplayInfo.PromotionInfo));
                    Assert.That(
                        foundPart?.DisplayInfo?.MultipleStartInfo,
                        Is.Not.Null,
                        "{0} not invalid - {1}.{2} missing",
                        partLogString,
                        nameof(Participant.DisplayInfo),
                        nameof(ParticipantDisplayInfo.MultipleStartInfo));
                });

                Assert.Multiple(() =>
                {
                    if (foundPart == null
                        || foundPart.DisplayInfo == null
                        || foundPart.DisplayInfo.PromotionInfo == null
                        || foundPart.DisplayInfo.MultipleStartInfo == null)
                    {
                        throw new ArgumentNullException();
                    }

                    Assert.That(
                        foundPart.DisplayInfo.MultipleStartInfo.MultipleStarts,
                        Is.EqualTo(
                            chkPart.MultipleStarts),
                        "{0}: {1}",
                        partLogString,
                        nameof(CheckMultipleStartInfo.MultipleStarts));

                    Assert.That(
                        foundPart.DisplayInfo.PromotionInfo.PossiblePromotionA,
                        Is.EqualTo(
                            chkPart.PossiblePromotionA),
                        "{0}: {1}",
                        partLogString,
                        nameof(CheckPromotionInfo.PossiblePromotionA));
                    Assert.That(
                        foundPart.DisplayInfo.PromotionInfo.PossiblePromotionAInfo,
                        Is.Null.Or.Empty.Or.EqualTo(
                            chkPart.PossiblePromotionAInfo),
                        "{0}: {1}",
                        partLogString,
                        nameof(CheckPromotionInfo.PossiblePromotionAInfo));

                    Assert.That(
                        foundPart.DisplayInfo.PromotionInfo.PossiblePromotionB,
                        Is.EqualTo(
                            chkPart.PossiblePromotionB),
                        "{0}: {1}",
                        partLogString,
                        nameof(CheckPromotionInfo.PossiblePromotionB));
                    Assert.That(
                        foundPart.DisplayInfo.PromotionInfo.PossiblePromotionBInfo,
                        Is.Null.Or.Empty.Or.EqualTo(
                            chkPart.PossiblePromotionBInfo),
                        "{0}: {1}",
                        partLogString,
                        nameof(CheckPromotionInfo.PossiblePromotionBInfo));
                });
            }
        }



        #endregion Dance Competition Helper

        [AfterScenario]
        public void AfterScenario()
        {
            DisposeAllCreated();
        }
    }
}
