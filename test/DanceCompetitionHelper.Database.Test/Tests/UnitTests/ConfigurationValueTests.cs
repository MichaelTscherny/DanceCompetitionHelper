using DanceCompetitionHelper.Database.Enum;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Database.Test.Tests.UnitTests
{
    [TestFixture]
    public class ConfigurationValueTests
    {
        public static readonly Guid TestGuid01 = new Guid("11111111-1111-1111-1111-111111111111");
        public static readonly Guid TestGuid02 = new Guid("22222222-2222-2222-2222-222222222222");
        public static readonly Guid TestGuid03 = new Guid("33333333-3333-3333-3333-333333333333");

        public static readonly object[][] Scope_TestData = new object[][]
        {
            new object[]
            {
                ConfigurationScopeEnum.Global.ToString(),
                new ConfigurationValue()
                {
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
                ConfigurationScopeEnum.CompetitionVenue.ToString(),
                new ConfigurationValue()
                {
                    Organization = OrganizationEnum.Oetsv,
                    CompetitionId = TestGuid01,
                    CompetitionClassId = TestGuid02,
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
    }
}
