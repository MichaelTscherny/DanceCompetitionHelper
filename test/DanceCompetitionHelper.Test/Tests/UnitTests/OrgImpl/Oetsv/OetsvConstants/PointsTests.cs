﻿using static DanceCompetitionHelper.OrgImpl.Oetsv.OetsvConstants;

namespace DanceCompetitionHelper.Test.Tests.UnitTests.OrgImpl.Oetsv.OetsvConstants
{
    [TestFixture]
    public class PointsTests
    {
        [Test]
        // -------------
        [TestCase(AgeClasses.Pupil, AgeClasses.Pupil, true)]
        [TestCase(AgeClasses.Junior, AgeClasses.Junior, true)]
        [TestCase(AgeClasses.Juvenile, AgeClasses.Juvenile, true)]
        [TestCase(AgeClasses.Adult, AgeClasses.Adult, true)]
        [TestCase(AgeClasses.Senior, AgeClasses.Senior, true)]
        [TestCase(AgeClasses.Formation, AgeClasses.Formation, true)]
        // -------------
        [TestCase(AgeClasses.Adult, AgeClasses.Senior, false)]
        [TestCase(AgeClasses.Senior, AgeClasses.Adult, false)]
        // -------------
        public void AgeClassForSamePoints_Test(
            string ageClassBase,
            string ageClassCompare,
            bool expected)
        {
            Assert.That(
                Points.AgeClassForSamePoints(
                    ageClassBase,
                    ageClassCompare),
                Is.EqualTo(
                    expected));

        }
    }
}
