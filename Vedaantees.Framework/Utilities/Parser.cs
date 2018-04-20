using System;

namespace Vedaantees.Framework.Utilities
{
    public class Parser
    {
        public static long? TryParseToLong(string input)
        {
            long outValue;
            return long.TryParse(input, out outValue) ? (long?) outValue : null;
        }

        public static double? TryParseToDouble(string input)
        {
            double outValue;
            return double.TryParse(input, out outValue) ? (double?) outValue : null;
        }

        public static DateTime? TryParseToDateTime(string input)
        {
            DateTime outValue;
            return DateTime.TryParse(input, out outValue) ? (DateTime?) outValue : null;
        }
    }
}