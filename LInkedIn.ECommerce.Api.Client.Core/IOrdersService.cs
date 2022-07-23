using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.ServiceFabric.Services.Remoting;

namespace LInkedIn.ECommerce.Api.Client.Core
{
    [ServiceContract]
    public interface IOrdersService : IService
    {
        [OperationContract]
        Task<IEnumerable<Order>> GetOrdersAsync();
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public decimal Total { get; set; }
    }
}


