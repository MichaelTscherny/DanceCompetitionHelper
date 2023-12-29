﻿using DanceCompetitionHelper.Database.DisplayInfo;
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

            using var dbTrans = useDb.BeginTransaction()
                ?? throw new ArgumentNullException(
                    "dbTrans");

            try
            {
                Assert.Multiple(() =>
                {
                    foreach (var toChk in checkComps)
                    {
                        var foundComp = GetCompetition(
                            useDb,
                            toChk.CompetitionName);

                        Assert.That(
                            foundComp,
                            Is.Not.Null,
                            $"{nameof(Competition)} '{toChk}' not found!");

                        if (foundComp == null)
                        {
                            continue;
                        }

                        Assert.Multiple(() =>
                        {
                            Assert.That(
                                foundComp.CompetitionName,
                                Is.EqualTo(
                                    toChk.CompetitionName),
                                $"{toChk}: {nameof(foundComp.CompetitionName)}");
                            Assert.That(
                                foundComp.Organization,
                                Is.EqualTo(
                                    toChk.Organization),
                                $"{toChk}: {nameof(foundComp.Organization)}");
                            Assert.That(
                                foundComp.OrgCompetitionId,
                                Is.EqualTo(
                                    toChk.OrgCompetitionId),
                                $"{toChk}: {nameof(foundComp.OrgCompetitionId)}");
                            Assert.That(
                                foundComp.CompetitionInfo,
                                Is.EqualTo(
                                    toChk.CompetitionInfo),
                                $"{toChk}: {nameof(foundComp.CompetitionInfo)}");
                            Assert.That(
                                foundComp.CompetitionDate,
                                Is.EqualTo(
                                    toChk.CompetitionDate ?? UseNow),
                                $"{toChk}: {nameof(foundComp.CompetitionDate)}");
                        });
                    }
                });
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

            using var dbTrans = useDb.BeginTransaction()
                ?? throw new ArgumentNullException(
                    "dbTrans");
            try
            {
                Assert.Multiple(() =>
                {
                    foreach (var curChk in checkCompClasses)
                    {
                        var foundComp = GetCompetition(
                            useDb,
                            curChk.CompetitionName);

                        Assert.That(
                            foundComp,
                            Is.Not.Null,
                            $"{nameof(Competition)} '{curChk}' not found!");

                        if (foundComp == null)
                        {
                            continue;
                        }

                        var foundCompClass = GetCompetitionClass(
                            useDb,
                            foundComp.CompetitionId,
                            curChk.CompetitionClassName);

                        Assert.That(
                            foundCompClass,
                            Is.Not.Null,
                            $"{nameof(CompetitionClass)} '{curChk}' not found!");

                        if (foundCompClass == null)
                        {
                            continue;
                        }

                        var checkFollowUpCompClass = GetCompetitionClass(
                            useDb,
                            foundComp.CompetitionId,
                            curChk.FollowUpCompetitionClassName);

                        if (string.IsNullOrEmpty(
                            curChk.FollowUpCompetitionClassName) == false)
                        {
                            Assert.That(
                                checkFollowUpCompClass,
                                Is.Not.Null,
                                $"Follow Up {nameof(CompetitionClass)} '{curChk.FollowUpCompetitionClassName}' not found!");
                        }

                        var checkAdjudicatorPanel = GetAdjudicatorPanel(
                            useDb,
                            foundComp.CompetitionId,
                            curChk.AdjudicatorPanelName);

                        Assert.That(
                            checkAdjudicatorPanel,
                            Is.Not.Null,
                            $"{nameof(AdjudicatorPanel)} '{curChk}' not found!");

                        if (checkAdjudicatorPanel == null)
                        {
                            continue;
                        }

                        Assert.Multiple(() =>
                        {
                            Assert.That(
                                foundCompClass.CompetitionId,
                                Is.EqualTo(
                                    foundComp.CompetitionId),
                                $"{curChk}: {nameof(foundComp.CompetitionId)}");
                            Assert.That(
                                foundCompClass.OrgClassId,
                                Is.EqualTo(
                                    curChk.OrgClassId),
                                $"{curChk}: {nameof(curChk.OrgClassId)}");

                            if (checkFollowUpCompClass != null)
                            {
                                Assert.That(
                                    foundCompClass.FollowUpCompetitionClass?.CompetitionClassName,
                                    Is.EqualTo(
                                        checkFollowUpCompClass.CompetitionClassName),
                                    $"{curChk}: {nameof(curChk.FollowUpCompetitionClassName)}");
                            }

                            Assert.That(
                                foundCompClass.AdjudicatorPanelId,
                                Is.EqualTo(
                                    checkAdjudicatorPanel.AdjudicatorPanelId),
                                $"{curChk}: {nameof(curChk.AdjudicatorPanelName)}");

                            Assert.That(
                                foundCompClass.Discipline,
                                Is.EqualTo(
                                    curChk.Discipline),
                                $"{curChk}: {nameof(curChk.Discipline)}");
                            Assert.That(
                                foundCompClass.AgeClass,
                                Is.EqualTo(
                                    curChk.AgeClass),
                                $"{curChk}: {nameof(curChk.AgeClass)}");
                            Assert.That(
                                foundCompClass.AgeGroup,
                                Is.EqualTo(
                                    curChk.AgeGroup),
                                $"{curChk}: {nameof(curChk.AgeGroup)}");
                            Assert.That(
                                foundCompClass.Class,
                                Is.EqualTo(
                                    curChk.Class),
                                $"{curChk}: {nameof(curChk.Class)}");

                            Assert.That(
                                foundCompClass.MinStartsForPromotion,
                                Is.EqualTo(
                                    curChk.MinStartsForPromotion),
                                $"{curChk}: {nameof(curChk.MinStartsForPromotion)}");
                            Assert.That(
                                foundCompClass.MinPointsForPromotion,
                                Is.EqualTo(
                                    curChk.MinPointsForPromotion),
                                $"{curChk}: {nameof(curChk.MinPointsForPromotion)}");
                        });
                    }
                });
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

            using var dbTrans = useDb.BeginTransaction()
                ?? throw new ArgumentNullException(
                    "dbTrans");

            try
            {
                Assert.Multiple(() =>
                {
                    foreach (var curChk in checkAdjPanels)
                    {
                        var foundComp = GetCompetition(
                            useDb,
                            curChk.CompetitionName);

                        Assert.That(
                            foundComp,
                            Is.Not.Null,
                            $"{nameof(Competition)} '{curChk}' not found!");

                        if (foundComp == null)
                        {
                            continue;
                        }

                        var foundAdjPanel = GetAdjudicatorPanel(
                            useDb,
                            foundComp.CompetitionId,
                            curChk.Name);

                        Assert.That(
                            foundAdjPanel,
                            Is.Not.Null,
                            $"{nameof(AdjudicatorPanel)} '{curChk}' not found!");

                        if (foundAdjPanel == null)
                        {
                            continue;
                        }

                        Assert.Multiple(() =>
                        {
                            Assert.That(
                                foundAdjPanel.CompetitionId,
                                Is.EqualTo(
                                    foundComp.CompetitionId),
                                $"{curChk}: {nameof(foundComp.CompetitionId)}");
                            Assert.That(
                                foundAdjPanel.Name,
                                Is.EqualTo(
                                    curChk.Name),
                                $"{curChk}: {nameof(foundAdjPanel.Name)}");
                            Assert.That(
                                foundAdjPanel.Comment,
                                Is.Null.Or.Empty.Or.EqualTo(
                                    curChk.Comment),
                                $"{curChk}: {nameof(foundComp.Comment)}");
                        });
                    }
                });
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

            using var dbTrans = useDb.BeginTransaction()
                ?? throw new ArgumentNullException(
                    "dbTrans");

            try
            {
                Assert.Multiple(() =>
                {
                    foreach (var curChk in checkAdjs)
                    {
                        var foundComp = GetCompetition(
                            useDb,
                            curChk.CompetitionName);

                        Assert.That(
                            foundComp,
                            Is.Not.Null,
                            $"{nameof(Competition)} '{curChk}' not found!");

                        if (foundComp == null)
                        {
                            continue;
                        }

                        var foundAdjPanel = GetAdjudicatorPanel(
                            useDb,
                            foundComp.CompetitionId,
                            curChk.AdjudicatorPanelName);

                        Assert.That(
                            foundAdjPanel,
                            Is.Not.Null,
                            $"{nameof(AdjudicatorPanel)} '{curChk}' not found!");

                        if (foundAdjPanel == null)
                        {
                            continue;
                        }

                        var foundAdj = useDb.Adjudicators.FirstOrDefault(
                            x => x.AdjudicatorPanelId == foundAdjPanel.AdjudicatorPanelId
                            && x.Name == curChk.Name);

                        Assert.That(
                            foundAdj,
                            Is.Not.Null,
                            $"{nameof(Adjudicator)} '{curChk}' not found!");

                        if (foundAdj == null)
                        {
                            continue;
                        }

                        Assert.Multiple(() =>
                        {
                            Assert.That(
                                foundAdj.AdjudicatorPanelId,
                                Is.EqualTo(
                                    foundAdjPanel.AdjudicatorPanelId),
                                $"{curChk}: {nameof(foundAdj.AdjudicatorPanelId)}");
                            Assert.That(
                                foundAdj.Abbreviation,
                                Is.EqualTo(
                                    curChk.Abbreviation),
                                $"{curChk}: {nameof(curChk.Abbreviation)}");
                            Assert.That(
                                foundAdj.Name,
                                Is.EqualTo(
                                    curChk.Name),
                                $"{curChk}: {nameof(curChk.Name)}");
                            Assert.That(
                                foundAdj.Comment,
                                Is.Null.Or.Empty.Or.EqualTo(
                                    curChk.Comment),
                                $"{curChk}: {nameof(curChk.Comment)}");
                        });
                    }
                });
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

            using var dbTrans = useDb.BeginTransaction()
                ?? throw new ArgumentNullException(
                    "dbTrans");

            try
            {
                Assert.Multiple(() =>
                {
                    foreach (var curChkHist in checkCompClassesHist)
                    {
                        var foundComp = GetCompetition(
                            useDb,
                            curChkHist.CompetitionName);

                        Assert.That(
                            foundComp,
                            Is.Not.Null,
                            $"{nameof(Competition)} '{curChkHist}' not found!");

                        if (foundComp == null)
                        {
                            continue;
                        }

                        var foundCompClassHist = useDb.CompetitionClassesHistory.FirstOrDefault(
                            x => x.Competition == foundComp
                            && x.CompetitionClassName == curChkHist.CompetitionClassName);

                        Assert.That(
                            foundCompClassHist,
                            Is.Not.Null,
                            $"{nameof(CompetitionClass)} '{curChkHist}' not found!");

                        if (foundCompClassHist == null)
                        {
                            continue;
                        }

                        Assert.Multiple(() =>
                        {
                            Assert.That(
                                foundCompClassHist.CompetitionId,
                                Is.EqualTo(
                                    foundComp.CompetitionId),
                                $"{curChkHist}: {nameof(foundComp.CompetitionId)}");
                            Assert.That(
                                foundCompClassHist.CompetitionClassHistoryId,
                                Is.EqualTo(
                                    foundCompClassHist.CompetitionClassHistoryId),
                                $"{curChkHist}: {nameof(foundComp.CompetitionId)}");
                            Assert.That(
                                foundCompClassHist.Version,
                                Is.EqualTo(
                                    curChkHist.Version),
                                $"{curChkHist}: {nameof(curChkHist.Version)}");
                            Assert.That(
                                foundCompClassHist.OrgClassId,
                                Is.EqualTo(
                                    curChkHist.OrgClassId),
                                $"{curChkHist}: {nameof(curChkHist.OrgClassId)}");
                            Assert.That(
                                foundCompClassHist.Discipline,
                                Is.EqualTo(
                                    curChkHist.Discipline),
                                $"{curChkHist}: {nameof(curChkHist.Discipline)}");
                            Assert.That(
                                foundCompClassHist.AgeClass,
                                Is.EqualTo(
                                    curChkHist.AgeClass),
                                $"{curChkHist}: {nameof(curChkHist.AgeClass)}");
                            Assert.That(
                                foundCompClassHist.AgeGroup,
                                Is.EqualTo(
                                    curChkHist.AgeGroup),
                                $"{curChkHist}: {nameof(curChkHist.AgeGroup)}");
                            Assert.That(
                                foundCompClassHist.Class,
                                Is.EqualTo(
                                    curChkHist.Class),
                                $"{curChkHist}: {nameof(curChkHist.Class)}");

                            Assert.That(
                                foundCompClassHist.MinStartsForPromotion,
                                Is.EqualTo(
                                    curChkHist.MinStartsForPromotion),
                                $"{curChkHist}: {nameof(curChkHist.MinStartsForPromotion)}");
                            Assert.That(
                                foundCompClassHist.MinPointsForPromotion,
                                Is.EqualTo(
                                    curChkHist.MinPointsForPromotion),
                                $"{curChkHist}: {nameof(curChkHist.MinPointsForPromotion)}");
                        });
                    }
                });
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

            using var dbTrans = useDb.BeginTransaction()
                ?? throw new ArgumentNullException(
                    "dbTrans");

            try
            {
                Assert.Multiple(() =>
                {
                    foreach (var curChk in checkParticipants)
                    {
                        var foundComp = GetCompetition(
                            useDb,
                            curChk.CompetitionName);

                        Assert.That(
                            foundComp,
                            Is.Not.Null,
                            $"{nameof(Competition)} '{curChk}' not found!");

                        if (foundComp == null)
                        {
                            continue;
                        }

                        var foundCompClass = GetCompetitionClass(
                            useDb,
                            foundComp.CompetitionId,
                            curChk.CompetitionClassName);

                        Assert.That(
                            foundCompClass,
                            Is.Not.Null,
                            $"{nameof(CompetitionClass)} '{curChk}' not found!");

                        if (foundCompClass == null)
                        {
                            continue;
                        }

                        var foundParticipant = useDb.Participants.FirstOrDefault(
                            x => x.Competition == foundComp
                            && x.CompetitionClass == foundCompClass
                            && x.NamePartA == curChk.NamePartA
                            && x.NamePartB == curChk.NamePartB);

                        Assert.That(
                            foundParticipant,
                            Is.Not.Null,
                            $"{nameof(Participant)} '{curChk}' not found!");

                        if (foundParticipant == null)
                        {
                            continue;
                        }

                        Assert.Multiple(() =>
                        {
                            Assert.That(
                                foundParticipant.CompetitionId,
                                Is.EqualTo(
                                    foundComp.CompetitionId),
                                $"{curChk}: {nameof(foundComp.CompetitionId)}");
                            Assert.That(
                                foundParticipant.CompetitionClassId,
                                Is.EqualTo(
                                    foundCompClass.CompetitionClassId),
                                $"{curChk}: {nameof(foundCompClass.CompetitionClassId)}");

                            Assert.That(
                                foundParticipant.StartNumber,
                                Is.EqualTo(
                                    curChk.StartNumber),
                                $"{curChk}: {nameof(curChk.StartNumber)}");
                            Assert.That(
                                foundParticipant.NamePartA,
                                Is.EqualTo(
                                    curChk.NamePartA),
                                $"{curChk}: {nameof(curChk.NamePartA)}");
                            Assert.That(
                                foundParticipant.OrgIdPartA,
                                Is.EqualTo(
                                    curChk.OrgIdPartA),
                                $"{curChk}: {nameof(curChk.OrgIdPartA)}");
                            Assert.That(
                                foundParticipant.NamePartB,
                                Is.EqualTo(
                                    curChk.NamePartB),
                                $"{curChk}: {nameof(curChk.NamePartB)}");
                            Assert.That(
                                foundParticipant.OrgIdPartB,
                                Is.EqualTo(
                                    curChk.OrgIdPartB),
                                $"{curChk}: {nameof(curChk.OrgIdPartB)}");

                            Assert.That(
                                foundParticipant.OrgIdClub,
                                Is.EqualTo(
                                    curChk.OrgIdClub),
                                $"{curChk}: {nameof(curChk.OrgIdClub)}");

                            Assert.That(
                                foundParticipant.OrgPointsPartA,
                                Is.EqualTo(
                                    curChk.OrgPointsPartA),
                                $"{curChk}: {nameof(curChk.OrgPointsPartA)}");
                            Assert.That(
                                foundParticipant.OrgStartsPartA,
                                Is.EqualTo(
                                    curChk.OrgStartsPartA),
                                $"{curChk}: {nameof(curChk.OrgStartsPartA)}");
                            Assert.That(
                                foundParticipant.MinStartsForPromotionPartA,
                                Is.EqualTo(
                                    curChk.MinStartsForPromotionPartA),
                                $"{curChk}: {nameof(curChk.MinStartsForPromotionPartA)}");

                            Assert.That(
                                foundParticipant.OrgPointsPartB,
                                Is.EqualTo(
                                    curChk.OrgPointsPartB),
                                $"{curChk}: {nameof(curChk.OrgPointsPartB)}");
                            Assert.That(
                                foundParticipant.OrgStartsPartB,
                                Is.EqualTo(
                                    curChk.OrgStartsPartB),
                                $"{curChk}: {nameof(curChk.OrgStartsPartB)}");
                            Assert.That(
                                foundParticipant.MinStartsForPromotionPartB,
                                Is.EqualTo(
                                    curChk.MinStartsForPromotionPartB),
                                $"{curChk}: {nameof(curChk.MinStartsForPromotionPartB)}");
                        });
                    }
                });
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

            using var dbTrans = useDb.BeginTransaction()
                ?? throw new ArgumentNullException(
                    "dbTrans");

            try
            {
                Assert.Multiple(() =>
                {
                    foreach (var curChkHist in checkParticipantsHist)
                    {
                        var foundComp = GetCompetition(
                            useDb,
                            curChkHist.CompetitionName);

                        Assert.That(
                            foundComp,
                            Is.Not.Null,
                            $"{nameof(Competition)} '{curChkHist}' not found!");

                        if (foundComp == null)
                        {
                            continue;
                        }

                        var foundCompClassHist = useDb.CompetitionClassesHistory.FirstOrDefault(
                            x => x.Competition == foundComp
                            && x.CompetitionClassName == curChkHist.CompetitionClassName);

                        Assert.That(
                            foundCompClassHist,
                            Is.Not.Null,
                            $"{nameof(CompetitionClass)} '{curChkHist}' not found!");

                        if (foundCompClassHist == null)
                        {
                            continue;
                        }

                        var foundParticipant = useDb.ParticipantsHistory.FirstOrDefault(
                            x => x.Competition == foundComp
                            && x.CompetitionClassHistory == foundCompClassHist
                            && x.NamePartA == curChkHist.NamePartA
                            && x.NamePartB == curChkHist.NamePartB);

                        Assert.That(
                            foundParticipant,
                            Is.Not.Null,
                            $"{nameof(Participant)} '{curChkHist}' not found!");

                        if (foundParticipant == null)
                        {
                            continue;
                        }

                        Assert.Multiple(() =>
                        {
                            Assert.That(
                                foundParticipant.CompetitionId,
                                Is.EqualTo(
                                    foundComp.CompetitionId),
                                $"{curChkHist}: {nameof(foundComp.CompetitionId)}");
                            Assert.That(
                                foundParticipant.CompetitionClassHistoryId,
                                Is.EqualTo(
                                    foundCompClassHist.CompetitionClassHistoryId),
                                $"{curChkHist}: {nameof(foundCompClassHist.CompetitionClassHistoryId)}");

                            Assert.That(
                                foundParticipant.Version,
                                Is.EqualTo(
                                    curChkHist.Version),
                                $"{curChkHist}: {nameof(curChkHist.Version)}");
                            Assert.That(
                                foundParticipant.StartNumber,
                                Is.EqualTo(
                                    curChkHist.StartNumber),
                                $"{curChkHist}: {nameof(curChkHist.StartNumber)}");
                            Assert.That(
                                foundParticipant.NamePartA,
                                Is.EqualTo(
                                    curChkHist.NamePartA),
                                $"{curChkHist}: {nameof(curChkHist.NamePartA)}");
                            Assert.That(
                                foundParticipant.OrgIdPartA,
                                Is.EqualTo(
                                    curChkHist.OrgIdPartA),
                                $"{curChkHist}: {nameof(curChkHist.OrgIdPartA)}");
                            Assert.That(
                                foundParticipant.NamePartB,
                                Is.EqualTo(
                                    curChkHist.NamePartB),
                                $"{curChkHist}: {nameof(curChkHist.NamePartB)}");
                            Assert.That(
                                foundParticipant.OrgIdPartB,
                                Is.EqualTo(
                                    curChkHist.OrgIdPartB),
                                $"{curChkHist}: {nameof(curChkHist.OrgIdPartB)}");

                            Assert.That(
                                foundParticipant.OrgIdClub,
                                Is.EqualTo(
                                    curChkHist.OrgIdClub),
                                $"{curChkHist}: {nameof(curChkHist.OrgIdClub)}");

                            Assert.That(
                                foundParticipant.OrgPointsPartA,
                                Is.EqualTo(
                                    curChkHist.OrgPointsPartA),
                                $"{curChkHist}: {nameof(curChkHist.OrgPointsPartA)}");
                            Assert.That(
                                foundParticipant.OrgStartsPartA,
                                Is.EqualTo(
                                    curChkHist.OrgStartsPartA),
                                $"{curChkHist}: {nameof(curChkHist.OrgStartsPartA)}");

                            Assert.That(
                                foundParticipant.OrgPointsPartB,
                                Is.EqualTo(
                                    curChkHist.OrgPointsPartB),
                                $"{curChkHist}: {nameof(curChkHist.OrgPointsPartB)}");
                            Assert.That(
                                foundParticipant.OrgStartsPartB,
                                Is.EqualTo(
                                    curChkHist.OrgStartsPartB),
                                $"{curChkHist}: {nameof(curChkHist.OrgStartsPartB)}");
                        });
                    }
                });
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

            Assert.Multiple(() =>
            {
                foreach (var chkCmp in checkComps)
                {
                    var foundComp = useDanceCompHelper.GetCompetition(
                        useDanceCompHelper.GetCompetition(
                            chkCmp.CompetitionName));

                    Assert.That(
                        foundComp,
                        Is.Not.Null,
                        $"{nameof(Competition)} '{chkCmp.CompetitionName}' not found!");

                    if (foundComp == null)
                    {
                        continue;
                    }

                    var compLogString = string.Format(
                        "{0} '{1}'",
                        nameof(Competition),
                        foundComp.CompetitionName);

                    Assert.Multiple(() =>
                    {
                        if (foundComp == null)
                        {
                            throw new ArgumentNullException(
                                nameof(foundComp));
                        }

                        Assert.That(
                            foundComp.CompetitionName,
                            Is.EqualTo(
                                chkCmp.CompetitionName),
                            $"{compLogString}: {nameof(Competition.CompetitionName)}");
                        Assert.That(
                            foundComp.Organization,
                            Is.EqualTo(
                                chkCmp.Organization),
                            $"{compLogString}: {nameof(Competition.Organization)}");

                        if (chkCmp.OrgCompetitionId != null)
                        {
                            Assert.That(
                                foundComp.OrgCompetitionId,
                                Is.EqualTo(
                                    chkCmp.OrgCompetitionId),
                                $"{compLogString}: {nameof(Competition.OrgCompetitionId)}");
                        }

                        if (chkCmp.CompetitionInfo != null)
                        {
                            Assert.That(
                                foundComp.CompetitionInfo,
                                Is.EqualTo(
                                    chkCmp.CompetitionInfo),
                                $"{compLogString}: {nameof(Competition.CompetitionInfo)}");
                        }

                        if (chkCmp.CompetitionDate.HasValue)
                        {
                            Assert.That(
                                foundComp.CompetitionDate,
                                Is.EqualTo(
                                    chkCmp.CompetitionDate),
                                $"{compLogString}: {nameof(Competition.CompetitionDate)}");
                        }
                    });
                }
            });
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

            Assert.Multiple(() =>
            {
                foreach (var chkCompClass in checkCompClass)
                {
                    var useComp = useDanceCompHelper.GetCompetition(
                        chkCompClass.CompetitionName);

                    Assert.That(
                        useComp,
                        Is.Not.Null,
                        $"{nameof(Competition)} '{chkCompClass.CompetitionName}' not found!");
                    Assert.That(
                        useComp.HasValue,
                        Is.True,
                        $"{nameof(Competition)} '{chkCompClass.CompetitionName}' not found");

                    if (useComp == null)
                    {
                        continue;
                    }

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
                                $"{compClassLogString} not found");
                            Assert.That(
                                foundCompClass?.DisplayInfo,
                                Is.Not.Null,
                                $"{compClassLogString} not invalid - {nameof(CompetitionClass.DisplayInfo)} missing");
                            Assert.That(
                                foundCompClass?.DisplayInfo?.ExtraParticipants,
                                Is.Not.Null,
                                $"{compClassLogString} not invalid - {nameof(CompetitionClass.DisplayInfo.ExtraParticipants)} missing");
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
                            $"{compClassLogString}: {nameof(CompetitionClass.OrgClassId)}");

                        if (string.IsNullOrEmpty(
                            chkCompClass.FollowUpCompetitionClassName))
                        {
                            Assert.That(
                                foundCompClass.FollowUpCompetitionClass,
                                Is.Null,
                                $"{compClassLogString}: {nameof(CompetitionClass.FollowUpCompetitionClass)}");
                        }
                        else
                        {
                            Assert.That(
                                foundCompClass.FollowUpCompetitionClass?.CompetitionClassName,
                                Is.EqualTo(
                                    chkCompClass.FollowUpCompetitionClassName),
                                $"{compClassLogString}: {nameof(CompetitionClass.FollowUpCompetitionClass)}");
                        }

                        if (chkCompClass.Discipline != null)
                        {
                            Assert.That(
                                foundCompClass.Discipline,
                                Is.EqualTo(
                                    OetsvConstants.Disciplines.ToDisciplines(
                                        chkCompClass.Discipline)),
                                $"{compClassLogString}: {nameof(CompetitionClass.Discipline)}");
                        }

                        if (chkCompClass.AgeClass != null)
                        {
                            Assert.That(
                                foundCompClass.AgeClass,
                                Is.EqualTo(
                                    OetsvConstants.AgeClasses.ToAgeClasses(
                                        chkCompClass.AgeClass)),
                                $"{compClassLogString}: {nameof(CompetitionClass.AgeClass)}");
                        }

                        if (chkCompClass.AgeGroup != null)
                        {
                            Assert.That(
                                foundCompClass.AgeGroup,
                                Is.EqualTo(
                                    OetsvConstants.AgeGroups.ToAgeGroup(
                                        chkCompClass.AgeGroup)),
                                $"{compClassLogString}: {nameof(CompetitionClass.AgeGroup)}");
                        }

                        if (chkCompClass.Class != null)
                        {
                            Assert.That(
                                foundCompClass.Class,
                                Is.EqualTo(
                                    OetsvConstants.Classes.ToClasses(
                                        chkCompClass.Class)),
                                $"{compClassLogString}: {nameof(CompetitionClass.Class)}");
                        }

                        if (chkCompClass.MinPointsForPromotion.HasValue)
                        {
                            Assert.That(
                                foundCompClass.MinPointsForPromotion,
                                Is.EqualTo(
                                    chkCompClass.MinPointsForPromotion),
                                $"{compClassLogString}: {nameof(CompetitionClass.MinPointsForPromotion)}");
                        }

                        if (chkCompClass.MinStartsForPromotion.HasValue)
                        {
                            Assert.That(
                                foundCompClass.MinStartsForPromotion,
                                Is.EqualTo(
                                    chkCompClass.MinStartsForPromotion),
                                $"{compClassLogString}: {nameof(CompetitionClass.MinStartsForPromotion)}");
                        }

                        if (chkCompClass.PointsForFirst.HasValue)
                        {
                            Assert.That(
                                foundCompClass.PointsForFirst,
                                Is.EqualTo(
                                    chkCompClass.PointsForFirst),
                                $"{compClassLogString}: {nameof(CompetitionClass.PointsForFirst)}");
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
                            $"{compClassLogString}: {nameof(CompetitionClassDisplayInfo.CountParticipants)}");

                        Assert.That(
                            foundCompClass.DisplayInfo.ExtraParticipants.ByWinning,
                            Is.EqualTo(
                                chkCompClass.ExtraPartByWinning),
                            $"{compClassLogString}: {nameof(ExtraParticipants.ByWinning)}");
                        Assert.That(
                            foundCompClass.DisplayInfo.ExtraParticipants.ByWinningInfo,
                            Is.Null.Or.Empty.Or.EqualTo(
                                chkCompClass.ExtraPartByWinningInfo),
                            $"{compClassLogString}: {nameof(ExtraParticipants.ByWinningInfo)}");
                        Assert.That(
                            foundCompClass.DisplayInfo.ExtraParticipants.ByPromotion,
                            Is.EqualTo(
                                chkCompClass.ExtraPartByPromotion),
                            $"{compClassLogString}: {nameof(ExtraParticipants.ByPromotion)}");
                        Assert.That(
                            foundCompClass.DisplayInfo.ExtraParticipants.ByPromotionInfo,
                            Is.Null.Or.Empty.Or.EqualTo(
                                chkCompClass.ExtraPartByPromotionInfo),
                            $"{compClassLogString}: {nameof(ExtraParticipants.ByPromotionInfo)}");
                    });
                }
            });
        }

        [Then(@"following Counts exists in Competitions of DanceCompetitionHelper ""([^""]*)""")]
        public void ThenFollowingCountsExistsInDanceCompetitionsOfCompetitionHelper(
            string danceCompHelper,
            Table table)
        {
            var checkCounts = table.CreateSet<DanceCompHelperCountsPoco>();
            var useDanceCompHelper = GetDanceCompetitionHelper(
                danceCompHelper);

            Assert.Multiple(() =>
            {
                foreach (var curChk in checkCounts)
                {
                    Assert.Multiple(() =>
                    {
                        var compId = useDanceCompHelper.GetCompetition(
                            curChk.CompetitionName);

                        Assert.That(
                            compId,
                            Is.Not.Null,
                            $"Nothign found for '{nameof(curChk.CompetitionName)}' '{curChk.CompetitionName}' (1)");
                        Assert.That(
                            compId.HasValue,
                            Is.True,
                            $"Nothign found for '{nameof(curChk.CompetitionName)}' '{curChk.CompetitionName}' (1)");

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
            });
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

            Assert.Multiple(() =>
            {
                foreach (var chkMultiStart in multiStartParticipants)
                {
                    var useComp = useDanceCompHelper.GetCompetition(
                        chkMultiStart.CompetitionName);

                    Assert.That(
                        useComp,
                        Is.Not.Null,
                        $"{nameof(Competition)} '{chkMultiStart.CompetitionName}' not found!");
                    Assert.That(
                        useComp.HasValue,
                        Is.True,
                        $"{nameof(Competition)} '{chkMultiStart.CompetitionName}' not found");

                    if (useComp == null)
                    {
                        continue;
                    }

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
                        $"Multiple Start '{chkMultiStart}' not found!");
                }
            });
        }

        [Then(@"none multiple starts exists in Competitions of DanceCompetitionHelper ""([^""]*)""")]
        public void ThenNoneMultipleStartsExistsInCompetitionDanceCompetitionHelper(
            string danceCompHelper,
            Table table)
        {
            var multiStartParticipants = table.CreateSet<ParticipantPoco>();
            var useDanceCompHelper = GetDanceCompetitionHelper(
                    danceCompHelper);

            Assert.Multiple(() =>
            {
                foreach (var chkMultipleStart in multiStartParticipants)
                {
                    var useComp = useDanceCompHelper.GetCompetition(
                        chkMultipleStart.CompetitionName);

                    Assert.That(
                        useComp,
                        Is.Not.Null,
                        $"{nameof(Competition)} '{chkMultipleStart.CompetitionName}' not found!");
                    Assert.That(
                        useComp.HasValue,
                        Is.True,
                        $"{nameof(Competition)} '{chkMultipleStart.CompetitionName}' not found");

                    if (useComp == null)
                    {
                        continue;
                    }

                    var useCompId = useComp.Value;
                    var curMultiStarter = useDanceCompHelper
                        .GetMultipleStarter(
                            useCompId)
                        .ToList();

                    Assert.That(
                        curMultiStarter.Count,
                        Is.EqualTo(0),
                        $"Found '{curMultiStarter.Count}' multiple starts in '{chkMultipleStart.CompetitionName}' instead of none!");
                }
            });
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

            Assert.Multiple(() =>
            {
                foreach (var chkPart in checkParticipants)
                {
                    var useComp = useDanceCompHelper.GetCompetition(
                        chkPart.CompetitionName);

                    Assert.That(
                        useComp,
                        Is.Not.Null,
                        $"{nameof(Competition)} '{chkPart.CompetitionName}' not found!");
                    Assert.That(
                        useComp.HasValue,
                        Is.True,
                        $"{nameof(Competition)} '{chkPart.CompetitionName}' not found");

                    if (useComp == null)
                    {
                        continue;
                    }

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
                            $"{partLogString} not found");
                        Assert.That(
                            foundPart?.DisplayInfo,
                            Is.Not.Null,
                            $"{partLogString} not invalid - {nameof(Participant.DisplayInfo)} missing");
                        Assert.That(
                            foundPart?.DisplayInfo?.PromotionInfo,
                            Is.Not.Null,
                            $"{partLogString} not invalid - {nameof(Participant.DisplayInfo)}.{nameof(ParticipantDisplayInfo.PromotionInfo)} missing");
                        Assert.That(
                            foundPart?.DisplayInfo?.MultipleStartInfo,
                            Is.Not.Null,
                            $"{partLogString} not invalid - {nameof(Participant.DisplayInfo)}.{nameof(ParticipantDisplayInfo.MultipleStartInfo)} missing");
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
                            $"{partLogString}: {nameof(CheckMultipleStartInfo.MultipleStarts)}");

                        Assert.That(
                            foundPart.DisplayInfo.PromotionInfo.PossiblePromotionA,
                            Is.EqualTo(
                                chkPart.PossiblePromotionA),
                            $"{partLogString}: {nameof(CheckPromotionInfo.PossiblePromotionA)}");
                        Assert.That(
                            foundPart.DisplayInfo.PromotionInfo.PossiblePromotionAInfo,
                            Is.Null.Or.Empty.Or.EqualTo(
                                chkPart.PossiblePromotionAInfo),
                            $"{partLogString}: {nameof(CheckPromotionInfo.PossiblePromotionAInfo)}");

                        Assert.That(
                            foundPart.DisplayInfo.PromotionInfo.PossiblePromotionB,
                            Is.EqualTo(
                                chkPart.PossiblePromotionB),
                            $"{partLogString}: {nameof(CheckPromotionInfo.PossiblePromotionB)}");
                        Assert.That(
                            foundPart.DisplayInfo.PromotionInfo.PossiblePromotionBInfo,
                            Is.Null.Or.Empty.Or.EqualTo(
                                chkPart.PossiblePromotionBInfo),
                            $"{partLogString}: {nameof(CheckPromotionInfo.PossiblePromotionBInfo)}");
                    });
                }
            });
        }

        [Then(@"following Configuration Values exists in DanceCompetitionHelper ""([^""]*)""")]
        public void ThenFollowingConfigurationValuesExistsInDanceCompetitionHelper(
            string danceCompHelper,
            Table table)
        {
            var checkCfgValues = table.CreateSet<ConfigurationValuePoco>();
            var useDanceCompHelper = GetDanceCompetitionHelper(
                danceCompHelper);

            Assert.Multiple(() =>
            {
                foreach (var curCfgVal in checkCfgValues)
                {
                    // sanity checks...
                    curCfgVal.SanityCheck();

                    var useOrganization = curCfgVal.Organization;
                    Competition? useComp = null;
                    CompetitionClass? useCompClass = null;
                    CompetitionVenue? useCompVenue = null;

                    if (string.IsNullOrEmpty(
                        curCfgVal.CompetitionName) == false)
                    {
                        useComp = useDanceCompHelper
                            .GetCompetition(
                                useDanceCompHelper.GetCompetition(
                                    curCfgVal.CompetitionName ?? string.Empty));

                        Assert.That(
                            useComp,
                            Is.Not.Null,
                            $"[{curCfgVal}]: {nameof(Competition)} '{curCfgVal.CompetitionName}' is missing");

                        if (useComp == null)
                        {
                            continue;
                        }
                    }

                    if (string.IsNullOrEmpty(
                        curCfgVal.CompetitionClassName) == false)
                    {
                        var useCompetitionClassId = useDanceCompHelper.GetCompetitionClass(
                            curCfgVal.CompetitionClassName);

                        useCompClass = useDanceCompHelper
                            .GetCompetitionClasses(
                                useComp?.CompetitionId ?? Guid.Empty)
                            ?.FirstOrDefault(
                                x => x.CompetitionClassId == useCompetitionClassId);

                        Assert.That(
                            useCompClass,
                            Is.Not.Null,
                            $"[{curCfgVal}]: {nameof(CompetitionClass)} '{curCfgVal.CompetitionClassName}' is missing");

                        if (useCompClass == null)
                        {
                            continue;
                        }

                        Assert.That(
                            useCompClass.CompetitionId,
                            Is.EqualTo(
                                useComp?.CompetitionId),
                            $"[{curCfgVal}]: {nameof(CompetitionClass)} '{curCfgVal.CompetitionClassName}' ID missmatch");
                    }

                    if (string.IsNullOrEmpty(
                        curCfgVal.CompetitionVenueName) == false)
                    {
                        // ToDo: extend when "CompetitionVenue" implemented
                        useCompVenue = null;
                    }

                    ConfigurationValue? foundCfg = null;

                    if (foundCfg == null
                        && useOrganization == null
                        && useComp == null
                        && useCompClass == null
                        && useCompVenue == null)
                    {
                        foundCfg = useDanceCompHelper.GetConfiguration(
                            curCfgVal.Key);
                    }

                    if (foundCfg == null
                        && useOrganization != null
                        && useComp != null
                        && useCompClass != null
                        && useCompVenue != null)
                    {
                        foundCfg = useDanceCompHelper.GetConfiguration(
                            curCfgVal.Key,
                            useCompClass,
                            useCompVenue);
                    }

                    if (foundCfg == null
                        && useOrganization != null
                        && useComp != null
                        && useCompClass != null)
                    {
                        foundCfg = useDanceCompHelper.GetConfiguration(
                            curCfgVal.Key,
                            useCompClass);
                    }

                    if (foundCfg == null
                        && useOrganization != null
                        && useComp != null)
                    {
                        foundCfg = useDanceCompHelper.GetConfiguration(
                            curCfgVal.Key,
                            useComp);
                    }

                    if (foundCfg == null
                        && useOrganization != null)
                    {
                        foundCfg = useDanceCompHelper.GetConfiguration(
                            curCfgVal.Key,
                            useOrganization.Value);
                    }

                    // check...
                    if (string.IsNullOrEmpty(
                        curCfgVal.Value))
                    {
                        Assert.That(
                            foundCfg?.Value,
                            Is.Null.Or.Empty,
                            $"[{curCfgVal}]: {nameof(ConfigurationValue.Value)} missmatch (1)");
                    }
                    else
                    {
                        Assert.That(
                            foundCfg?.Value,
                            Is.Not.Null.And.EqualTo(
                                curCfgVal.Value),
                            $"[{curCfgVal}]: {nameof(ConfigurationValue.Value)} missmatch (2)");
                    }
                }
            });
        }

        #endregion Dance Competition Helper

        [AfterScenario]
        public void AfterScenario()
        {
            DisposeAllCreated();
        }
    }
}
