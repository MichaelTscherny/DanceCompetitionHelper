using DanceCompetitionHelper.Database.Test.Pocos.DanceCompetitionHelper;
using DanceCompetitionHelper.Test.Bindings;

namespace DanceCompetitionHelper.Test.Tests.UnitTests
{
    [TestFixture]
    public class BindingBaseTests
    {
        public static readonly object[][] SortForCreation_TestData = new object[][]
        {
            new object[]
            {
                "null",
                null,
                new List<CompetitionClassPoco>(),
            },
            new object[]
            {
                "empty",
                new List<CompetitionClassPoco>(),
                new List<CompetitionClassPoco>(),
            },
            new object[]
            {
                "Single 01",
                new List<CompetitionClassPoco>()
                {
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta D",
                        FollowUpCompetitionClassName = null,
                    },
                },
                new List<CompetitionClassPoco>()
                {
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta D",
                        FollowUpCompetitionClassName = null,
                    },
                },
            },
            new object[]
            {
                "Single 02",
                new List<CompetitionClassPoco>()
                {
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta C",
                        FollowUpCompetitionClassName = null,
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta D",
                        FollowUpCompetitionClassName = null,
                    },
                },
                new List<CompetitionClassPoco>()
                {
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta C",
                        FollowUpCompetitionClassName = null,
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta D",
                        FollowUpCompetitionClassName = null,
                    },
                },
            },
            new object[]
            {
                "Dependent 01",
                new List<CompetitionClassPoco>()
                {
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta D",
                        FollowUpCompetitionClassName = "Allg. Sta C",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta C",
                        FollowUpCompetitionClassName = null,
                    },
                },
                new List<CompetitionClassPoco>()
                {
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta C",
                        FollowUpCompetitionClassName = null,
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta D",
                        FollowUpCompetitionClassName = "Allg. Sta C",
                    },
                },
            },
            new object[]
            {
                "Dependent 02",
                new List<CompetitionClassPoco>()
                {
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta D",
                        FollowUpCompetitionClassName = "Allg. Sta C",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta C",
                        FollowUpCompetitionClassName = "Allg. Sta B",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta B",
                        FollowUpCompetitionClassName = "Allg. Sta A",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta A",
                        FollowUpCompetitionClassName = "Allg. Sta S",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta S",
                        FollowUpCompetitionClassName = null,
                    },
                },
                new List<CompetitionClassPoco>()
                {
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta S",
                        FollowUpCompetitionClassName = null,
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta A",
                        FollowUpCompetitionClassName = "Allg. Sta S",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta B",
                        FollowUpCompetitionClassName = "Allg. Sta A",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta C",
                        FollowUpCompetitionClassName = "Allg. Sta B",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta D",
                        FollowUpCompetitionClassName = "Allg. Sta C",
                    },
                },
            },
            new object[]
            {
                "Dependent 03",
                new List<CompetitionClassPoco>()
                {
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta D",
                        FollowUpCompetitionClassName = "Allg. Sta C",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta C",
                        FollowUpCompetitionClassName = "Allg. Sta B",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta B",
                        FollowUpCompetitionClassName = "Allg. Sta A",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta A",
                        FollowUpCompetitionClassName = "Allg. Sta S",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta S",
                        FollowUpCompetitionClassName = null,
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. La D",
                        FollowUpCompetitionClassName = "Allg. La C",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. La C",
                        FollowUpCompetitionClassName = "Allg. La B",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. La B",
                        FollowUpCompetitionClassName = "Allg. La A",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. La A",
                        FollowUpCompetitionClassName = "Allg. La S",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. La S",
                        FollowUpCompetitionClassName = null,
                    },
                },
                new List<CompetitionClassPoco>()
                {
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta S",
                        FollowUpCompetitionClassName = null,
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. La S",
                        FollowUpCompetitionClassName = null,
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta A",
                        FollowUpCompetitionClassName = "Allg. Sta S",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. La A",
                        FollowUpCompetitionClassName = "Allg. La S",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta B",
                        FollowUpCompetitionClassName = "Allg. Sta A",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. La B",
                        FollowUpCompetitionClassName = "Allg. La A",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta C",
                        FollowUpCompetitionClassName = "Allg. Sta B",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. La C",
                        FollowUpCompetitionClassName = "Allg. La B",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. Sta D",
                        FollowUpCompetitionClassName = "Allg. Sta C",
                    },
                    new CompetitionClassPoco()
                    {
                        CompetitionClassName = "Allg. La D",
                        FollowUpCompetitionClassName = "Allg. La C",
                    },
                },
            },
        };

        [Test]
        [TestCaseSource(nameof(SortForCreation_TestData))]
        public void SortForCreation_Test(
            string name,
            List<CompetitionClassPoco> toSort,
            List<CompetitionClassPoco> expected)
        {
            Assert.That(
                BindingBase.SortForCreation(
                    toSort),
                Is.EqualTo(
                    expected),
                $"{name}");
        }
    }
}
