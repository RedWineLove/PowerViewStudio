using CommonInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading.Tasks;

namespace SearchService
{
    class Program
    {
        static EndpointAddress serviceAddress;
        static void Main(string[] args)
        {
            if (FindService())
                InvokeService();
        }

        // ** DISCOVERY ** //
        static bool FindService()
        {

            Console.WriteLine("\nFinding Calculator Service ..");
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            DiscoveryClient discoveryClient =
                new DiscoveryClient(new UdpDiscoveryEndpoint());

            var calculatorServices =                
                discoveryClient.Find(new FindCriteria(typeof(IHelloWorldService)));

            discoveryClient.Close();

            Console.WriteLine("Find Completed: " + sw.Elapsed);

            if (calculatorServices.Endpoints.Count == 0)
            {
                Console.WriteLine("\nNo services are found.");
                return false;
            }
            else
            {
                serviceAddress = calculatorServices.Endpoints[0].Address;
                return true;
            }

            
        }

        static void InvokeService()
        {
            Console.WriteLine("Invoke Service");
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            ChannelFactory<IHelloWorldService> myChannelFactory = null;
            try
            {
                Console.WriteLine("\nInvoking Calculator Service at {0}\n", serviceAddress);
                BasicHttpBinding myBinding = new BasicHttpBinding();
                myChannelFactory = new ChannelFactory<IHelloWorldService>(myBinding, serviceAddress);
                IHelloWorldService service = myChannelFactory.CreateChannel();
                string requestMesg = service.SayHello("My Name is " + System.Net.Dns.GetHostName());
                Console.WriteLine("Result: " + requestMesg);
                myChannelFactory.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                myChannelFactory.Abort();
            }
            finally
            {
                Console.WriteLine("Invoke Completed: " + sw.Elapsed);
            }
        }
    }
    
}
