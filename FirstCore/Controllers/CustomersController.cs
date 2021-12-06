using FirstCore.Data;
using FirstCore.Data.Dtos;
using FirstCore.IRepos;
using FirstCore.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FirstCore.Controllers
{
    [Route("api/Customers")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersRepo customersRepo = null;
        public CustomersController(ICustomersRepo Repo)
        {
            customersRepo = Repo;
        }
        [HttpGet]
        [Authorize(Roles =AppConstants.AdminRole)]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var customersDto = await customersRepo.GetAllAsync();
            return Ok(customersDto);
        }
        [HttpGet]
        [Route("details")]
        public async Task<IActionResult> GetCustomerAsync(int id)
        {
            var customerDto = await customersRepo.FindAsync(id);
            if (customerDto != null)
                return Ok(customerDto);
            return NotFound();
        }
        [HttpGet]
        [Route("orders")]
        public async Task<IActionResult> GetCustomerOrdersAsync(int id)
        {
            var OrdersDto = await customersRepo.GetAllOrdersAsync(id);
            if (OrdersDto != null)
                return Ok(OrdersDto);
            return NotFound();
        }
        [HttpGet]
        [Route("orders/confirmed")]
        public async Task<IActionResult> GetCustomerConfirmedOrdersAsync(int id)
        {
            var OrdersDto = await customersRepo.GetConfirmedOrdersAsync(id);
            if (OrdersDto != null)
                return Ok(OrdersDto);
            return NotFound();
        }
        [HttpGet]
        [Route("orders/pending")]
        public async Task<IActionResult> GetCustomerPendingOrdersAsync(int id)
        {
            var OrdersDto = await customersRepo.GetPendingOrdersAsync(id);
            if (OrdersDto != null)
                return Ok(OrdersDto);
            return NotFound();
        }

        public async Task<IActionResult> AddCustomerAsync(CustomerDto customerDto)
        {
            if (ModelState.IsValid)
            {
                customerDto = await customersRepo.AddAsync(customerDto);
                return Created(Request.Path + "/" + customerDto.Id, customerDto);
            }
            return BadRequest();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCustomerAsync(int id, CustomerDto customerDto)
        {
            if (ModelState.IsValid)
            {
                var isFoundAndUpdated = await customersRepo.UpdateAsync(id, customerDto);
                if (isFoundAndUpdated)
                    return Ok();
                else
                    return NotFound();
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            var isFoundAndDeleted = await customersRepo.DeleteAsync(id);
            if (isFoundAndDeleted)
                return Ok();
            return NotFound();
        }
    }
}
