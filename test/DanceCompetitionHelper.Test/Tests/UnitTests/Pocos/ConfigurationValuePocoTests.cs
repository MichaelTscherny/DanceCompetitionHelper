using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Test.Pocos.DanceCompetitionHelper;

namespace DanceCompetitionHelper.Test.Tests.UnitTests.Pocos
{
    [TestFixture]
    internal class ConfigurationValuePocoTests
    {
        public static readonly object[][] SanityCheck_TestData = new object[][]
        {
            // --------------------
            new object[]
            {
                "OK 01",
                new ConfigurationValuePoco()
                {
                    Organization = OrganizationEnum.Any,
                    CompetitionName = nameof(ConfigurationValuePoco.CompetitionName),
                    CompetitionClassName = nameof(ConfigurationValuePoco.CompetitionClassName),
                    CompetitionVenueName = nameof(ConfigurationValuePoco.CompetitionVenueName),
                    Key = nameof(ConfigurationValuePoco.Key),
                },
                false,
            },
            new object[]
            {
                "OK 02",
                new ConfigurationValuePoco()
                {
                    Organization = OrganizationEnum.Any,
                    CompetitionName = nameof(ConfigurationValuePoco.CompetitionName),
                    CompetitionClassName = nameof(ConfigurationValuePoco.CompetitionClassName),
                    Key = nameof(ConfigurationValuePoco.Key),
                },
                false,
            },
            new object[]
            {
                "OK 03",
                new ConfigurationValuePoco()
                {
                    Organization = OrganizationEnum.Any,
                    CompetitionName = nameof(ConfigurationValuePoco.CompetitionName),
                    Key = nameof(ConfigurationValuePoco.Key),
                },
                false,
            },
            new object[]
            {
                "OK 04",
                new ConfigurationValuePoco()
                {
                    Organization = OrganizationEnum.Any,
                    Key = nameof(ConfigurationValuePoco.Key),
                },
                false,
            },
            new object[]
            {
                "OK 05",
                new ConfigurationValuePoco()
                {
                    Key = nameof(ConfigurationValuePoco.Key),
                },
                false,
            },
            // --------------------
            // -- 
            new object[]
            {
                "Error 00",
                new ConfigurationValuePoco()
                {
                    Organization = OrganizationEnum.Any,
                    CompetitionName = nameof(ConfigurationValuePoco.CompetitionName),
                    CompetitionClassName = nameof(ConfigurationValuePoco.CompetitionClassName),
                    CompetitionVenueName = nameof(ConfigurationValuePoco.CompetitionVenueName),
                    Key = string.Empty,
                },
                true,
            },
            // -- 
            new object[]
            {
                "Error 01-01",
                new ConfigurationValuePoco()
                {
                    // Organization = OrganizationEnum.Any,
                    CompetitionName = nameof(ConfigurationValuePoco.CompetitionName),
                    CompetitionClassName = nameof(ConfigurationValuePoco.CompetitionClassName),
                    CompetitionVenueName = nameof(ConfigurationValuePoco.CompetitionVenueName),
                    Key = nameof(ConfigurationValuePoco.Key),
                },
                true,
            },
            new object[]
            {
                "Error 01-02",
                new ConfigurationValuePoco()
                {
                    Organization = OrganizationEnum.Any,
                    // CompetitionName = nameof(ConfigurationValuePoco.CompetitionName),
                    CompetitionClassName = nameof(ConfigurationValuePoco.CompetitionClassName),
                    CompetitionVenueName = nameof(ConfigurationValuePoco.CompetitionVenueName),
                    Key = nameof(ConfigurationValuePoco.Key),
                },
                true,
            },
            new object[]
            {
                "Error 01-03",
                new ConfigurationValuePoco()
                {
                    Organization = OrganizationEnum.Any,
                    CompetitionName = nameof(ConfigurationValuePoco.CompetitionName),
                    // CompetitionClassName = nameof(ConfigurationValuePoco.CompetitionClassName),
                    CompetitionVenueName = nameof(ConfigurationValuePoco.CompetitionVenueName),
                    Key = nameof(ConfigurationValuePoco.Key),
                },
                true,
            },
            // --
            new object[]
            {
                "Error 02-01",
                new ConfigurationValuePoco()
                {
                    // Organization = OrganizationEnum.Any,
                    CompetitionName = nameof(ConfigurationValuePoco.CompetitionName),
                    CompetitionClassName = nameof(ConfigurationValuePoco.CompetitionClassName),
                    Key = nameof(ConfigurationValuePoco.Key),
                },
                true,
            },
            new object[]
            {
                "Error 02-02",
                new ConfigurationValuePoco()
                {
                    Organization = OrganizationEnum.Any,
                    // CompetitionName = nameof(ConfigurationValuePoco.CompetitionName),
                    CompetitionClassName = nameof(ConfigurationValuePoco.CompetitionClassName),
                    Key = nameof(ConfigurationValuePoco.Key),
                },
                true,
            },
            // --
            new object[]
            {
                "Error 03-01",
                new ConfigurationValuePoco()
                {
                    // Organization = OrganizationEnum.Any,
                    CompetitionName = nameof(ConfigurationValuePoco.CompetitionName),
                    Key = nameof(ConfigurationValuePoco.Key),
                },
                true,
            },
        };

        /// <summary>
        /// Yes, this is similar to <see cref="DanceCompetitionHelper.Database.Test.Tests.UnitTests.ConfigurationValueTests.SanityCheck_Test"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="toCheck"></param>
        /// <param name="throwsException"></param>
        [Test]
        [TestCaseSource(nameof(SanityCheck_TestData))]
        public void SanityCheck_Test(
            string name,
            ConfigurationValuePoco toCheck,
            bool throwsException)
        {
            if (throwsException)
            {
                Assert.Throws<ArgumentNullException>(
                    toCheck.SanityCheck,
                    name);
            }
            else
            {
                toCheck.SanityCheck();
            }
        }
    }
}
