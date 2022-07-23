using LInkedIn.ECommerce.Api.Client.Core;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Communication.Wcf;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Client;
using System.Fabric;

namespace LInkedIn.ECommerce.Client
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var serviceUri = new Uri("fabric:/LInkedIn.ECommerce/LInkedIn.ECommerce.Api.Orders");
            var binding = WcfUtility.CreateTcpClientBinding();

            var servicePartitionResolver = ServicePartitionResolver.GetDefault();
            var wcfCommunicationClientFactory = 
                new WcfCommunicationClientFactory<IOrdersService>(binding, null, servicePartitionResolver);

            var servicePartitionClient = 
                new ServicePartitionClient<WcfCommunicationClient<IOrdersService>>(wcfCommunicationClientFactory, serviceUri);

            Console.WriteLine("Please wait...");

            try
            {
                var orders = await servicePartitionClient.InvokeWithRetryAsync(client => client.Channel.GetOrdersAsync());

                if (orders != null && orders.Any())
                {
                    foreach (var order in orders)
                    {
                        Console.WriteLine($"{order.Id} {order.CustomerId} {order.OrderDate} {order.Total}");
                    }
                }
            }
            catch (FabricServiceNotFoundException ex)
            {
                Console.WriteLine($"Service did not find on address: {serviceUri} \n{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while calling the serive fabric wcf service: {ex.Message}");
            }
        }
    }
}
