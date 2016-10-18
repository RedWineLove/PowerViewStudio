using CommonInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri(string.Format("http://{0}:8000/discovery/scenarios/calculatorservice/{1}/",
        System.Net.Dns.GetHostName() , Guid.NewGuid().ToString()));

            // Create a ServiceHost for the CalculatorService type.
            using (ServiceHost serviceHost = new ServiceHost(typeof(HelloWorldService), baseAddress))
            {
                // add calculator endpoint
                serviceHost.AddServiceEndpoint(typeof(IHelloWorldService), new BasicHttpBinding(), string.Empty);

                // ** DISCOVERY ** //
                // make the service discoverable by adding the discovery behavior
                ServiceDiscoveryBehavior discoveryBehavior = new ServiceDiscoveryBehavior();
                serviceHost.Description.Behaviors.Add(discoveryBehavior);

                // send announcements on UDP multicast transport
                discoveryBehavior.AnnouncementEndpoints.Add(
                  new UdpAnnouncementEndpoint());

                // ** DISCOVERY ** //
                // add the discovery endpoint that specifies where to publish the services
                serviceHost.Description.Endpoints.Add(new UdpDiscoveryEndpoint());

                // Open the ServiceHost to create listeners and start listening for messages.
                serviceHost.Open();

                // The service can now be accessed.
                Console.WriteLine("Press <ENTER> to terminate service.");
                Console.ReadLine();
            }
        }
    }
}
