namespace DanceCompetitionHelper.OrgImpl.Oetsv.WorkData
{
    public class CompetitionClassImport : IEquatable<CompetitionClassImport>
    {
        public string? OrgClassIdRaw { get; set; }
        public string? OrgClassId { get; set; }

        public string? DisciplineRaw { get; set; }
        public string? Discipline { get; set; }

        public string? ClassRaw { get; set; }
        public string? Class { get; set; }

        public string? AgeClassRaw { get; set; }
        public string? AgeClass { get; set; }

        public string? AgeGroupRaw { get; set; }
        public string? AgeGroup { get; set; }

        public string? NameRaw { get; set; }
        public string? Name { get; set; }

        public override string ToString()
        {
            return string.Format(
                "{0} ({1}/{2}/{3}/{4}/{5} = {6}/{7}/{8}/{9}/{10})",
                Name,
                OrgClassId,
                AgeClass,
                AgeGroup,
                Discipline,
                Class,
                OrgClassIdRaw,
                AgeClassRaw,
                AgeGroupRaw,
                DisciplineRaw,
                ClassRaw);
        }

        public override bool Equals(object? obj)
        {
            return obj is CompetitionClassImport import &&
                   OrgClassIdRaw == import.OrgClassIdRaw &&
                   OrgClassId == import.OrgClassId &&
                   DisciplineRaw == import.DisciplineRaw &&
                   Discipline == import.Discipline &&
                   ClassRaw == import.ClassRaw &&
                   Class == import.Class &&
                   AgeClassRaw == import.AgeClassRaw &&
                   AgeClass == import.AgeClass &&
                   AgeGroupRaw == import.AgeGroupRaw &&
                   AgeGroup == import.AgeGroup &&
                   NameRaw == import.NameRaw &&
                   Name == import.Name;
        }

        public bool Equals(CompetitionClassImport? other)
        {
            return Equals(
                (object?)other);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(OrgClassIdRaw);
            hash.Add(OrgClassId);
            hash.Add(DisciplineRaw);
            hash.Add(Discipline);
            hash.Add(ClassRaw);
            hash.Add(Class);
            hash.Add(AgeClassRaw);
            hash.Add(AgeClass);
            hash.Add(AgeGroupRaw);
            hash.Add(AgeGroup);
            hash.Add(NameRaw);
            hash.Add(Name);
            return hash.ToHashCode();
        }
    }
}
