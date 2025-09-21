using QuickFix;

namespace OrderGenerator.Infra.Interfaces;

public interface IInitiatorServices
{
    public bool SendMessage(Message message);
}