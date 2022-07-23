using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using LInkedIn.ECommerce.Api.Client.Core;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using System.Linq;

namespace LInkedIn.ECommerce.Api.Orders
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Orders : StatelessService, IOrdersService
    {
        public Orders(StatelessServiceContext context)
            : base(context)
        { }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            var orders = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Now,
                    Total = 10
                },
                new Order
                {
                    Id = 2,
                    CustomerId = 2,
                    OrderDate = DateTime.Now,
                    Total = 10
                },
                new Order
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = DateTime.Now,
                    Total = 10
                },
                new Order
                {
                    Id = 4,
                    CustomerId = 3,
                    OrderDate = DateTime.Now,
                    Total = 10
                }
            };

            return await Task.FromResult(orders.AsEnumerable());
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener((context) => 
                    new WcfCommunicationListener<IOrdersService>(context, this,
                    WcfUtility.CreateTcpListenerBinding(), "ServiceEndpoint"))
            };
        }
    }
}
