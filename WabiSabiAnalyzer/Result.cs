namespace WabiSabiAnalyzer;

public class SimulationResult
{
    public int InputCount { get; }
    public int OutputCount { get; }
    public int ChangeCount { get; }
    public ulong InputAmount { get; }
    public ulong TotalFee { get; }
    public int Size { get; }
    public decimal CalculatedFeeRate { get; }
    public decimal AverageInputAnonset { get; }
    public decimal AverageOutputAnonset { get; }
    public int TaprootCount { get; }
    public int Bech32Count { get; }

    public SimulationResult(
        int inputCount,
        int outputCount,
        int changeCount,
        ulong inputAmount,
        ulong totalFee,
        int size,
        decimal calculatedFeeRate,
        decimal averageInputAnonset,
        decimal averageOutputAnonset,
        int taprootCount,
        int bech32Count
    )
    {
        InputCount = inputCount;
        OutputCount = outputCount;
        ChangeCount = changeCount;
        InputAmount = inputAmount;
        TotalFee = totalFee;
        Size = size;
        CalculatedFeeRate = calculatedFeeRate;
        AverageInputAnonset = averageInputAnonset;
        AverageOutputAnonset = averageOutputAnonset;
        TaprootCount = taprootCount;
        Bech32Count = bech32Count;
    }
}
