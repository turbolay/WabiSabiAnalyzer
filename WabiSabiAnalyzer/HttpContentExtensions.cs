using Newtonsoft.Json;

namespace WabiSabiAnalyzer;

public static class HttpContentExtensions
{
	/// <exception cref="JsonException">If JSON deserialization fails for any reason.</exception>
	/// <exception cref="InvalidOperationException">If the JSON string is <c>"null"</c> (valid value but forbiden in this implementation).</exception>
	public static async Task<T> ReadAsJsonAsync<T>(this HttpContent me)
	{
		var jsonString = await me.ReadAsStringAsync().ConfigureAwait(false);
		return JsonConvert.DeserializeObject<T>(jsonString)
		       ?? throw new InvalidOperationException("'null' is forbidden.");
	}
}