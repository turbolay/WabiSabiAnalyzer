using Newtonsoft.Json;
using WabiSabiAnalyzer;

var txidsPath = Directory.GetCurrentDirectory() + "/txids.txt";
var dbPath = Directory.GetCurrentDirectory() + "/db.json";

Dictionary<string, MempoolSpaceResponse> db = new();
if (File.Exists(dbPath))
{
	db = JsonConvert.DeserializeObject<Dictionary<string, MempoolSpaceResponse>>(File.ReadAllText(dbPath)) ??
	         new Dictionary<string, MempoolSpaceResponse>();
}

var results = new List<Result>();
var mempoolSpaceApiClient = new MempoolSpaceApiClient("Main");
var txids = new List<string>(File.ReadAllLines(txidsPath));

foreach (var txid in txids)
{
	if (!db.TryGetValue(txid, out var tx))
	{
		for (var i = 0; i < 3; i++)
		{
			try
			{
				tx = await mempoolSpaceApiClient.GetTransactionInfosAsync(txid, CancellationToken.None);
				break;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"ERROR: {ex}");
			}
		}
		if (tx is null)
		{
			Console.WriteLine($"{txid} couldn't be found by mempool.space nor in db");
			continue;
		}
		db.Add(txid, tx);
	}

	if (tx.vin.Count < 130)
	{
		Console.WriteLine($"{txid} is not a WabiSabi round");
		continue;
	}
	var inputGroupedByValue = tx.vin.Select(x => x.prevout.value).GetIndistinguishable().ToList();
	var outputsGroupedByValue = tx.vout.Select(x => x.value).GetIndistinguishable().ToList();
	var inputAmount = tx.vin.Select(x => x.prevout.value).Sum();
	var changes = outputsGroupedByValue.Where(x => x.count == 1).ToList();
	var totalChange = changes.Select(x => x.value).Sum();
	
	var result = new Result(
		txid: txid,
		confirmedIn: tx.status.block_height,
		inputCount: tx.vin.Count,
		outputCount: tx.vout.Count,
		changeCount: changes.Count,
		changeRatio: (decimal)changes.Count / tx.vout.Count,
		inputAmount: tx.vin.Select(x => x.prevout.value).Sum(),
		totalChange: totalChange,
		ratioChangeValue: (decimal)totalChange / inputAmount,
		totalFee: (ulong)tx.fee,
		size: (int)(tx.weight / 4.0m),
		calculatedFeeRate: tx.fee / (tx.weight / 4.0m),
		averageInputAnonset: (decimal)tx.vin.Count / inputGroupedByValue.Count,
		averageOutputAnonset: (decimal)tx.vout.Count / outputsGroupedByValue.Count,
		averageOutputAnonsetExcludingChange: (decimal)(tx.vout.Count - changes.Count) / (outputsGroupedByValue.Count - changes.Count),
		taprootCount: tx.vout.Count(x => x.scriptpubkey_type == "v1_p2tr"),
		bech32Count: tx.vout.Count(x => x.scriptpubkey_type == "v0_p2wpkh")
	);

	Console.WriteLine();
	Display.DisplayResults(new List<Result>() { result });
	
	results.Add(result);
}

var json = JsonConvert.SerializeObject(db, Formatting.Indented);
File.WriteAllText(dbPath, json);

Display.DisplayResults(results);