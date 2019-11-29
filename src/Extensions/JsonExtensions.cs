using System.Text.Json;

namespace ITfoxtec.Identity
{
    /// <summary>
    /// Extension methods for Json.
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Json Serializer.
        /// </summary>
        public static readonly JsonSerializerOptions Settings = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            //TODO waiting for IgnoreDefaultValues
            //IgnoreDefaultValues = true
        };

        /// <summary>
        /// Json Serializer with indented format.
        /// </summary>
        public static readonly JsonSerializerOptions SettingsIndented = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            //TODO waiting for IgnoreDefaultValues
            //IgnoreDefaultValues = true,
            WriteIndented = true
        };

        /// <summary>
        /// Converts an object to a json string.
        /// </summary>
        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj, Settings);
        }
        /// <summary>
        /// Converts an object to a json indented string.
        /// </summary>
        public static string ToJsonIndented(this object obj)
        {
            return JsonSerializer.Serialize(obj, SettingsIndented);
        }

        /// <summary>
        /// Converts a json string to an object.
        /// </summary>
        public static T ToObject<T>(this string json)
        {
            return JsonSerializer.Deserialize<T>(json, Settings);
        }
    }
}
