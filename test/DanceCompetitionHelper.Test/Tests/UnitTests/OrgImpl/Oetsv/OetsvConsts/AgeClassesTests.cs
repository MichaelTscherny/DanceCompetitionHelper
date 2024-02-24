using static DanceCompetitionHelper.OrgImpl.Oetsv.OetsvConstants;

namespace DanceCompetitionHelper.Test.Tests.UnitTests.OrgImpl.Oetsv.OetsvConsts
{
    [TestFixture]
    public class AgeClassesTests
    {
        [Test]
        // -------------
        [TestCase(AgeClasses.Juvenile, AgeClasses.Juvenile)]
        [TestCase("SCH", AgeClasses.Juvenile)]
        [TestCase("Schüler", AgeClasses.Juvenile)]
        [TestCase("schüler", AgeClasses.Juvenile)]
        [TestCase("SCHÜLER", AgeClasses.Juvenile)]
        [TestCase("Schueler", AgeClasses.Juvenile)]
        [TestCase("SCHUELER", AgeClasses.Juvenile)]
        [TestCase("schueler", AgeClasses.Juvenile)]
        // -------------
        [TestCase(AgeClasses.Junior, AgeClasses.Junior)]
        [TestCase("Junior", AgeClasses.Junior)]
        [TestCase("JUNIOR", AgeClasses.Junior)]
        [TestCase("junior", AgeClasses.Junior)]
        [TestCase("JUN", AgeClasses.Junior)]
        // -------------
        [TestCase(AgeClasses.Youth, AgeClasses.Youth)]
        [TestCase("JUG", AgeClasses.Youth)]
        [TestCase("Youth", AgeClasses.Youth)]
        [TestCase("youth", AgeClasses.Youth)]
        [TestCase("YOUTH", AgeClasses.Youth)]
        [TestCase("YOU", AgeClasses.Youth)]
        [TestCase("You", AgeClasses.Youth)]
        [TestCase("you", AgeClasses.Youth)]
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
        [TestCase(AgeClasses.Open, AgeClasses.Open)]
        [TestCase("open", AgeClasses.Open)]
        // -------------
        [TestCase(AgeClasses.Under16, AgeClasses.Under16)]
        [TestCase("u16", AgeClasses.Under16)]
        // -------------
        [TestCase(null, null)]
        [TestCase("", null)]
        [TestCase("dummy", null)]
        public void ToAgeClasses_Test(
            string? input,
            string? expected)
        {
            Assert.That(
                AgeClasses.ToAgeClasses(
                    input),
                Is.EqualTo(
                    expected));

        }
    }
}
