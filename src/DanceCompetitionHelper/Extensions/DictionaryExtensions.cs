namespace DanceCompetitionHelper.Extensions
{
    public static class DictionaryExtensions
    {
        public static List<TVal>? AddToBucket<TKey, TVal>(
            this Dictionary<TKey, List<TVal>> useDict,
            TKey byKey,
            TVal addValue,
            bool addOnce = false)
            where TKey : notnull
        {
            if (useDict == null
                || byKey == null)
            {
                return null;
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

            return foundList;
        }

        public static List<TVal>? AddToBucket<TKey1, TKey2, TVal>(
            this Dictionary<TKey1, Dictionary<TKey2, List<TVal>>> useDict,
            TKey1 byKey1,
            TKey2 byKey2,
            TVal addValue,
            bool addOnce = false)
            where TKey1 : notnull
            where TKey2 : notnull
        {
            if (useDict == null
                || byKey1 == null
                || byKey2 == null)
            {
                return null;
            }

            if (useDict.TryGetValue(
                byKey1,
                out var foundNestedDict) == false)
            {
                foundNestedDict = new Dictionary<TKey2, List<TVal>>();
                useDict[byKey1] = foundNestedDict;
            }

            if (foundNestedDict.TryGetValue(
                byKey2,
                out var foundList) == false)
            {
                foundList = new List<TVal>();
                foundNestedDict[byKey2] = foundList;
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

            return foundList;
        }

        public static List<TVal>? AddToBucket<TKey, TVal>(
            this Dictionary<TKey, List<TVal>> useDict,
            TKey byKey,
            IEnumerable<TVal> addValues,
            bool addOnce = false)
            where TKey : notnull
        {
            List<TVal>? retList = null;

            if (useDict == null
                || byKey == null)
            {
                return retList;
            }

            foreach (var curVal in addValues ?? Enumerable.Empty<TVal>())
            {
                retList = useDict.AddToBucket(
                    byKey,
                    curVal,
                    addOnce);
            }

            return retList;
        }

        public static HashSet<TVal>? AddToBucket<TKey, TVal>(
            this Dictionary<TKey, HashSet<TVal>> useDict,
            TKey byKey,
            TVal addValue)
            where TKey : notnull
        {
            if (useDict == null
                || byKey == null)
            {
                return null;
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

            return foundHashSet;
        }

        public static HashSet<TVal>? AddToBucket<TKey, TVal>(
            this Dictionary<TKey, HashSet<TVal>> useDict,
            TKey byKey,
            IEnumerable<TVal> addValues)
            where TKey : notnull
        {
            HashSet<TVal>? retHashSet = null;
            if (useDict == null
                || byKey == null)
            {
                return retHashSet;
            }

            foreach (var curVal in addValues ?? Enumerable.Empty<TVal>())
            {
                retHashSet = useDict.AddToBucket(
                    byKey,
                    curVal);
            }

            return retHashSet;
        }
    }
}
