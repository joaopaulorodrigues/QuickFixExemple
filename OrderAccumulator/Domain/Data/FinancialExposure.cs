using QuickFix.Fields;

namespace OrderAccumulator.Domain.Data;

public class FinancialExposure
{
    private decimal LIMIT = 100000;
    private IDictionary<string, decimal> ExposureData;

    public FinancialExposure()
    {
        ExposureData = new Dictionary<string, decimal>
        {
            {"PETR4", 0},
            {"VALE3", 0},
            {"VIIA4", 0},
        };
    }

    public decimal GetExposure(Symbol symbol) => ExposureData[symbol.Value];

    public bool SetExposure(Symbol symbol, Side side, OrderQty qtd, Price price)
    {
        if (!ExposureData.TryGetValue(symbol.Value, out var currentExposure))
            return false;
        
        decimal orderValue = qtd.Value * price.Value;
        decimal newExposure = currentExposure;

        switch (side.Value)
        {
            case Side.BUY:
                newExposure += orderValue;
                if (newExposure >= LIMIT)
                    return false;
                break;

            case Side.SELL:
                newExposure -= orderValue;
                if (newExposure < 0)
                    return false;
                break;

            default:
                return false;
        }

        ExposureData[symbol.Value] = newExposure;
        return true;
    }
}