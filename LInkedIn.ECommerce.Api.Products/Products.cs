using System.Fabric;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using LInkedIn.ECommerce.Api.Core;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace LInkedIn.ECommerce.Api.Products;

/// <summary>
/// An instance of this class is created for each service instance by the Service Fabric runtime.
/// </summary>
internal sealed class Products : StatelessService, IProductsService
{
    public Products(StatelessServiceContext context)
        : base(context)
    { }

    public Task<(bool IsSuccess, Product product, string ErrorMessage)> GetProductAsync(int id)
    {
        return Task.FromResult((true, new Product { Id = 1, Name = $"Product_{id}" }, string.Empty));
    }

    /// <summary>
    /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
    /// </summary>
    /// <returns>A collection of listeners.</returns>
    protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
    {
        return this.CreateServiceRemotingInstanceListeners();
    }
}
