using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace LInkedIn.ECommerce.Api.Customers;

/// <summary>
/// The FabricRuntime creates an instance of this class for each service type instance.
/// </summary>
internal sealed class Customers : StatelessService
{
    public Customers(StatelessServiceContext context)
        : base(context)
    { }

    /// <summary>
    /// Optional override to create listeners (like tcp, http) for this service instance.
    /// </summary>
    /// <returns>The collection of listeners.</returns>
    protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
    {
        return new ServiceInstanceListener[]
        {
            new ServiceInstanceListener(serviceContext =>
                new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                {
                    ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                    var builder = WebApplication.CreateBuilder();

                    builder.Services.AddSingleton(serviceContext);
                    builder.WebHost
                                .UseKestrel()
                                .UseContentRoot(Directory.GetCurrentDirectory())
                                .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                .UseUrls(url);
                    
                    // Add services to the container.
                    
                    builder.Services.AddControllers();
                    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                    builder.Services.AddEndpointsApiExplorer();
                    builder.Services.AddSwaggerGen();
                    
                    var app = builder.Build();
                    
                    // Configure the HTTP request pipeline.
                    //if (app.Environment.IsDevelopment())
                    //{
                        app.UseSwagger();
                        app.UseSwaggerUI();
                    //}
                    
                    app.UseAuthorization();
                    
                    app.MapControllers();
                    
                    return app;


                }))
        };
    }
}
