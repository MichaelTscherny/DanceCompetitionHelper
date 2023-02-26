namespace DanceCompetitionHelper.OrgImpl.Oetsv.WorkData
{
    public class CompetitionParticipantImport : IEquatable<CompetitionParticipantImport>
    {
        public string? OrgClassIdRaw { get; set; }
        public string? OrgClassId { get; set; }

        public string? StartNumberRaw { get; set; }
        public int? StartNumber { get; set; }

        public string? Part01FirstNameRaw { get; set; }
        public string? Part01LastNameRaw { get; set; }
        public string? Part01Name { get; set; }

        public string? Part01OrgIdRaw { get; set; }
        public string? Part01OrgId { get; set; }

        public string? Part02FirstNameRaw { get; set; }
        public string? Part02LastNameRaw { get; set; }
        public string? Part02Name { get; set; }

        public string? Part02OrgIdRaw { get; set; }
        public string? Part02OrgId { get; set; }

        public string? ClubNameRaw { get; set; }
        public string? ClubName { get; set; }

        public string? ClubOrgIdRaw { get; set; }
        public string? ClubOrgId { get; set; }

        public string? StateRaw { get; set; }
        public string? State { get; set; }

        public string? StateAbbrRaw { get; set; }
        public string? StateAbbr { get; set; }

        public string? DisciplineRaw { get; set; }
        public string? Discipline { get; set; }

        public string? ClassRaw { get; set; }
        public string? Class { get; set; }

        public string? AgeClassRaw { get; set; }
        public string? AgeClass { get; set; }

        public string? AgeGroupRaw { get; set; }
        public string? AgeGroup { get; set; }

        public string? OrgPointsRaw { get; set; }
        public int? OrgPoints { get; set; }

        public string? OrgStartsRaw { get; set; }
        public int? OrgStarts { get; set; }

        public string? MinPointsForPromotionRaw { get; set; }
        public int? MinPointsForPromotion { get; set; }

        public string? MinStartsForPromotionRaw { get; set; }
        public int? MinStartsForPromotion { get; set; }

        public override string ToString()
        {
            return string.Format(
                "{0}/{1} ({2}/{3}/{4}/{5}/{6} - {7}/{8} - {9}/{10} - Prom: {11}/{12})",
                Part01Name,
                Part02Name,
                OrgClassId,
                AgeClass,
                AgeGroup,
                Discipline,
                Class,
                ClubName,
                State,
                OrgPoints,
                OrgStarts,
                MinPointsForPromotion,
                MinStartsForPromotion);
        }

        public override bool Equals(object? obj)
        {
            return obj is CompetitionParticipantImport import &&
                   OrgClassIdRaw == import.OrgClassIdRaw &&
                   OrgClassId == import.OrgClassId &&
                   StartNumberRaw == import.StartNumberRaw &&
                   StartNumber == import.StartNumber &&
                   Part01FirstNameRaw == import.Part01FirstNameRaw &&
                   Part01LastNameRaw == import.Part01LastNameRaw &&
                   Part01Name == import.Part01Name &&
                   Part02FirstNameRaw == import.Part02FirstNameRaw &&
                   Part02LastNameRaw == import.Part02LastNameRaw &&
                   Part02Name == import.Part02Name &&
                   ClubNameRaw == import.ClubNameRaw &&
                   ClubName == import.ClubName &&
                   ClubOrgIdRaw == import.ClubOrgIdRaw &&
                   ClubOrgId == import.ClubOrgId &&
                   StateRaw == import.StateRaw &&
                   State == import.State &&
                   StateAbbrRaw == import.StateAbbrRaw &&
                   StateAbbr == import.StateAbbr &&
                   DisciplineRaw == import.DisciplineRaw &&
                   Discipline == import.Discipline &&
                   ClassRaw == import.ClassRaw &&
                   Class == import.Class &&
                   AgeClassRaw == import.AgeClassRaw &&
                   AgeClass == import.AgeClass &&
                   AgeGroupRaw == import.AgeGroupRaw &&
                   AgeGroup == import.AgeGroup &&
                   OrgPointsRaw == import.OrgPointsRaw &&
                   OrgPoints == import.OrgPoints &&
                   OrgStartsRaw == import.OrgStartsRaw &&
                   OrgStarts == import.OrgStarts &&
                   MinPointsForPromotionRaw == import.MinPointsForPromotionRaw &&
                   MinPointsForPromotion == import.MinPointsForPromotion &&
                   MinStartsForPromotionRaw == import.MinStartsForPromotionRaw &&
                   MinStartsForPromotion == import.MinStartsForPromotion;
        }

        public bool Equals(CompetitionParticipantImport? other)
        {
            return Equals(
                (object?)other);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(OrgClassIdRaw);
            hash.Add(OrgClassId);
            hash.Add(StartNumberRaw);
            hash.Add(StartNumber);
            hash.Add(Part01FirstNameRaw);
            hash.Add(Part01LastNameRaw);
            hash.Add(Part01Name);
            hash.Add(Part02FirstNameRaw);
            hash.Add(Part02LastNameRaw);
            hash.Add(Part02Name);
            hash.Add(ClubNameRaw);
            hash.Add(ClubName);
            hash.Add(ClubOrgIdRaw);
            hash.Add(ClubOrgId);
            hash.Add(StateRaw);
            hash.Add(State);
            hash.Add(StateAbbrRaw);
            hash.Add(StateAbbr);
            hash.Add(DisciplineRaw);
            hash.Add(Discipline);
            hash.Add(ClassRaw);
            hash.Add(Class);
            hash.Add(AgeClassRaw);
            hash.Add(AgeClass);
            hash.Add(AgeGroupRaw);
            hash.Add(AgeGroup);
            hash.Add(OrgPointsRaw);
            hash.Add(OrgPoints);
            hash.Add(OrgStartsRaw);
            hash.Add(OrgStarts);
            hash.Add(MinPointsForPromotionRaw);
            hash.Add(MinPointsForPromotion);
            hash.Add(MinStartsForPromotionRaw);
            hash.Add(MinStartsForPromotion);
            return hash.ToHashCode();
        }
    }
}
