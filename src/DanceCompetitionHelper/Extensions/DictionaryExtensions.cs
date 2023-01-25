namespace DanceCompetitionHelper.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddToBucket<TKey, TVal>(
            this Dictionary<TKey, List<TVal>> useDict,
            TKey byKey,
            TVal addValue,
            bool addOnce = false)
            where TKey : notnull
        {
            if (useDict == null
                || byKey == null)
            {
                return;
            }

            if (useDict.TryGetValue(
                byKey,
                out var foundList) == false)
            {
                foundList = new List<TVal>();
                useDict[byKey] = foundList;
            }

            if (addOnce)
            {
                if (foundList.Contains(
                    addValue) == false)
                {
                    foundList.Add(
                        addValue);
                }
            }
            else
            {
                foundList.Add(
                    addValue);
            }
        }

        public static void AddToBucket<TKey, TVal>(
            this Dictionary<TKey, HashSet<TVal>> useDict,
            TKey byKey,
            TVal addValue)
            where TKey : notnull
        {
            if (useDict == null
                || byKey == null)
            {
                return;
            }

            if (useDict.TryGetValue(
                byKey,
                out var foundHashSet) == false)
            {
                foundHashSet = new HashSet<TVal>();
                useDict[byKey] = foundHashSet;
            }

            if (foundHashSet.Contains(
                addValue) == false)
            {
                foundHashSet.Add(
                    addValue);
            }
        }
    }
}
