using Microsoft.ServiceFabric.Services.Remoting;

namespace LInkedIn.ECommerce.Api.Core;

public interface IProductsService : IService
{
    Task<(bool IsSuccess, Product product, string ErrorMessage)> GetProductAsync(int id);
}

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
}