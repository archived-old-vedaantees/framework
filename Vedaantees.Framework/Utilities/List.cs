#region  usings 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace Vedaantees.Framework.Utilities
{
    /// <summary>
    ///     Extensions for list type collections
    /// </summary>
    public static class List
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
                if (seenKeys.Add(keySelector(element)))
                    yield return element;
        }

        public static string ConvertListToString<T>(this IList<T> elementsInList, string[] propertyNamesToRead,
            char delimiter)
        {
            var stringBuilder = new StringBuilder();

            foreach (var element in elementsInList)
            {
                var tempElement = element;
                var value = string.Empty;

                foreach (var property in propertyNamesToRead)
                    if (property.Contains("."))
                    {
                        var splits = property.Split('.');
                        var childProperty = element.GetType().GetProperty(splits[0]).GetValue(element, null);

                        for (var iCnt = 1; iCnt <= splits.Count() - 1; iCnt++)
                            if (splits.Count() - 1 == iCnt)
                            {
                                if (childProperty != null)
                                    if (value == string.Empty)
                                        value =
                                            childProperty.GetType().GetProperty(splits[iCnt]).GetValue(childProperty,
                                                null).ToString();
                                    else
                                        value = value + delimiter +
                                                childProperty.GetType().GetProperty(splits[iCnt]).GetValue(
                                                    childProperty, null);
                            }
                            else
                            {
                                childProperty = childProperty?.GetType().GetProperty(splits[iCnt])
                                    .GetValue(childProperty, null);
                            }
                    }
                    else
                    {
                        if (value == string.Empty)
                            value = tempElement.GetType().GetProperty(property).GetValue(tempElement, null).ToString();
                        else
                            value = value + delimiter + " " +
                                    tempElement.GetType().GetProperty(property).GetValue(tempElement, null);
                    }

                stringBuilder.AppendLine(value);
            }

            return stringBuilder.ToString();
        }

        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> fnRecurse)
        {
            foreach (var item in source)
            {
                yield return item;
                var seqRecurse = fnRecurse(item);

                if (seqRecurse != null)
                    foreach (var itemRecurse in Traverse(seqRecurse, fnRecurse))
                        yield return itemRecurse;
            }
        }

        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var item in @this) action(item);
        }

        public static string ConvertToString(List<string> @this, string seperator)
        {
            return @this.Aggregate((c, n) => c + seperator + n);
        }
    }
}