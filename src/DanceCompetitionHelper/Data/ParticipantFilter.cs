using DanceCompetitionHelper.Database.Tables;

using Microsoft.EntityFrameworkCore;

namespace DanceCompetitionHelper.Data
{
    public class ParticipantFilter
    {
        public string? Name { get; set; }
        public int? StartNumberFrom { get; set; }
        public int? StartNumberTo { get; set; }

        public void CheckValuesAndFix()
        {
            // if only one value, "StartNumberFrom" is set
            if (StartNumberFrom.HasValue == false
                && StartNumberTo.HasValue)
            {
                StartNumberFrom = StartNumberTo;
                StartNumberTo = null;
            }

            // "StartNumberFrom" is always less or equal to "StartNumberTo"
            if (StartNumberFrom.HasValue
                && StartNumberTo.HasValue)
            {
                if (StartNumberFrom.Value < StartNumberTo.Value)
                {
                    (StartNumberFrom, StartNumberTo) = (StartNumberTo, StartNumberFrom);
                }
            }
        }

        public IQueryable<Participant> Where(
            IQueryable<Participant> participants)
        {
            CheckValuesAndFix();

            var retQuery = participants;

            if (string.IsNullOrEmpty(
                Name) == false
                // we don't mind "full whitespace string"...
                && string.IsNullOrWhiteSpace(
                    Name) == false)
            {
                // TODO: case sensitive?..
                var useNamePattern = Name;

                if (useNamePattern.StartsWith('%') == false)
                {
                    useNamePattern = '%' + useNamePattern;
                }

                if (useNamePattern.EndsWith('%') == false)
                {
                    useNamePattern += '%';
                }

                retQuery = retQuery.Where(
                    x => EF.Functions.Like(
                        x.NamePartA,
                        useNamePattern)
                    || EF.Functions.Like(
                        x.NamePartB,
                        useNamePattern)
                    || EF.Functions.Like(
                        x.ClubName,
                        useNamePattern));
            }

            if (StartNumberFrom.HasValue)
            {
                retQuery = retQuery.Where(
                    x => x.StartNumber >= StartNumberFrom);
            }

            if (StartNumberTo.HasValue)
            {
                retQuery = retQuery.Where(
                    x => x.StartNumber <= StartNumberTo);
            }

            return retQuery;
        }
    }
}
