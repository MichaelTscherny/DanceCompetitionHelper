namespace DanceCompetitionHelper.OrgImpl.Oetsv.WorkData
{
    public class CompetitionParticipantImport : IEquatable<CompetitionParticipantImport>
    {
        public string? RegOrgClassIdRaw { get; set; }
        public string? RegOrgClassId { get; set; }

        public string? RegStartNumberRaw { get; set; }
        public int? RegStartNumber { get; set; }

        public string? RegPartAFirstNameRaw { get; set; }
        public string? RegPartALastNameRaw { get; set; }
        public string? RegPartAName { get; set; }

        public string? RegPartAOrgIdRaw { get; set; }
        public string? RegPartAOrgId { get; set; }

        public string? RegPartBFirstNameRaw { get; set; }
        public string? RegPartBLastNameRaw { get; set; }
        public string? RegPartBName { get; set; }

        public string? RegPartBOrgIdRaw { get; set; }
        public string? RegPartBOrgId { get; set; }

        public string? RegClubNameRaw { get; set; }
        public string? RegClubName { get; set; }

        public string? RegClubOrgIdRaw { get; set; }
        public string? RegClubOrgId { get; set; }

        public string? RegStateRaw { get; set; }
        public string? RegState { get; set; }

        public string? RegStateAbbrRaw { get; set; }
        public string? RegStateAbbr { get; set; }

        public string? RegDisciplineRaw { get; set; }
        public string? RegDiscipline { get; set; }

        public string? RegClassRaw { get; set; }
        public string? RegClass { get; set; }

        public string? OrgCurrentClassRaw { get; set; }
        public string? OrgCurrentClass { get; set; }

        public string? RegAgeClassRaw { get; set; }
        public string? RegAgeClass { get; set; }

        public string? RegAgeGroupRaw { get; set; }
        public string? RegAgeGroup { get; set; }

        public string? OrgPointsRaw { get; set; }
        public double? OrgPoints { get; set; }

        public string? OrgStartsRaw { get; set; }
        public int? OrgStarts { get; set; }

        public string? OrgMinPointsForPromotionRaw { get; set; }
        public double? OrgMinPointsForPromotion { get; set; }

        public string? OrgMinStartsForPromotionRaw { get; set; }
        public int? OrgMinStartsForPromotion { get; set; }

        public override string ToString()
        {
            return string.Format(
                "{0}/{1} ({2}/{3}/{4}/{5}/{6} - {7}/{8} - {9}/{10} - Prom: {11}/{12})",
                RegPartAName,
                RegPartBName,
                RegOrgClassId,
                RegAgeClass,
                RegAgeGroup,
                RegDiscipline,
                RegClass,
                RegClubName,
                RegState,
                OrgPoints,
                OrgStarts,
                OrgMinPointsForPromotion,
                OrgMinStartsForPromotion);
        }

        public override bool Equals(object? obj)
        {
            return obj is CompetitionParticipantImport import &&
                   RegOrgClassIdRaw == import.RegOrgClassIdRaw &&
                   RegOrgClassId == import.RegOrgClassId &&
                   RegStartNumberRaw == import.RegStartNumberRaw &&
                   RegStartNumber == import.RegStartNumber &&
                   RegPartAFirstNameRaw == import.RegPartAFirstNameRaw &&
                   RegPartALastNameRaw == import.RegPartALastNameRaw &&
                   RegPartAName == import.RegPartAName &&
                   RegPartAOrgIdRaw == import.RegPartAOrgIdRaw &&
                   RegPartAOrgId == import.RegPartAOrgId &&
                   RegPartBFirstNameRaw == import.RegPartBFirstNameRaw &&
                   RegPartBLastNameRaw == import.RegPartBLastNameRaw &&
                   RegPartBName == import.RegPartBName &&
                   RegPartBOrgIdRaw == import.RegPartBOrgIdRaw &&
                   RegPartBOrgId == import.RegPartBOrgId &&
                   RegClubNameRaw == import.RegClubNameRaw &&
                   RegClubName == import.RegClubName &&
                   RegClubOrgIdRaw == import.RegClubOrgIdRaw &&
                   RegClubOrgId == import.RegClubOrgId &&
                   RegStateRaw == import.RegStateRaw &&
                   RegState == import.RegState &&
                   RegStateAbbrRaw == import.RegStateAbbrRaw &&
                   RegStateAbbr == import.RegStateAbbr &&
                   RegDisciplineRaw == import.RegDisciplineRaw &&
                   RegDiscipline == import.RegDiscipline &&
                   RegClassRaw == import.RegClassRaw &&
                   RegClass == import.RegClass &&
                   OrgCurrentClassRaw == import.OrgCurrentClassRaw &&
                   OrgCurrentClass == import.OrgCurrentClass &&
                   RegAgeClassRaw == import.RegAgeClassRaw &&
                   RegAgeClass == import.RegAgeClass &&
                   RegAgeGroupRaw == import.RegAgeGroupRaw &&
                   RegAgeGroup == import.RegAgeGroup &&
                   OrgPointsRaw == import.OrgPointsRaw &&
                   OrgPoints == import.OrgPoints &&
                   OrgStartsRaw == import.OrgStartsRaw &&
                   OrgStarts == import.OrgStarts &&
                   OrgMinPointsForPromotionRaw == import.OrgMinPointsForPromotionRaw &&
                   OrgMinPointsForPromotion == import.OrgMinPointsForPromotion &&
                   OrgMinStartsForPromotionRaw == import.OrgMinStartsForPromotionRaw &&
                   OrgMinStartsForPromotion == import.OrgMinStartsForPromotion;
        }

        public bool Equals(CompetitionParticipantImport? other)
        {
            return Equals(
                (object?)other);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(RegOrgClassIdRaw);
            hash.Add(RegOrgClassId);
            hash.Add(RegStartNumberRaw);
            hash.Add(RegStartNumber);
            hash.Add(RegPartAFirstNameRaw);
            hash.Add(RegPartALastNameRaw);
            hash.Add(RegPartAName);
            hash.Add(RegPartAOrgIdRaw);
            hash.Add(RegPartBFirstNameRaw);
            hash.Add(RegPartBLastNameRaw);
            hash.Add(RegPartBName);
            hash.Add(RegPartBOrgIdRaw);
            hash.Add(RegClubNameRaw);
            hash.Add(RegClubName);
            hash.Add(RegClubOrgIdRaw);
            hash.Add(RegClubOrgId);
            hash.Add(RegStateRaw);
            hash.Add(RegState);
            hash.Add(RegStateAbbrRaw);
            hash.Add(RegStateAbbr);
            hash.Add(RegDisciplineRaw);
            hash.Add(RegDiscipline);
            hash.Add(RegClassRaw);
            hash.Add(RegClass);
            hash.Add(OrgCurrentClassRaw);
            hash.Add(OrgCurrentClass);
            hash.Add(RegAgeClassRaw);
            hash.Add(RegAgeClass);
            hash.Add(RegAgeGroupRaw);
            hash.Add(RegAgeGroup);
            hash.Add(OrgPointsRaw);
            hash.Add(OrgPoints);
            hash.Add(OrgStartsRaw);
            hash.Add(OrgStarts);
            hash.Add(OrgMinPointsForPromotionRaw);
            hash.Add(OrgMinPointsForPromotion);
            hash.Add(OrgMinStartsForPromotionRaw);
            hash.Add(OrgMinStartsForPromotion);
            return hash.ToHashCode();
        }
    }
}
