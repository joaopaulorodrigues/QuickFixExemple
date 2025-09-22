using OrderAccumulator.Infra;
using QuickFix;
using QuickFix.Store;

namespace OrderAccumulator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                SessionSettings settings = new SessionSettings("executor.cfg");
                IApplication executorApp = new AcceptorService();
                IMessageStoreFactory storeFactory = new FileStoreFactory(settings);


                ThreadedSocketAcceptor acceptor =
                    new ThreadedSocketAcceptor(executorApp, storeFactory, settings);

                
                acceptor.Start();
                
                Console.WriteLine("press <enter> to quit");
                Console.Read();
                acceptor.Stop();
            }
            catch (System.Exception e)
            {
                Console.WriteLine("==FATAL ERROR==");
                Console.WriteLine(e.ToString());
            }
        }
    }
}