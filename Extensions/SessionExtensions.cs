using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace PharmaSphere.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ISession"/> to support complex object storage using JSON serialization.
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// Serializes an object to a JSON string and stores it in the session.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="session">The session instance.</param>
        /// <param name="key">The session key.</param>
        /// <param name="value">The object to store.</param>
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        /// <summary>
        /// Retrieves and deserializes an object from the session.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="session">The session instance.</param>
        /// <param name="key">The session key.</param>
        /// <returns>The deserialized object, or the default value of <typeparamref name="T"/> if not found.</returns>
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
