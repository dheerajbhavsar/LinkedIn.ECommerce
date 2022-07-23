using Microsoft.AspNetCore.Mvc;
using LInkedIn.ECommerce.Api.Core;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Client;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace LInkedIn.ECommerce.Api.Search.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    [HttpGet("products/{id}")]
    public async Task<IActionResult> GetProductAsync(int id)
    {
        var service = GetProductsService();
        var (IsSuccess, product, ErrorMessage) =
            await service.GetProductAsync(id);

        if (IsSuccess)
            return Ok(product);

        return NotFound(ErrorMessage);
    }

    [HttpGet("customers/{id}")]
    public async Task<IActionResult> GetCustomerAsync(int id)
    {
        var serviceName = "fabric:/LinkedIn.ECommerce/LInkedIn.ECommerce.Api.Customers";
        var serviceUri = await ResolveAsync(serviceName);
        try
        {
            var client = new HttpClient() { BaseAddress = serviceUri };
            var result = await client.GetAsync($"api/customers/{id}");

            if(result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var customer = JsonSerializer.Deserialize<dynamic>(content);

                return Ok(customer);
            }

            return NotFound();
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    private static IProductsService GetProductsService()
    {
        return ServiceProxy.Create<IProductsService>(new Uri("fabric:/LinkedIn.ECommerce/LInkedIn.ECommerce.Api.Products"));
    }

    private static async Task<Uri> ResolveAsync(string name)
    {
        var uri = new Uri(name);
        var resolver = ServicePartitionResolver.GetDefault();

        var service = await resolver.ResolveAsync(uri, ServicePartitionKey.Singleton, CancellationToken.None);

        var address = JObject.Parse(service.GetEndpoint().Address);

        var primary = (string)address["Endpoints"].First();
        return new Uri(primary);
    }
}
