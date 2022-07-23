using Microsoft.AspNetCore.Mvc;

namespace LInkedIn.ECommerce.Api.Customers.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        var customer = new Customer { Id = 1, Name = $"Customer_{id}" };
        return await Task.FromResult(Ok(customer));
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}