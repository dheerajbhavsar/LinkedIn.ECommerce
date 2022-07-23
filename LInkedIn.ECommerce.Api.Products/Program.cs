using System.Diagnostics;
using Microsoft.ServiceFabric.Services.Runtime;

namespace LInkedIn.ECommerce.Api.Products;

internal static class Program
{
    /// <summary>
    /// This is the entry point of the service host process.
    /// </summary>
    private static async Task Main()
    {
        try
        {
            // The ServiceManifest.XML file defines one or more service type names.
            // Registering a service maps a service type name to a .NET type.
            // When Service Fabric creates an instance of this service type,
            // an instance of the class is created in this host process.

            await ServiceRuntime.RegisterServiceAsync("LInkedIn.ECommerce.Api.ProductsType",
                context => new Products(context));

            ServiceEventSource.Current.ServiceTypeRegistered(Environment.ProcessId, typeof(Products).Name);

            // Prevents this host process from terminating so services keep running.
            Thread.Sleep(Timeout.Infinite);
        }
        catch (Exception e)
        {
            ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
            throw;
        }
    }
}
