using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Vedaantees.Framework.Utilities
{
    public class JsonExtensions
    {
        private enum JsonType { Object, Array }

        public Dictionary<string, string> Flatten(string json)
        {
            var jsonObject = JObject.Parse(json);
            var jTokens = jsonObject.Descendants().Where(p => !p.Any());
            var results = jTokens.Aggregate(new Dictionary<string, string>(), (properties, jToken) =>
                            {
                                properties.Add(jToken.Path, jToken.ToString());
                                return properties;
                            });
                            return results;
        }

        public string Unflatten(IDictionary<string, string> keyValues)
        {
            JContainer result = null;
            var setting = new JsonMergeSettings {MergeArrayHandling = MergeArrayHandling.Merge};

            foreach (var pathValue in keyValues)
            {
                if (result == null)
                    result = UnflattenSingle(pathValue);
                else
                    result.Merge(UnflattenSingle(pathValue), setting);
            }
            return (result as JObject)?.ToString(Formatting.None);
        }

        private static JContainer UnflattenSingle(KeyValuePair<string, string> keyValue)
        {
            var path = keyValue.Key;
            var value = keyValue.Value;
            var pathSegments = SplitPath(path);
            JContainer lastItem = null;

            foreach (var pathSegment in pathSegments.Reverse())
            {
                var type = GetJsonType(pathSegment);
                switch (type)
                {
                    case JsonType.Object:
                        var obj = new JObject();
                        if (null == lastItem)
                        {
                            obj.Add(pathSegment, value);
                        }
                        else
                        {
                            obj.Add(pathSegment, lastItem);
                        }
                        lastItem = obj;
                        break;

                    case JsonType.Array:
                        var array = new JArray();
                        int index = GetArrayIndex(pathSegment);
                        array = FillEmpty(array, index);
                        if (lastItem == null)
                        {
                            array[index] = value;
                        }
                        else
                        {
                            array[index] = lastItem;
                        }
                        lastItem = array;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return lastItem;
        }

        public static IList<string> SplitPath(string path)
        {
            var reg = new Regex(@"(?!\.)([^. ^\[\]]+)|(?!\[)(\d+)(?=\])");
            return (from Match match in reg.Matches(path) select match.Value).ToList();
        }

        private static JArray FillEmpty(JArray array, int index)
        {
            for (var i = 0; i <= index; i++)
                array.Add(null);

            return array;
        }

        private static JsonType GetJsonType(string pathSegment)
        {
            return int.TryParse(pathSegment, out var x) ? JsonType.Array : JsonType.Object;
        }

        private static int GetArrayIndex(string pathSegment)
        {
            if (int.TryParse(pathSegment, out var result))
                return result;

            throw new Exception("Unable to parse array index: " + pathSegment);
        }

    }
}