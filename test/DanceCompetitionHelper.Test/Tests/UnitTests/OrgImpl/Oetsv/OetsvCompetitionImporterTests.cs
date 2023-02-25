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
                    testImporter.OrgCompetitionId,
                    Is.EqualTo("6451"),
                    nameof(testImporter.OrgCompetitionId));
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
                Assert.That(
                    testImporter.CompetitionClasses,
                    Is.EqualTo(
                         new[]
                         {
                             "ALG;00;D;STA;*Allg.Kl. Sta D",
                             "ALG;00;C;STA;*Allg.Kl. Sta C",
                             "ALG;00;B;LA;*Allg.Kl. La B",
                             "ALG;00;B;STA;*Allg.Kl. Sta B",
                             "ALG;00;A;LA;*Allg.Kl. La A",
                         }),
                    nameof(testImporter.CompetitionClasses));
                Assert.That(
                    testImporter.MastersOfCeremony,
                    Is.EqualTo(
                         new[]
                         {
                             "Master Of Ceremonies, / Wien",
                             "Master Of The Universe, / Wien",
                         }),
                    nameof(testImporter.MastersOfCeremony));
                Assert.That(
                    testImporter.Chairperson,
                    Is.EqualTo(
                         new string[]
                         {
                         }),
                    nameof(testImporter.Chairperson));
                Assert.That(
                    testImporter.Assessors,
                    Is.EqualTo(
                         new[]
                         {
                             "Assessors 01 / Wien",
                             "Assessors 02 / Wien",
                         }),
                    nameof(testImporter.Assessors));
                Assert.That(
                    testImporter.Adjudicators,
                    Is.EqualTo(
                         new[]
                         {
                             "Adjudicator 01, Wien",
                             "Adjudicator 02, Wien",
                             "Adjudicator 03, Wels",
                             "Adjudicator 04, Graz",
                             "Adjudicator 05, Salzburg",
                         }),
                    nameof(testImporter.Adjudicators));
            });
        }
    }
}
