using System.Collections.Immutable;

namespace DanceCompetitionHelper.OrgImpl.Oetsv
{
    public static class OetsvConstants
    {
        public static class Disciplines
        {
            public const string Sta = "Sta";
            public const string La = "La";
            public const string Combination = "Kombi";
            public const string Freestyle = "Kuer";

            public static string? ToDisciplines(
                string? useString)
            {
                switch (useString)
                {
                    case Sta:
                    case "sta":
                    case "STA":
                    case "Standard":
                        return Sta;

                    case La:
                    case "la":
                    case "LA":
                    case "Latin":
                        return La;

                    case Combination:
                    case "Combination":
                    case "KOMBI":
                    case "Kombination":
                    case "combi":
                    case "COMBI":
                        return Combination;

                    case Freestyle:
                    case "Freestyle":
                    case "Kür":
                    case "KÜR":
                    case "kuer":
                    case "KUER":
                        return Freestyle;
                }

                return useString;
            }
        }

        public static class AgeClasses
        {
            public const string Pupil = "Sch";
            public const string Junior = "Jun";
            public const string Juvenile = "Jug";
            public const string Adult = "Allg";
            public const string Senior = "Sen";
            public const string Formation = "For";

            public static string? ToAgeClasses(
                string? useString)
            {
                switch (useString)
                {
                    case Pupil:
                    case "SCH":
                    case "Schüler":
                    case "Schueler":
                        return Pupil;

                    case Junior:
                    case "Junior":
                    case "JUN":
                        return Junior;

                    case Juvenile:
                    case "Juvenile":
                    case "Juv":
                    case "JUV":
                    case "JUG":
                        return Juvenile;

                    case Adult:
                    case "Adult":
                    case "Adlt":
                    case "ADLT":
                    case "Adt":
                    case "ADT":
                        return Adult;

                    case Senior:
                    case "Senior":
                    case "sen":
                    case "SEN":
                        return Senior;

                    case Formation:
                    case "FOR":
                    case "FORM":
                    case "Form":
                    case "Formation":
                    case "FORMATION":
                        return Formation;
                }

                return useString;
            }
        }

        public static class Classes
        {
            const string Amateur = "Bsp";
            const string D = "D";
            const string C = "C";
            const string B = "B";
            const string A = "A";
            const string S = "S";

            public static string? ToClasses(
                string? useString)
            {
                switch (useString)
                {
                    case Amateur:
                    case "BSP":
                        return Amateur;

                    case D:
                    case "d":
                        return D;

                    case C:
                    case "c":
                        return C;

                    case B:
                    case "b":
                        return B;

                    case A:
                    case "a":
                        return A;

                    case S:
                    case "s":
                        return S;
                }

                return useString;
            }

            public static string? GetHigherClassifications(
                string? forClass)
            {
                switch (ToClasses(forClass))
                {
                    case D:
                        return C;

                    case C:
                        return B;

                    case B:
                        return A;

                    case S:
                        return S;
                }

                return null;
            }
        }

        public static class Points
        {
            public static readonly IReadOnlyList<IReadOnlySet<string>> SameAgeClassForPoints = new List<IReadOnlySet<string>>()
                {
                    new HashSet<string>()
                    {
                        AgeClasses.Pupil,
                        AgeClasses.Junior,
                        AgeClasses.Juvenile,
                        AgeClasses.Adult,
                    }.ToImmutableHashSet(),
                    new HashSet<string>()
                    {
                        AgeClasses.Senior,
                    }.ToImmutableHashSet(),
                    new HashSet<string>()
                    {
                        AgeClasses.Formation,
                    }.ToImmutableHashSet(),
                }.AsReadOnly();

            public static readonly IReadOnlyDictionary<string, IReadOnlySet<string>> SameAgeClassForPointsByAgeClass;

            static Points()
            {
                var toSave = new Dictionary<string, IReadOnlySet<string>>();

                foreach (var curSameAge in SameAgeClassForPoints)
                {
                    foreach (var curVal in curSameAge)
                    {
                        toSave[curVal] = curSameAge;
                    }
                }

                SameAgeClassForPointsByAgeClass = toSave.ToImmutableDictionary();
            }

            public static bool AgeClassForSamePoints(
                string? ageClassBase,
                string? ageClassCompare)
            {
                var useAgeClassBase = AgeClasses.ToAgeClasses(
                    ageClassBase);
                var useAgeClassCompare = AgeClasses.ToAgeClasses(
                    ageClassCompare);

                if (string.IsNullOrEmpty(useAgeClassBase)
                    || string.IsNullOrEmpty(useAgeClassCompare))
                {
                    return false;
                }

                if (SameAgeClassForPointsByAgeClass.TryGetValue(
                    useAgeClassBase,
                    out var chkClasses) == false)
                {
                    return false;
                }

                return chkClasses.Contains(
                    useAgeClassCompare);
            }
        }
    }
}
