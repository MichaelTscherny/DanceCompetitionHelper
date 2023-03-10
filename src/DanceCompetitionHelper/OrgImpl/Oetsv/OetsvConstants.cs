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

        /// <summary>
        /// See "5otsvto2023.pdf" (ÖTSV TURNIERORDNUNG): Zu § 8, Pkt 2, JUNIOREN, page 28
        /// </summary>
        public static class AgeClasses
        {
            public const string Juvenile = "Sch";
            public const string Junior = "Jun";
            public const string Youth = "Jug";
            public const string Adult = "Allg";
            public const string Senior = "Sen";
            public const string Formation = "For";

            public static string? ToAgeClasses(
                string? useString)
            {
                switch (useString)
                {
                    case Juvenile:
                    case "SCH":
                    case "Schüler":
                    case "schüler":
                    case "SCHÜLER":
                    case "Schueler":
                    case "SCHUELER":
                    case "schueler":
                    case "Juvenile":
                    case "Juv":
                    case "JUV":
                        return Juvenile;

                    case Junior:
                    case "Junior":
                    case "JUNIOR":
                    case "junior":
                    case "JUN":
                        return Junior;

                    case Youth:
                    case "JUG":
                    case "Youth":
                    case "youth":
                    case "YOUTH":
                    case "YOU":
                    case "You":
                    case "you":
                        return Youth;

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

        public static class AgeGroups
        {
            public const string GroupNone = null;
            public const string Group1 = "1";
            public const string Group2 = "2";
            public const string Group3 = "3";
            public const string Group4 = "4";

            public static string? ToAgeGroup(
                string? useString)
            {
                switch (useString?.Trim())
                {
                    case GroupNone:
                    case "0":
                    case "":
                    case "-":
                        return GroupNone;

                    case Group1:
                    case "I":
                    case "i":
                        return Group1;

                    case Group2:
                    case "II":
                    case "ii":
                        return Group2;

                    case Group3:
                    case "III":
                    case "iii":
                        return Group3;

                    case Group4:
                    case "IV":
                    case "iv":
                        return Group4;
                }

                return null;
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

            /// <summary>
            /// See "5otsvto2023.pdf" (ÖTSV TURNIERORDNUNG): 
            /// * § 9 - STARTKLASSEN, page 33
            /// * § 10 - STARTKLASSENÄNDERUNG, page 35
            /// </summary>
            /// <param name="forAgeClass"></param>
            /// <param name="forClass"></param>
            /// <returns></returns>
            public static string? GetHigherClassifications(
                string? forDiscepline,
                string? forAgeClass,
                string? forAgeGroup,
                string? forClass)
            {
                var useDiscepline = Disciplines.ToDisciplines(forDiscepline);
                var useAgeClasses = AgeClasses.ToAgeClasses(forAgeClass);
                var useAgeGroup = AgeGroups.ToAgeGroup(forAgeGroup);
                var useClasses = ToClasses(forClass);

                switch (useAgeClasses)
                {
                    case AgeClasses.Juvenile:
                        switch (useClasses)
                        {
                            case D:
                                return C;

                        }
                        break;

                    case AgeClasses.Junior:
                        switch (useClasses)
                        {
                            case D:
                                return C;

                            case C:
                                return B;
                        }
                        break;

                    case AgeClasses.Youth:
                        switch (useClasses)
                        {
                            case D:
                                return C;

                            case C:
                                return B;

                            case B:
                                return A;
                        }
                        break;


                    case AgeClasses.Adult:
                        switch (useClasses)
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
                        break;

                    case AgeClasses.Senior:
                        switch (useDiscepline)
                        {
                            case Disciplines.Sta:
                                switch (useClasses)
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
                                break;

                            case Disciplines.La:
                                switch (useAgeGroup)
                                {
                                    case AgeGroups.Group1:
                                    case AgeGroups.Group2:
                                    case AgeGroups.Group3:
                                        switch (useClasses)
                                        {
                                            case D:
                                                return C;

                                            case C:
                                                return B;

                                            case B:
                                                return S;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
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
                    case AgeClasses.Juvenile:
                    case AgeClasses.Junior:
                    case AgeClasses.Youth:
                    case AgeClasses.Adult:
                        switch (useAgeClassCompare)
                        {
                            case AgeClasses.Juvenile:
                            case AgeClasses.Junior:
                            case AgeClasses.Youth:
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
