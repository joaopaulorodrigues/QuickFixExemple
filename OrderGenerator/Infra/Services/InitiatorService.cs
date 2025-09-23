using OrderGenerator.Infra.Interfaces;
using QuickFix;
using QuickFix.Fields;
using QuickFix.FIX44;
using Message = QuickFix.Message;

namespace OrderGenerator.Infra.Services;

public class InitiatorService: MessageCracker, IApplication, IInitiatorServices
{
    private Session? _session = null;

    public void FromAdmin(Message message, SessionID sessionId) { }
    public void ToAdmin(Message message, SessionID sessionId) { }
    public void ToApp(Message message, SessionID sessionId)
    {
        try
        {
            bool possDupFlag = false;
            if (message.Header.IsSetField(Tags.PossDupFlag))
            {
                possDupFlag = message.Header.GetBoolean(Tags.PossDupFlag);
            }
            if (possDupFlag)
                throw new DoNotSend();
        }
        catch (FieldNotFoundException)
        { }

        Console.WriteLine();
        Console.WriteLine("OUT: " + message.ConstructString());
    }
    public void FromApp(Message message, SessionID sessionId)
    {
        Console.WriteLine("IN:  " + message.ConstructString());
        try
        {
            Crack(message, sessionId);
        }
        catch (Exception ex)
        {
            Console.WriteLine("==Cracker exception==");
            Console.WriteLine(ex.ToString());
            Console.WriteLine(ex.StackTrace);
        }
    }
    public void OnCreate(SessionID sessionId)
    {
        _session = Session.LookupSession(sessionId);
        if (_session is null)
            throw new ApplicationException("Somehow session is not found");
    }
    public void OnLogon(SessionID sessionId) { Console.WriteLine("Logon - " + sessionId); }
    public void OnLogout(SessionID sessionId) { Console.WriteLine("Logout - " + sessionId); }

    public void OnMessage(ExecutionReport n, SessionID s)
    {
        ExecType execType = n.ExecType;
        if(execType.Value == ExecType.NEW)
            Console.WriteLine(" Executada ");
        else
            Console.WriteLine(" Não executada ");
    }

    public bool SendMessage(Message m)
    {
        if (_session is not null)
            return _session.Send(m);
        else
        {
            Console.WriteLine("Can't send message: session not created.");
            return false;
        }
    }
}