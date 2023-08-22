using System.Net;

namespace WabiSabiAnalyzer;

public class MempoolSpaceApiClient
{
	private readonly HttpClient _httpClient = new();
	
	public MempoolSpaceApiClient(string network)
	{
		_httpClient.BaseAddress = new Uri(
			network == "TestNet"
				? "https://mempool.space/testnet/"
				: "https://mempool.space/");
	}
	
	public async Task<MempoolSpaceResponse?> GetTransactionInfosAsync(string txid, CancellationToken cancel)
	{
		HttpResponseMessage response;
		// Ensure not being banned by Mempool.space's API
		await Task.Delay(1000, cancel).ConfigureAwait(false);

		using var request = new HttpRequestMessage(HttpMethod.Get, $"api/tx/{txid}");
		response = await _httpClient.SendAsync(request, cancel).ConfigureAwait(false);

		if (!response.IsSuccessStatusCode)
		{
			// Tx was not received or not accepted into mempool by mempool.space's node.
			if (response.StatusCode == HttpStatusCode.NotFound)
			{
				return null;
			}
			throw new InvalidOperationException($"There was an unexpected error with request to mempool.space. {nameof(HttpStatusCode)} was {response?.StatusCode}.");
		}

		return await response.Content.ReadAsJsonAsync<MempoolSpaceResponse>().ConfigureAwait(false);
	}
}