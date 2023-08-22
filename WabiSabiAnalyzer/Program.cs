﻿using WabiSabiAnalyzer;

string txidsPath = Directory.GetCurrentDirectory() + "/txids.txt";

var results = new List<Result>();
var mempoolSpaceApiClient = new MempoolSpaceApiClient("Main");
var txids = new List<string>(File.ReadAllLines(txidsPath));

foreach (var txid in txids)
{
	MempoolSpaceResponse? tx = null;
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
		Console.WriteLine($"{txid} couldn't be found by mempool.space");
		continue;
	}

	var inputGroupedByValue = tx.vin.Select(x => x.prevout.value).GetIndistinguishable().ToList();
	var outputsGroupedByValue = tx.vout.Select(x => x.value).GetIndistinguishable().ToList();
	var changeCount = outputsGroupedByValue.Count(x => x.count == 1);

	var result = new Result(
		txid: txid,
		inputCount: tx.vin.Count,
		outputCount: tx.vout.Count,
		changeCount: changeCount,
		changeRatio: (decimal)changeCount / tx.vout.Count,
		inputAmount: tx.vin.Select(x => (ulong)x.prevout.value).Sum(),
		totalFee: (ulong)tx.fee,
		size: (int)(tx.weight / 4.0m),
		calculatedFeeRate: tx.fee / (tx.weight / 4.0m),
		averageInputAnonset: (decimal)tx.vin.Count / inputGroupedByValue.Count,
		averageOutputAnonset: (decimal)tx.vout.Count / outputsGroupedByValue.Count,
		taprootCount: tx.vout.Count(x => x.scriptpubkey_type == "v1_p2tr"),
		bech32Count: tx.vout.Count(x => x.scriptpubkey_type == "v0_p2wpkh")
	);

	Console.WriteLine();
	Display.DisplayResults(new List<Result>() { result });
	
	results.Add(result);
}

Display.DisplayResults(results);