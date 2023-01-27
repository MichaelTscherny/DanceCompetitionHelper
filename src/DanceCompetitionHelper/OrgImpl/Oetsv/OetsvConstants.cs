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
                    case "Combi":
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
                    case "schüler":
                    case "SCHÜLER":
                    case "Schueler":
                    case "SCHUELER":
                    case "schueler":
                        return Pupil;

                    case Junior:
                    case "Junior":
                    case "JUNIOR":
                    case "junior":
                    case "JUN":
                        return Junior;

                    case Juvenile:
                    case "Juvenile":
                    case "Juv":
                    case "JUV":
                    case "JUG":
                        return Juvenile;

                    case Adult:
                    case "ALLG":
                    case "Adult":
                    case "ADULT":
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
                    case "form":
                    case "Form":
                    case "Formation":
                    case "formation":
                    case "FORMATION":
                        return Formation;
                }

                return useString;
            }
        }

        public static class Classes
        {
            public const string Amateur = "Bsp";
            public const string D = "D";
            public const string C = "C";
            public const string B = "B";
            public const string A = "A";
            public const string S = "S";

            public static string? ToClasses(
                string? useString)
            {
                switch (useString)
                {
                    case Amateur:
                    case "bsp":
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

                    case A:
                        return S;
                }

                return null;
            }
        }

        public static class Points
        {
            public static bool AgeClassForSamePoints(
                string? ageClassBase,
                string? ageClassCompare)
            {
                var useAgeClassBase = AgeClasses.ToAgeClasses(
                    ageClassBase);
                var useAgeClassCompare = AgeClasses.ToAgeClasses(
                    ageClassCompare);

                switch (useAgeClassBase)
                {
                    case AgeClasses.Pupil:
                    case AgeClasses.Junior:
                    case AgeClasses.Juvenile:
                    case AgeClasses.Adult:
                        switch (useAgeClassCompare)
                        {
                            case AgeClasses.Pupil:
                            case AgeClasses.Junior:
                            case AgeClasses.Juvenile:
                            case AgeClasses.Adult:
                                return true;
                        }
                        break;

                    case AgeClasses.Senior:
                        switch (useAgeClassCompare)
                        {
                            case AgeClasses.Senior:
                                return true;
                        }
                        break;

                    case AgeClasses.Formation:
                        switch (useAgeClassCompare)
                        {
                            case AgeClasses.Formation:
                                return true;
                        }
                        break;
                }

                return false;
            }
        }
    }
}
