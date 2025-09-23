using OrderAccumulator.Domain.Data;
using QuickFix;
using QuickFix.Fields;
using QuickFix.FIX44;
using Message = QuickFix.Message;

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

    public void OnMessage(NewOrderSingle n, SessionID s)
    {
        Symbol symbol = n.Symbol;
        Side side = n.Side;
        OrderQty orderQty = n.OrderQty;
        Price price = n.Price;
        ClOrdID clOrdID = n.ClOrdID;

        var (execType, ordStatus) = SetExposure(symbol, side, orderQty, price);
        var exReport = GetExecutionReport(n, execType, ordStatus, symbol, side, orderQty, price, clOrdID);
        SendReport(s, exReport);
    }

    private (ExecType, OrdStatus) SetExposure(Symbol symbol, Side side, OrderQty orderQty, Price price)
    {
        ExecType execType;
        OrdStatus ordStatus;
        if (exposure.SetExposure(symbol, side, orderQty, price))
        {
            execType = new ExecType(ExecType.NEW);
            ordStatus = new OrdStatus(OrdStatus.NEW);
            Console.WriteLine("Executada");
        }
        else
        {
            execType = new ExecType(ExecType.REJECTED);
            ordStatus = new OrdStatus(OrdStatus.REJECTED);
            Console.WriteLine("Ultrapassou a exposição financeira");
        }

        return (execType,  ordStatus);
    }

    private static void SendReport(SessionID s, ExecutionReport exReport)
    {
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

    private ExecutionReport GetExecutionReport(NewOrderSingle n, ExecType execType, OrdStatus ordStatus, Symbol symbol, Side side,
        OrderQty orderQty, Price price, ClOrdID clOrdID)
    {
        ExecutionReport exReport = new ExecutionReport(
            new OrderID(GenOrderID()),
            new ExecID(GenExecID()),
            execType,
            ordStatus,
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
        return exReport;
    }
}