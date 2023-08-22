namespace WabiSabiAnalyzer;

public static class Display
{
	public static void DisplayResults(List<Result> results)
	{
	    Console.WriteLine();
	    if (results.Count > 1)
	    {
		    Console.WriteLine($"Average results from {results.Count} rounds");
	    }
	    else
	    {
		    Console.WriteLine($"Txid: {results.First().Txid}");
		    Console.WriteLine($"Confirmed in: {results.First().ConfirmedIn}");
	    }
	    Console.WriteLine($"Number of inputs:\t{results.Median(r => r.InputCount):0.##}");
	    Console.WriteLine($"Number of outputs:\t{results.Median(r => r.OutputCount):0.##}");
	    Console.WriteLine($"Number of changes:\t{results.Average(r => r.ChangeCount):0.##}");
	    Console.WriteLine($"Frequency of changes:\t{results.Average(r => r.ChangeRatio)*100:0.##} %");
	    Console.WriteLine($"Total in:\t\t{(decimal)(results.Median(r => (double)r.InputAmount) ?? 0) / 100000000m} BTC");
	    Console.WriteLine($"Total fee:\t\t{(decimal)(results.Median(r => (double)r.TotalFee) ?? 0) / 100000000m} BTC");
	    Console.WriteLine($"Size:\t\t\t{results.Median(r => r.Size):0.##} vbyte");
	    Console.WriteLine($"Fee rate:\t\t{results.Median(r => r.CalculatedFeeRate):0.##} sats/vbyte");
	    Console.WriteLine($"Input anonset:\t\t{results.Median(r => r.AverageInputAnonset):0.##}");
	    Console.WriteLine($"Output anonset:\t\t{results.Median(r => r.AverageOutputAnonset):0.##}");
	    Console.WriteLine($"Output AS w/o change:\t{results.Median(r => r.AverageOutputAnonsetExcludingChange):0.##}");
	    Console.WriteLine($"Taproot/bech32 ratio:\t{results.Median(r => r.TaprootCount):0.##}/{results.Median(r => r.Bech32Count):0.##}");
	}

}