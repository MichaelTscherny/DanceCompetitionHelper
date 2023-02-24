using DanceCompetitionHelper.OrgImpl.Oetsv;
using Microsoft.Extensions.Logging;
using TestHelper.Extensions;

namespace DanceCompetitionHelper.Test.Tests.UnitTests.OrgImpl.Oetsv
{
    [TestFixture]
    public class OetsvCompetitionImporterTests
    {
        [Test]
        public void Import_CompetitionImport01()
        {
            var fakeLogger = new Mock<ILogger<OetsvCompetitionImporter>>();
            var testImporter = new OetsvCompetitionImporter(
                fakeLogger.Object);

            testImporter.ExtractData(
                Path.Combine(
                    AssemblyExtensions.GetAssemblyPath() ?? string.Empty,
                    @"TestData\Importer\Oetsv\CompetitionImport01.csv"));

            Assert.Multiple(() =>
            {
                Assert.That(
                    testImporter.Oranizer,
                    Is.EqualTo("UTSC Forum - Wien"),
                    nameof(testImporter.Oranizer));
                Assert.That(
                    testImporter.CompetitionName,
                    Is.EqualTo("Hans-Rueff-Gedächtnispokal + Sportunion-Bundesmeisterschaft"),
                    nameof(testImporter.CompetitionName));
                Assert.That(
                    testImporter.CompetitionLocation,
                    Is.EqualTo("HdB Floridsdorf"),
                    nameof(testImporter.CompetitionLocation));
                Assert.That(
                    testImporter.CompetitionAddress,
                    Is.EqualTo("Wien"),
                    nameof(testImporter.CompetitionAddress));
                Assert.That(
                    testImporter.CompetitionDate,
                    Is.EqualTo(new DateTime(2013, 5, 12)),
                    nameof(testImporter.CompetitionDate));
                Assert.That(
                    testImporter.CompetitionType,
                    Is.EqualTo(
                         OetsvConstants.CompetitionType.Bewertungsturnier),
                    nameof(testImporter.CompetitionType));
            });
        }
    }
}
