using DanceCompetitionHelper.Config;
using DanceCompetitionHelper.OrgImpl.Oetsv;
using DanceCompetitionHelper.OrgImpl.Oetsv.WorkData;
using Microsoft.Extensions.Logging;
using TestHelper.Extensions;

namespace DanceCompetitionHelper.Test.Tests.UnitTests.OrgImpl.Oetsv
{
    [TestFixture]
    public class OetsvCompetitionImporterTests
    {
        [Test]
        public void ExtractData_CompetitionImport01()
        {
            var fakeLogger = new Mock<ILogger<OetsvCompetitionImporter>>();
            var testImporter = new OetsvCompetitionImporter(
                fakeLogger.Object,
                new ImporterSettings());

            var rootPath = AssemblyExtensions.GetAssemblyPath() ?? string.Empty;

            testImporter.ExtractData(
                Path.Combine(
                    rootPath,
                    @"TestData\Importer\Oetsv\CompetitionImport01_Competition.csv"),
                Path.Combine(
                    rootPath,
                    @"TestData\Importer\Oetsv\CompetitionImport01_Participants.csv"));

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
                             // "ALG;00;D;STA;*Allg.Kl. Sta D"
                             new CompetitionClassImport()
                             {
                                 OrgClassIdRaw = "1",
                                 OrgClassId = "1",

                                 AgeClassRaw = "ALG",
                                 AgeClass = OetsvConstants.AgeClasses.Adult,

                                 AgeGroupRaw = "00",
                                 AgeGroup = OetsvConstants.AgeGroups.GroupNone,

                                 ClassRaw = "D",
                                 Class = OetsvConstants.Classes.D,

                                 DisciplineRaw = "STA",
                                 Discipline = OetsvConstants.Disciplines.Sta,

                                 NameRaw = "*Allg.Kl. Sta D",
                                 Name = "Allg.Kl. Sta D",
                             },
                             // "2;ALG;00;C;STA;*Allg.Kl. Sta C"
                             new CompetitionClassImport()
                             {
                                 OrgClassIdRaw = "2",
                                 OrgClassId = "2",

                                 AgeClassRaw = "ALG",
                                 AgeClass = OetsvConstants.AgeClasses.Adult,

                                 AgeGroupRaw = "00",
                                 AgeGroup = OetsvConstants.AgeGroups.GroupNone,

                                 ClassRaw = "C",
                                 Class = OetsvConstants.Classes.C,

                                 DisciplineRaw = "STA",
                                 Discipline = OetsvConstants.Disciplines.Sta,

                                 NameRaw = "*Allg.Kl. Sta C",
                                 Name = "Allg.Kl. Sta C",
                             },
                             // "3;ALG;00;B;LA;*Allg.Kl. La B"
                             new CompetitionClassImport()
                             {
                                 OrgClassIdRaw = "3",
                                 OrgClassId = "3",

                                 AgeClassRaw = "ALG",
                                 AgeClass = OetsvConstants.AgeClasses.Adult,

                                 AgeGroupRaw = "00",
                                 AgeGroup = OetsvConstants.AgeGroups.GroupNone,

                                 ClassRaw = "B",
                                 Class = OetsvConstants.Classes.B,

                                 DisciplineRaw = "LA",
                                 Discipline = OetsvConstants.Disciplines.La,

                                 NameRaw = "*Allg.Kl. La B",
                                 Name = "Allg.Kl. La B",
                             },
                             // "4;ALG;00;B;STA;*Allg.Kl. Sta B"
                             new CompetitionClassImport()
                             {
                                 OrgClassIdRaw = "4",
                                 OrgClassId = "4",

                                 AgeClassRaw = "ALG",
                                 AgeClass = OetsvConstants.AgeClasses.Adult,

                                 AgeGroupRaw = "00",
                                 AgeGroup = OetsvConstants.AgeGroups.GroupNone,

                                 ClassRaw = "B",
                                 Class = OetsvConstants.Classes.B,

                                 DisciplineRaw = "STA",
                                 Discipline = OetsvConstants.Disciplines.Sta,

                                 NameRaw = "*Allg.Kl. Sta B",
                                 Name = "Allg.Kl. Sta B",
                             },
                             // "5;ALG;00;A;LA;*Allg.Kl. La A"
                             new CompetitionClassImport()
                             {
                                 OrgClassIdRaw = "5",
                                 OrgClassId = "5",

                                 AgeClassRaw = "ALG",
                                 AgeClass = OetsvConstants.AgeClasses.Adult,

                                 AgeGroupRaw = "00",
                                 AgeGroup = OetsvConstants.AgeGroups.GroupNone,

                                 ClassRaw = "A",
                                 Class = OetsvConstants.Classes.A,

                                 DisciplineRaw = "LA",
                                 Discipline = OetsvConstants.Disciplines.La,

                                 NameRaw = "*Allg.Kl. La A",
                                 Name = "Allg.Kl. La A",
                             },
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

            Assert.That(
                testImporter.Participants,
                Is.EqualTo(
                    new[]
                    {
                        new CompetitionParticipantImport()
                        {
                            RegOrgClassIdRaw = "1",
                            RegOrgClassId = "1",

                            RegStartNumberRaw = "1",
                            RegStartNumber = 1,

                            RegPartALastNameRaw = "Doe",
                            RegPartAFirstNameRaw = "John",
                            RegPartAName = "John Doe",

                            RegPartBLastNameRaw = "Doe",
                            RegPartBFirstNameRaw = "Jane",
                            RegPartBName = "Jane Doe",

                            RegClubNameRaw = "Choice Styria",
                            RegClubName = "Choice Styria",

                            RegStateRaw = "Steiermark",
                            RegState = "Steiermark",

                            RegStateAbbrRaw = "St",
                            RegStateAbbr = "St",

                            RegDisciplineRaw = "LA",
                            RegDiscipline = OetsvConstants.Disciplines.La,

                            RegAgeClassRaw = "SCH",
                            RegAgeClass = OetsvConstants.AgeClasses.Juvenile,

                            RegAgeGroupRaw = "00",
                            RegAgeGroup = OetsvConstants.AgeGroups.GroupNone,

                            RegClassRaw = "D",
                            RegClass = OetsvConstants.Classes.D,

                            OrgCurrentClassRaw = "D",
                            OrgCurrentClass = OetsvConstants.Classes.D,

                            RegPartAOrgIdRaw = "6887",
                            RegPartAOrgId = "6887",

                            RegPartBOrgIdRaw = "7283",
                            RegPartBOrgId = "7283",

                            RegClubOrgIdRaw = "607",
                            RegClubOrgId = "607",

                            OrgPointsRaw = "594",
                            OrgPoints = 594,

                            OrgStartsRaw = "13",
                            OrgStarts = 13,

                            OrgMinPointsForPromotionRaw = "1000",
                            OrgMinPointsForPromotion = 1000,

                            OrgMinStartsForPromotionRaw = "10",
                            OrgMinStartsForPromotion = 10,
                        },
                        new CompetitionParticipantImport()
                        {
                            RegOrgClassIdRaw = "2",
                            RegOrgClassId = "2",

                            RegStartNumberRaw = "2",
                            RegStartNumber = 2,

                            RegPartALastNameRaw = "Summer",
                            RegPartAFirstNameRaw = "John",
                            RegPartAName = "John Summer",

                            RegPartBLastNameRaw = "Summer",
                            RegPartBFirstNameRaw = "Jane",
                            RegPartBName = "Jane Summer",

                            RegClubNameRaw = "Unknown",
                            RegClubName = "Unknown",

                            RegStateRaw = "Wien",
                            RegState = "Wien",

                            RegStateAbbrRaw = "W",
                            RegStateAbbr = "W",

                            RegDisciplineRaw = "STA",
                            RegDiscipline = OetsvConstants.Disciplines.Sta,

                            RegAgeClassRaw = "SCH",
                            RegAgeClass = OetsvConstants.AgeClasses.Juvenile,

                            RegAgeGroupRaw = "00",
                            RegAgeGroup = OetsvConstants.AgeGroups.GroupNone,

                            RegClassRaw = "C",
                            RegClass = OetsvConstants.Classes.C,

                            OrgCurrentClassRaw = "C",
                            OrgCurrentClass = OetsvConstants.Classes.C,

                            RegPartAOrgIdRaw = "6889",
                            RegPartAOrgId = "6889",

                            RegPartBOrgIdRaw = "7285",
                            RegPartBOrgId = "7285",

                            RegClubOrgIdRaw = "666",
                            RegClubOrgId = "666",

                            OrgPointsRaw = "123",
                            OrgPoints = 123,

                            OrgStartsRaw = "4",
                            OrgStarts = 4,

                            OrgMinPointsForPromotionRaw = "2000",
                            OrgMinPointsForPromotion = 2000,

                            OrgMinStartsForPromotionRaw = "10",
                            OrgMinStartsForPromotion = 10,
                        },
                    }),
                nameof(testImporter.Participants));
        }

        [Test]
        [Ignore("only for debugging!")]
        public void ImportOrUpdateByUrl_1508()
        {
            var fakeLogger = new Mock<ILogger<OetsvCompetitionImporter>>();
            var testImporter = new OetsvCompetitionImporter(
                fakeLogger.Object,
                new ImporterSettings());

            testImporter.ImportOrUpdateByUrl(
                null,
                testImporter.GetCompetitioUriForOrgId(
                    1508),
                null,
                testImporter.GetParticipantsUriForOrgId(
                    1508));
        }
    }
}
