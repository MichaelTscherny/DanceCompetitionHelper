using static DanceCompetitionHelper.OrgImpl.Oetsv.OetsvConstants;

namespace DanceCompetitionHelper.Test.Tests.UnitTests.OrgImpl.Oetsv.OetsvConstants
{
    [TestFixture]
    public class AgeClassesTests
    {
        [Test]
        // -------------
        [TestCase(AgeClasses.Pupil, AgeClasses.Pupil)]
        [TestCase("SCH", AgeClasses.Pupil)]
        [TestCase("Schüler", AgeClasses.Pupil)]
        [TestCase("schüler", AgeClasses.Pupil)]
        [TestCase("SCHÜLER", AgeClasses.Pupil)]
        [TestCase("Schueler", AgeClasses.Pupil)]
        [TestCase("SCHUELER", AgeClasses.Pupil)]
        [TestCase("schueler", AgeClasses.Pupil)]
        // -------------
        [TestCase(AgeClasses.Junior, AgeClasses.Junior)]
        [TestCase("Junior", AgeClasses.Junior)]
        [TestCase("JUNIOR", AgeClasses.Junior)]
        [TestCase("junior", AgeClasses.Junior)]
        [TestCase("JUN", AgeClasses.Junior)]
        // -------------
        [TestCase(AgeClasses.Juvenile, AgeClasses.Juvenile)]
        [TestCase("Juvenile", AgeClasses.Juvenile)]
        [TestCase("Juv", AgeClasses.Juvenile)]
        [TestCase("JUV", AgeClasses.Juvenile)]
        [TestCase("JUG", AgeClasses.Juvenile)]
        // -------------
        [TestCase(AgeClasses.Adult, AgeClasses.Adult)]
        [TestCase("ALLG", AgeClasses.Adult)]
        [TestCase("Adult", AgeClasses.Adult)]
        [TestCase("ADULT", AgeClasses.Adult)]
        [TestCase("Adt", AgeClasses.Adult)]
        [TestCase("ADT", AgeClasses.Adult)]
        // -------------
        [TestCase(AgeClasses.Senior, AgeClasses.Senior)]
        [TestCase("Senior", AgeClasses.Senior)]
        [TestCase("sen", AgeClasses.Senior)]
        [TestCase("SEN", AgeClasses.Senior)]
        // -------------
        [TestCase(AgeClasses.Formation, AgeClasses.Formation)]
        [TestCase("FOR", AgeClasses.Formation)]
        [TestCase("FORM", AgeClasses.Formation)]
        [TestCase("form", AgeClasses.Formation)]
        [TestCase("Form", AgeClasses.Formation)]
        [TestCase("Formation", AgeClasses.Formation)]
        [TestCase("formation", AgeClasses.Formation)]
        [TestCase("FORMATION", AgeClasses.Formation)]
        // -------------
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("dummy", "dummy")]
        public void ToAgeClasses_Test(
            string input,
            string expected)
        {
            Assert.That(
                AgeClasses.ToAgeClasses(
                    input),
                Is.EqualTo(
                    expected));

        }
    }
}
