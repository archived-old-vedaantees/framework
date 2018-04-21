using System;
using System.Collections.Generic;
using System.Linq;

namespace Vedaantees.Framework.Utilities
{
    public static class StringExtensions
    {
        public static string ToFirstLower(this string lower)
        {
            var firstChar = lower.Substring(0, 1);
            lower = lower.Substring(1, lower.Length - 1);
            return firstChar.ToLower() + lower;
        }

        public static string ToFirstUpper(this string lower)
        {
            var firstChar = lower.Substring(0, 1);
            lower = lower.Substring(1, lower.Length - 1);
            return firstChar.ToUpper() + lower.ToLower();
        }

        public static string ToCommaSeperatedStringsInDoubleQuotes(this List<string> tList)
        {
            var flatten = tList.Aggregate("", (current, el) => current + "\"" + el + "\"" + ",");
            return flatten.TrimEnd(',');
        }

        public static string ToCommaSeperatedString(this List<string> tList)
        {
            var flatten = tList.Aggregate("", (current, el) => current + el + ",");
            return flatten.TrimEnd(',');
        }

        public static string ToPascalCase(this string theString)
        {
            if (string.IsNullOrEmpty(theString))
                return string.Empty;

            if (theString.Length < 2) return theString.ToUpper();

            // Split the string into words.
            var words = theString.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            var result = "";
            foreach (var word in words)
                result +=
                    word.Substring(0, 1).ToUpper() +
                    word.Substring(1);

            return result;
        }
    }
}