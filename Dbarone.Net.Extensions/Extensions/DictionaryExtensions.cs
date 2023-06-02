namespace Dbarone.Net.Extensions;

/// <summary>
/// Extension methods for dictionaries.
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Gets a value from a dictionary, or default if the key does not exist.
    /// </summary>
    /// <typeparam name="K">The key type.</typeparam>
    /// <typeparam name="T">The value type</typeparam>
    /// <param name="dict">The dictionary to get the value from.</param>
    /// <param name="key">The key value.</param>
    /// <param name="defaultValue">The default value to return if the key is not found in the dictionary.</param>
    /// <returns>Returns the value associated with the key value specified, or the default value if the key does not exist.</returns>
    public static T GetOrDefault<K, T>(this IDictionary<K, T> dict, K key, T defaultValue = default(T))
    {
        if (dict.TryGetValue(key, out T result))
        {
            return result;
        }

        return defaultValue;
    }
}
