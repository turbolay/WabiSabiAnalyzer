namespace WabiSabiAnalyzer;

public class Result
{
    public string Txid { get; }
    public int ConfirmedIn { get; }
    public int InputCount { get; }
    public int OutputCount { get; }
    public int ChangeCount { get; }
    public decimal ChangeRatio { get; }
    public ulong InputAmount { get; }
    public ulong TotalFee { get; }
    public int Size { get; }
    public decimal CalculatedFeeRate { get; }
    public decimal AverageInputAnonset { get; }
    public decimal AverageOutputAnonset { get; }
    public decimal AverageOutputAnonsetExcludingChange { get; }
    public int TaprootCount { get; }
    public int Bech32Count { get; }

    public Result(
        string txid,
        int confirmedIn,
        int inputCount,
        int outputCount,
        int changeCount,
        decimal changeRatio,
        ulong inputAmount,
        ulong totalFee,
        int size,
        decimal calculatedFeeRate,
        decimal averageInputAnonset,
        decimal averageOutputAnonset,
        decimal averageOutputAnonsetExcludingChange,
        int taprootCount,
        int bech32Count
    )
    {
        Txid = txid;
        ConfirmedIn = confirmedIn;
        InputCount = inputCount;
        OutputCount = outputCount;
        ChangeCount = changeCount;
        ChangeRatio = changeRatio;
        InputAmount = inputAmount;
        TotalFee = totalFee;
        Size = size;
        CalculatedFeeRate = calculatedFeeRate;
        AverageInputAnonset = averageInputAnonset;
        AverageOutputAnonset = averageOutputAnonset;
        AverageOutputAnonsetExcludingChange = averageOutputAnonsetExcludingChange;
        TaprootCount = taprootCount;
        Bech32Count = bech32Count;
    }
}
