using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Database.Test.Tests.UnitTests
{
    [TestFixture]
    internal class ConfigurationValueTests
    {
        public static readonly Guid TestGuid01 = new Guid("11111111-1111-1111-1111-111111111111");
        public static readonly Guid TestGuid02 = new Guid("22222222-2222-2222-2222-222222222222");
        public static readonly Guid TestGuid03 = new Guid("33333333-3333-3333-3333-333333333333");

        public static readonly object[][] Scope_TestData = new object[][]
        {
            new object[]
            {
                ConfigurationScopeEnum.Global.ToString() + " 01",
                new ConfigurationValue()
                {
                },
                ConfigurationScopeEnum.Global,
            },
            new object[]
            {
                ConfigurationScopeEnum.Global.ToString() + " 02",
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Any,
                },
                ConfigurationScopeEnum.Global,
            },
            new object[]
            {
                ConfigurationScopeEnum.Organization.ToString(),
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Oetsv,
                },
                ConfigurationScopeEnum.Organization,
            },
            new object[]
            {
                ConfigurationScopeEnum.Competition.ToString(),
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Oetsv,
                    CompetitionId = TestGuid01,
                },
                ConfigurationScopeEnum.Competition,
            },
            new object[]
            {
                ConfigurationScopeEnum.CompetitionClass.ToString(),
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Oetsv,
                    CompetitionId = TestGuid01,
                    CompetitionClassId = TestGuid02,
                },
                ConfigurationScopeEnum.CompetitionClass,
            },
            new object[]
            {
                ConfigurationScopeEnum.CompetitionVenue.ToString() + " 01",
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Oetsv,
                    CompetitionId = TestGuid01,
                    CompetitionClassId = TestGuid02,
                    CompetitionVenueId = TestGuid03,
                },
                ConfigurationScopeEnum.CompetitionVenue,
            },
            new object[]
            {
                ConfigurationScopeEnum.CompetitionVenue.ToString() + " 02",
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Oetsv,
                    CompetitionId = TestGuid01,
                    // CompetitionClassId = TestGuid02,
                    CompetitionVenueId = TestGuid03,
                },
                ConfigurationScopeEnum.CompetitionVenue,
            },
        };

        [Test]
        [TestCaseSource(nameof(Scope_TestData))]
        public void Scope_Test(
            string name,
            ConfigurationValue value,
            ConfigurationScopeEnum expectedScope)
        {
            Assert.That(
                value.Scope,
                Is.EqualTo(
                    expectedScope),
                $"[{name}] {nameof(ConfigurationValue.Scope)}");
        }

        public static readonly object[][] SanityCheck_TestData = new object[][]
        {
            // --------------------
            new object[]
            {
                "OK 01",
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Oetsv,
                    CompetitionId = TestGuid01,
                    CompetitionClassId = TestGuid02,
                    CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                false,
            },
            new object[]
            {
                "OK 02",
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Oetsv,
                    CompetitionId = TestGuid01,
                    // CompetitionClassId = TestGuid02,
                    CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                false,
            },
            new object[]
            {
                "OK 03",
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Oetsv,
                    CompetitionId = TestGuid01,
                    CompetitionClassId = TestGuid02,
                    // CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                false,
            },
            new object[]
            {
                "OK 04",
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Oetsv,
                    CompetitionId = TestGuid01,
                    // CompetitionClassId = TestGuid02,
                    // CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                false,
            },
            new object[]
            {
                "OK 05",
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Oetsv,
                    // CompetitionId = TestGuid01,
                    // CompetitionClassId = TestGuid02,
                    // CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                false,
            },
            new object[]
            {
                "OK 06",
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Any,
                    // CompetitionId = TestGuid01,
                    // CompetitionClassId = TestGuid02,
                    // CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                false,
            },
            new object[]
            {
                "OK 07",
                new ConfigurationValue()
                {
                    // Organization = OrganizationEnum.Any,
                    // CompetitionId = TestGuid01,
                    // CompetitionClassId = TestGuid02,
                    // CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                false,
            },
            // --------------------
            // -- 
            new object[]
            {
                "Error 00",
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Oetsv,
                    CompetitionId = TestGuid01,
                    CompetitionClassId = TestGuid02,
                    CompetitionVenueId = TestGuid03,
                    Key = string.Empty,
                },
                true,
            },
            // -- 
            new object[]
            {
                "Error 01-01",
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Any,
                    CompetitionId = TestGuid01,
                    CompetitionClassId = TestGuid02,
                    CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                true,
            },
            new object[]
            {
                "Error 01-02",
                new ConfigurationValue()
                {
                    // Organization = OrganizationEnum.Oetsv,
                    CompetitionId = TestGuid01,
                    CompetitionClassId = TestGuid02,
                    CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                true,
            },
            new object[]
            {
                "Error 01-03",
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Oetsv,
                    // CompetitionId = TestGuid01,
                    CompetitionClassId = TestGuid02,
                    CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                true,
            },
            new object[]
            {
                "Error 01-04",
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Any,
                    CompetitionId = TestGuid01,
                    // CompetitionClassId = TestGuid02,
                    CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                true,
            },
            // --
            new object[]
            {
                "Error 02-01",
                new ConfigurationValue()
                {
                    // Organization = OrganizationEnum.Oetsv,
                    CompetitionId = TestGuid01,
                    CompetitionClassId = TestGuid02,
                    // CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                true,
            },
            new object[]
            {
                "Error 02-02",
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Oetsv,
                    // CompetitionId = TestGuid01,
                    CompetitionClassId = TestGuid02,
                    // CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                true,
            },
            // --
            new object[]
            {
                "Error 03-01",
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Oetsv,
                    // CompetitionId = TestGuid01,
                    CompetitionClassId = TestGuid02,
                    // CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                true,
            },
            new object[]
            {
                "Error 03-02",
                new ConfigurationValue()
                {
                    // Organization = OrganizationEnum.Oetsv,
                    CompetitionId = TestGuid01,
                    CompetitionClassId = TestGuid02,
                    // CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                true,
            },
            // --
            new object[]
            {
                "Error 04-01",
                new ConfigurationValue()
                {
                    // Organization = OrganizationEnum.Any,
                    CompetitionId = TestGuid01,
                    // CompetitionClassId = TestGuid02,
                    // CompetitionVenueId = TestGuid03,
                    Key = nameof(ConfigurationValue.Key),
                },
                true,
            },
        };

        /// <summary>
        /// Yes, this is similar to <see cref="DanceCompetitionHelper.Test.Tests.UnitTests.Pocos.ConfigurationValuePocoTests.SanityCheck_Test"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="toCheck"></param>
        /// <param name="throwsException"></param>
        [Test]
        [TestCaseSource(nameof(SanityCheck_TestData))]
        public void SanityCheck_Test(
            string name,
            ConfigurationValue toCheck,
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
