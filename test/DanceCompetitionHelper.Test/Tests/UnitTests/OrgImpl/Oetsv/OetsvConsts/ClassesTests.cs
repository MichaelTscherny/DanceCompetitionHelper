using static DanceCompetitionHelper.OrgImpl.Oetsv.OetsvConstants;

namespace DanceCompetitionHelper.Test.Tests.UnitTests.OrgImpl.Oetsv.OetsvConsts
{
    [TestFixture]
    public class ClassesTests
    {
        [Test]
        // -------------
        [TestCase(Classes.Amateur, Classes.Amateur)]
        [TestCase("bsp", Classes.Amateur)]
        [TestCase("BSP", Classes.Amateur)]
        // -------------
        [TestCase(Classes.D, Classes.D)]
        [TestCase("d", Classes.D)]
        // -------------
        [TestCase(Classes.C, Classes.C)]
        [TestCase("c", Classes.C)]
        // -------------
        [TestCase(Classes.B, Classes.B)]
        [TestCase("b", Classes.B)]
        // -------------
        [TestCase(Classes.A, Classes.A)]
        [TestCase("A", Classes.A)]
        // -------------
        [TestCase(Classes.S, Classes.S)]
        [TestCase("S", Classes.S)]
        // -------------
        [TestCase(null, null)]
        [TestCase("", null)]
        [TestCase("dummy", null)]
        public void ToClasses_Test(
            string input,
            string expected)
        {
            Assert.That(
                Classes.ToClasses(
                    input),
                Is.EqualTo(
                    expected));

        }

        [Test]
        // -------------
        [TestCase(Disciplines.Sta, AgeClasses.Adult, AgeGroups.GroupNone, Classes.Amateur, null)]
        [TestCase(Disciplines.Sta, AgeClasses.Adult, AgeGroups.GroupNone, Classes.D, Classes.C)]
        [TestCase(Disciplines.Sta, AgeClasses.Adult, AgeGroups.GroupNone, Classes.C, Classes.B)]
        [TestCase(Disciplines.Sta, AgeClasses.Adult, AgeGroups.GroupNone, Classes.B, Classes.A)]
        // -------------
        [TestCase(Disciplines.La, AgeClasses.Senior, AgeGroups.Group1, Classes.D, Classes.C)]
        [TestCase(Disciplines.La, AgeClasses.Senior, AgeGroups.Group1, Classes.C, Classes.B)]
        [TestCase(Disciplines.La, AgeClasses.Senior, AgeGroups.Group1, Classes.B, Classes.S)]
        [TestCase(Disciplines.La, AgeClasses.Senior, AgeGroups.Group1, Classes.A, null)]
        // -------------
        [TestCase(Disciplines.La, AgeClasses.Senior, AgeGroups.Group4, Classes.D, null)]
        [TestCase(Disciplines.La, AgeClasses.Senior, AgeGroups.Group4, Classes.C, null)]
        [TestCase(Disciplines.La, AgeClasses.Senior, AgeGroups.Group4, Classes.B, null)]
        [TestCase(Disciplines.La, AgeClasses.Senior, AgeGroups.Group4, Classes.A, null)]
        [TestCase(Disciplines.La, AgeClasses.Senior, AgeGroups.Group4, Classes.S, null)]
        // -------------
        [TestCase(null, null, null, null, null)]
        [TestCase("", "", "", "", null)]
        [TestCase("dummy", "dummy", "dummy", "dummy", null)]
        public void GetHigherClassifications_Test(
            string forDiscepline,
            string forAgeClass,
            string forAgeGroup,
            string forClass,
            string expected)
        {
            Assert.That(
                Classes.GetHigherClassifications(
                    forDiscepline,
                    forAgeClass,
                    forAgeGroup,
                    forClass),
                Is.EqualTo(
                    expected));

        }
    }
}
