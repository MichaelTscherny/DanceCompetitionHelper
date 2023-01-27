using static DanceCompetitionHelper.OrgImpl.Oetsv.OetsvConstants;

namespace DanceCompetitionHelper.Test.Tests.UnitTests.OrgImpl.Oetsv.OetsvConstants
{
    [TestFixture]
    public class OetsvConstantsTests
    {
        [Test]
        // -------------
        [TestCase(Disciplines.Sta, Disciplines.Sta)]
        [TestCase("sta", Disciplines.Sta)]
        [TestCase("STA", Disciplines.Sta)]
        [TestCase("Standard", Disciplines.Sta)]
        // -------------
        [TestCase(Disciplines.La, Disciplines.La)]
        [TestCase("la", Disciplines.La)]
        [TestCase("LA", Disciplines.La)]
        [TestCase("Latin", Disciplines.La)]
        // -------------
        [TestCase(Disciplines.Combination, Disciplines.Combination)]
        [TestCase("Combination", Disciplines.Combination)]
        [TestCase("KOMBI", Disciplines.Combination)]
        [TestCase("Kombination", Disciplines.Combination)]
        [TestCase("combi", Disciplines.Combination)]
        [TestCase("Combi", Disciplines.Combination)]
        [TestCase("COMBI", Disciplines.Combination)]
        // -------------
        [TestCase(Disciplines.Freestyle, Disciplines.Freestyle)]
        [TestCase("Freestyle", Disciplines.Freestyle)]
        [TestCase("Kür", Disciplines.Freestyle)]
        [TestCase("KÜR", Disciplines.Freestyle)]
        [TestCase("kuer", Disciplines.Freestyle)]
        [TestCase("KUER", Disciplines.Freestyle)]
        // -------------
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("dummy", "dummy")]
        public void ToDisciplines_Test(
            string input,
            string expected)
        {
            Assert.That(
                Disciplines.ToDisciplines(
                    input),
                Is.EqualTo(
                    expected));

        }
    }
}