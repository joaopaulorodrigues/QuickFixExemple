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
        decimal order = qtd.Value*price.Value;
        var exposure = ExposureData[symbol.Value];
        if (side.Value == Side.BUY)
        {
            var newExposure = exposure + order;
            if (newExposure > LIMIT)
                return false;
            ExposureData[symbol.Value] = newExposure;
            return true;
        }
        else if (side.Value == Side.SELL)
        { 
            ExposureData[symbol.Value] = exposure - order;
            return true;
        }
        return false;
    }
}