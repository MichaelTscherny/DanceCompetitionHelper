﻿using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Database.Extensions
{
    public static class HumanReadableExtensions
    {
        public static string GetCompetitionName(
            this Competition? forCompetition,
            bool orgClassIdFirst = false) =>
                string.Format(
                    orgClassIdFirst
                        ? "({1}) {0}"
                        : "{0} ({1})",
                    forCompetition?.CompetitionName ?? "??",
                    forCompetition?.OrgCompetitionId ?? "??");

        public static string GetCompetitionClassName(
            this CompetitionClass? forCompetitionClass,
            bool orgClassIdFirst = false) =>
                string.Format(
                    orgClassIdFirst
                        ? "({1}) {0}"
                        : "{0} ({1})",
                    forCompetitionClass?.CompetitionClassName ?? "??",
                    forCompetitionClass?.OrgClassId ?? "??");

        public static string GetCompetitionClasseNames(
            this IEnumerable<CompetitionClass>? forCompetitionClasses,
            bool orgClassIdFirst = false) =>
                string.Join(
                    "; ",
                    (forCompetitionClasses ?? Enumerable.Empty<CompetitionClass>())
                        .OrderBy(
                            x => x.OrgClassId)
                        .Select(
                            x => x.GetCompetitionClassName(
                                orgClassIdFirst)));

        public static string GetStartNumber(
            this IEnumerable<Participant>? forParticipants) =>
                string.Join(
                    ", ",
                    forParticipants
                        ?.Select(
                            x => x.StartNumber)
                        ?.OrderBy(
                            x => x)
                        ?.Select(
                            x => string.Format(
                                "#{0}",
                                x))
                        ?? Enumerable.Empty<string>());

        public static string GetNames(
            this Participant? forParticipant)
        {
            return GetNames(
                forParticipant?.NamePartA,
                forParticipant?.NamePartB);
        }

        public static string GetNames(
            string? nameA,
            string? nameB)
        {
            var useNameA = nameA ?? string.Empty;
            var useNameB = nameB ?? string.Empty;

            if (string.IsNullOrEmpty(
                useNameB))
            {
                return useNameA;
            }

            return string.Format(
                "{0} / {1}",
                useNameA,
                useNameB);
        }

        public static (int CountParticipants, int ByWinning, int ByPromotion, int ExtraManualStarter, int SumParticipants) GetCompetitionParticipantCounts(
            this CompetitionClass competitionClass)
        {
            var useCountParticipants = competitionClass.DisplayInfo?.CountParticipants ?? 0;
            var useByWinning = competitionClass.DisplayInfo?.ExtraParticipants.ByWinning ?? 0;
            var useByPromotion = competitionClass.DisplayInfo?.ExtraParticipants.ByPromotion ?? 0;
            var useExtraManualStarter = competitionClass.ExtraManualStarter;
            var useSum = useCountParticipants
                + useByWinning
                + useByPromotion
                + useExtraManualStarter;

            return (
                useCountParticipants,
                useByWinning,
                useByPromotion,
                useExtraManualStarter,
                useSum);
        }

        public static string DefaultTrim(
            this string forString)
        {
            return forString?.Trim() ?? string.Empty;
        }

        public static string? DefaultNullableTrim(
            this string? forString)
        {
            return forString?.Trim();
        }
    }
}
