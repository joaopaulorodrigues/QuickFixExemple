using OrderGenerator.Domain;
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
            var teste = new OrdersInfo();

            SessionSettings settings = new SessionSettings(file);
            InitiatorService application = new InitiatorService(teste);
            IMessageStoreFactory storeFactory = new FileStoreFactory(settings);

            ILogFactory logFactory = new ScreenLogFactory(settings);
            QuickFix.Transport.SocketInitiator initiator = new QuickFix.Transport.SocketInitiator(application, storeFactory, settings, logFactory);
            
            services.AddScoped<OrdersInfo>(_ =>  teste );
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