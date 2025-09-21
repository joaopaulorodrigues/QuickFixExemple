using OrderGenerator.Infra.Interfaces;
using OrderGenerator.Infra.Services;
using QuickFix;
using QuickFix.Logger;
using QuickFix.Store;

namespace OrderGenerator.Infra.DependencyInjections;

public static class InfraDependencies
{
    public static void AddInfraDepedencies(this IServiceCollection services)
    {

        string file = "initiator.cfg";

        try
        {
            SessionSettings settings = new SessionSettings(file);
            InitiatorService application = new InitiatorService();
            IMessageStoreFactory storeFactory = new FileStoreFactory(settings);

            ILogFactory logFactory = new ScreenLogFactory(settings);
            QuickFix.Transport.SocketInitiator initiator = new QuickFix.Transport.SocketInitiator(application, storeFactory, settings, logFactory);

            services.AddSingleton<IInitiatorServices>(application);
            initiator.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }    
    }
}