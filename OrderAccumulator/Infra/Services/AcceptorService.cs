using OrderAccumulator.Domain.Data;
using QuickFix;
using QuickFix.Fields;

namespace OrderAccumulator.Infra;

public class AcceptorService: MessageCracker, IApplication
{
    private int orderID = 0;
    private int execID = 0;
    private FinancialExposure exposure = new FinancialExposure();
    private string GenOrderID() { return (++orderID).ToString(); }
    private string GenExecID() { return (++execID).ToString(); }
    
    public void FromApp(Message message, SessionID sessionID)
    {
        Crack(message, sessionID);
    }

    public void ToApp(Message message, SessionID sessionID) { }
    public void FromAdmin(Message message, SessionID sessionID) { }
    public void OnCreate(SessionID sessionID) { }
    public void OnLogout(SessionID sessionID) { }
    public void OnLogon(SessionID sessionID) { }
    public void ToAdmin(Message message, SessionID sessionID) { }

    public void OnMessage(QuickFix.FIX44.NewOrderSingle n, SessionID s)
    {
        Symbol symbol = n.Symbol;
        Side side = n.Side;
        OrdType ordType = n.OrdType;
        OrderQty orderQty = n.OrderQty;
        Price price = n.Price;
        ClOrdID clOrdID = n.ClOrdID;

        if (exposure.SetExposure(symbol, side, orderQty, price))
        {
            Console.WriteLine("Executada");
        }
        else
        {
            Console.WriteLine("Ultrapassou a exposição financeira");
        }


    QuickFix.FIX44.ExecutionReport exReport = new QuickFix.FIX44.ExecutionReport(
            new OrderID(GenOrderID()),
            new ExecID(GenExecID()),
            new ExecType(ExecType.FILL),
            new OrdStatus(OrdStatus.FILLED),
            symbol, //shouldn't be here?
            side,
            new LeavesQty(0),
            new CumQty(orderQty.Value),
            new AvgPx(price.Value));

        exReport.Set(clOrdID);
        exReport.Set(symbol);
        exReport.Set(orderQty);
        exReport.Set(new LastQty(orderQty.Value));
        exReport.Set(new LastPx(price.Value));

        if (n.IsSetAccount())
            exReport.SetField(n.Account);

        try
        {
            Session.SendToTarget(exReport, s);
        }
        catch (SessionNotFound ex)
        {
            Console.WriteLine("==session not found exception!==");
            Console.WriteLine(ex.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

}