namespace DanceCompetitionHelper.OrgImpl.Oetsv
{
    public static class OetsvConstants
    {
        public static class CompetitionType
        {
            public const string Bewertungsturnier = "Bewertungsturnier";
            public const string Landesmeisterschaft = "Landesmeisterschaft";
            public const string Staatsmeisterschaft = "Staatsmeisterschaft";
            public const string OesterrMeisterschaft = "Österr. Meisterschaft";

            public static string? ToCompetitionType(
                string? useString)
            {
                switch (useString?.Trim())
                {
                    case Bewertungsturnier:
                    case "BEWERTUNGSTURNIER":
                    case "bewertungsturnier":
                    case "BW":
                    case "Bw":
                    case "bw":
                        return Bewertungsturnier;

                    case Landesmeisterschaft:
                    case "LANDESMEISTERSCHAFT":
                    case "landesmeisterschaft":
                    case "LM":
                    case "Lm":
                    case "lm":
                        return Landesmeisterschaft;

                    case Staatsmeisterschaft:
                    case "STAATSMEISTERSCHAFT":
                    case "staatsmeisterschaft":
                    case "STAATS":
                    case "Staats":
                        return Staatsmeisterschaft;

                    case OesterrMeisterschaft:
                    case "ÖSTERR. MEISTERSCHAFT":
                    case "OESTERR. MEISTERSCHAFT":
                    case "österr. meisterschaft":
                    case "oesterr. meisterschaft":
                    case "ÖM":
                    case "OEM":
                    case "OeM":
                    case "öm":
                    case "oem":
                        return OesterrMeisterschaft;
                }

                return null;
            }

            /// <summary>
            /// See "5otsvto2023.pdf" (ÖTSV TURNIERORDNUNG): 
            /// * § 10 - STARTKLASSENÄNDERUNG, page 35
            /// </summary>
            /// <param name="useCompetitionType"></param>
            /// <returns></returns>
            public static double GetPointsForWinning(
                string? useCompetitionType)
            {
                switch (CompetitionType.ToCompetitionType(
                    useCompetitionType))
                {
                    case CompetitionType.Landesmeisterschaft:
                        return 150;

                    case CompetitionType.Staatsmeisterschaft:
                    case CompetitionType.OesterrMeisterschaft:
                        return 200;
                }

                return 100;
            }
        }


        public static class Disciplines
        {
            public const string Sta = "Sta";
            public const string La = "La";
            public const string Combination = "Kombi";
            public const string Freestyle = "Kuer";

            public static string? ToDisciplines(
                string? useString)
            {
                switch (useString?.Trim())
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
                    case "CO":
                    case "Co":
                    case "co":
                    case "KO":
                    case "Ko":
                    case "ko":
                        return Combination;

                    case Freestyle:
                    case "Freestyle":
                    case "Kür":
                    case "KÜR":
                    case "kuer":
                    case "KUER":
                        return Freestyle;
                }

                return null;
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
            public const string Open = "Offen";

            public static string? ToAgeClasses(
                string? useString)
            {
                switch (useString?.Trim())
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
                    case "ALG":
                    case "Alg":
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

                    case Open:
                    case "offen":
                    case "OFF":
                    case "off":
                    case "OF":
                    case "of":
                        return Open;
                }

                return null;
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
                    case "00":
                    case "":
                    case "-":
                        return GroupNone;

                    case Group1:
                    case "01":
                    case "I":
                    case "i":
                        return Group1;

                    case Group2:
                    case "02":
                    case "II":
                    case "ii":
                        return Group2;

                    case Group3:
                    case "03":
                    case "III":
                    case "iii":
                        return Group3;

                    case Group4:
                    case "05":
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
            public const string GirlsOnly = "G";

            public const int NoPromotionPossible = 999_999_999;

            public static string? ToClasses(
                string? useString)
            {
                switch (useString?.Trim())
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

                    case GirlsOnly:
                    case "g":
                    case "Girls":
                    case "girls":
                        return GirlsOnly;
                }

                return null;
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

            /// <summary>
            /// See "5otsvto2023.pdf" (ÖTSV TURNIERORDNUNG): 
            /// * § 10 - STARTKLASSENÄNDERUNG, page 35
            /// </summary>
            /// <param name="forAgeClass"></param>
            /// <param name="forClass"></param>
            /// <returns></returns>
            public static int GetMinStartsForPromotion(
                string? forDiscepline,
                string? forAgeClass,
                string? forAgeGroup,
                string? forClass)
            {
                return 10;
            }

            /// <summary>
            /// See "5otsvto2023.pdf" (ÖTSV TURNIERORDNUNG): 
            /// * § 9 - STARTKLASSEN, page 33
            /// * § 10 - STARTKLASSENÄNDERUNG, page 35
            /// </summary>
            /// <param name="forAgeClass"></param>
            /// <param name="forClass"></param>
            /// <returns></returns>
            public static int GetMinPointsForPromotion(
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
                    case AgeClasses.Adult:
                        switch (useClasses)
                        {
                            case Classes.D:
                                return 900;

                            case Classes.C:
                                return 1_500;

                            case Classes.B:
                                return 1_300;

                            case Classes.A:
                                return 1_600;

                            case Classes.S:
                                return NoPromotionPossible;
                        }
                        break;

                    case AgeClasses.Senior:
                        switch (useAgeGroup)
                        {
                            case AgeGroups.Group1:
                                switch (useDiscepline)
                                {
                                    case Disciplines.Sta:
                                        switch (useClasses)
                                        {
                                            case Classes.D:
                                                return 900;

                                            case Classes.C:
                                                return 1_500;

                                            case Classes.B:
                                                return 1_300;

                                            case Classes.A:
                                                return 1_600;

                                            case Classes.S:
                                                return NoPromotionPossible;
                                        }
                                        break;

                                    case Disciplines.La:
                                        switch (useClasses)
                                        {
                                            case Classes.D:
                                                return 900;

                                            case Classes.C:
                                                return 1_500;

                                            case Classes.B:
                                                return 1_800;

                                            case Classes.A:
                                                return 0;

                                            case Classes.S:
                                                return NoPromotionPossible;
                                        }
                                        break;
                                }
                                break;

                            case AgeGroups.Group2:
                            case AgeGroups.Group3:
                            case AgeGroups.Group4:
                                switch (useDiscepline)
                                {
                                    case Disciplines.Sta:
                                        switch (useClasses)
                                        {
                                            case Classes.D:
                                                return 1_400;

                                            case Classes.C:
                                                return 2_400;

                                            case Classes.B:
                                                return 2_000;

                                            case Classes.A:
                                                return 2_600;

                                            case Classes.S:
                                                return NoPromotionPossible;
                                        }
                                        break;

                                    case Disciplines.La:
                                        switch (useClasses)
                                        {
                                            case Classes.D:
                                                return 1_400;

                                            case Classes.C:
                                                return 2_400;

                                            case Classes.B:
                                                return 2_800;

                                            case Classes.A:
                                                return 0;

                                            case Classes.S:
                                                return NoPromotionPossible;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;

                    case AgeClasses.Juvenile:
                    case AgeClasses.Junior:
                        switch (useClasses)
                        {
                            case Classes.D:
                                return 1_000;

                            case Classes.C:
                                return 1_800;

                            case Classes.B:
                                return NoPromotionPossible;

                            case Classes.A:
                                return NoPromotionPossible;

                            case Classes.S:
                                return NoPromotionPossible;
                        }
                        break;

                    case AgeClasses.Youth:
                        switch (useClasses)
                        {
                            case Classes.D:
                                return 900;

                            case Classes.C:
                                return 1_500;

                            case Classes.B:
                                return 1_000;

                            case Classes.A:
                                return NoPromotionPossible;

                            case Classes.S:
                                return NoPromotionPossible;
                        }
                        break;
                }

                return NoPromotionPossible;
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

        public static class Participants
        {
            public const string NoneOetsvOrgId = "-99999";

            public static bool CheckValidOetsvParticipantOrgId(
                string? participantOrgId)
            {
                switch (participantOrgId)
                {
                    case null:
                    case NoneOetsvOrgId:
                        return false;

                    default:
                        if (int.TryParse(
                            participantOrgId,
                            out var parsedId) == false
                            || parsedId <= 0)
                        {
                            return false;
                        }

                        return true;
                }
            }
        }
    }
}
