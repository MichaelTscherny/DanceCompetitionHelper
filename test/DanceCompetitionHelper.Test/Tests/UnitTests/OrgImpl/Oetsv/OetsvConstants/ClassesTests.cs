using static DanceCompetitionHelper.OrgImpl.Oetsv.OetsvConstants;

namespace DanceCompetitionHelper.Test.Tests.UnitTests.OrgImpl.Oetsv.OetsvConstants
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
        [TestCase("", "")]
        [TestCase("dummy", "dummy")]
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
        [TestCase(Classes.Amateur, null)]
        [TestCase(Classes.D, Classes.C)]
        [TestCase(Classes.C, Classes.B)]
        [TestCase(Classes.B, Classes.A)]
        // -------------
        [TestCase(null, null)]
        [TestCase("", null)]
        [TestCase("dummy", null)]
        public void GetHigherClassifications_Test(
            string input,
            string expected)
        {
            Assert.That(
                Classes.GetHigherClassifications(
                    input),
                Is.EqualTo(
                    expected));

        }
    }
}
