using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Web.Models.ParticipantModels
{
    public class ParticipantPromotionInfo
    {
        private readonly HashSet<Participant> _sameParticipants = new HashSet<Participant>();
        private readonly HashSet<Guid> _sameParticipantIds = new HashSet<Guid>();

        private readonly Dictionary<Guid, CompetitionClass> _startingCompetitionClasses = new Dictionary<Guid, CompetitionClass>();

        public List<CompetitionClass> CompetitionClasses => new List<CompetitionClass>(
            _startingCompetitionClasses.Values);

        #region Participants

        public void AddParticipant(
            Participant participant)
        {
            if (participant == null)
            {
                return;
            }

            var useParticipantId = participant.ParticipantId;
            if (_sameParticipantIds.Contains(
                useParticipantId))
            {
                return;
            }

            _sameParticipants.Add(
                participant);
            _sameParticipantIds.Add(
                useParticipantId);
        }

        public bool IsSameParticipant(
            Participant chkParticipant)
        {
            return IsSameParticipant(
                chkParticipant?.ParticipantId ?? Guid.Empty);
        }

        public bool IsSameParticipant(
            Guid participantId)
        {
            return _sameParticipantIds.Contains(
                participantId);
        }

        public bool IsSameParticipant(
            IEnumerable<Guid> participantIds)
        {
            foreach (var curChkId in participantIds
                ?? Enumerable.Empty<Guid>())
            {
                if (IsSameParticipant(
                    curChkId))
                {
                    return true;
                }
            }

            return false;
        }

        public string GetNames()
        {
            return _sameParticipants
                .FirstOrDefault()
                .GetNames();
        }

        #endregion // Participants

        #region CompetitionClasses

        public void AddCompetitionClass(
            CompetitionClass competitionClass)
        {
            if (competitionClass == null)
            {
                return;
            }

            var useCompetitionClassId = competitionClass.CompetitionClassId;
            if (_startingCompetitionClasses.ContainsKey(
                useCompetitionClassId))
            {
                return;
            }

            _startingCompetitionClasses[
                useCompetitionClassId] = competitionClass;
        }

        public bool WithinCompetitionClass(
            CompetitionClass chkCompetitionClass)
        {
            return WithinCompetitionClass(
                chkCompetitionClass?.CompetitionClassId ?? Guid.Empty);
        }

        public bool WithinCompetitionClass(
            Guid competitionClassId)
        {
            return _startingCompetitionClasses.ContainsKey(
                competitionClassId);
        }

        #endregion // CompetitionClasses

        public IEnumerable<CompetitionClass> GetCompetitionClass()
        {
            foreach (var curCompClass in _startingCompetitionClasses.Values)
            {
                yield return curCompClass;
            }
        }

        public IEnumerable<CompetitionClass> GetCompetitionClassOrderedBy(
            IEnumerable<Guid> competitionClassIds)
        {
            foreach (var curCompClassId in competitionClassIds
                ?? Enumerable.Empty<Guid>())
            {
                if (_startingCompetitionClasses.TryGetValue(
                    curCompClassId,
                    out var foundCompClass))
                {
                    yield return foundCompClass;
                }
            }
        }
    }
}
