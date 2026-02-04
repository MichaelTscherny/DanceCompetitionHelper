using DanceCompetitionHelper.Database.Tables;

namespace DanceCompetitionHelper.Database.Extensions
{
    public static class TypeMapping
    {
        public static TDestination Map<TSource, TDestination>(
            this TSource source,
            TDestination destination)
            where TSource : TableBase
            where TDestination : TableBase
        {
            if (source == null)
            {
                return destination;
            }

            destination.Created = source.Created;
            destination.CreatedBy = source.CreatedBy;
            destination.LastModified = source.LastModified;
            destination.LastModifiedBy = source.LastModifiedBy;

            return destination;
        }
    }
}
