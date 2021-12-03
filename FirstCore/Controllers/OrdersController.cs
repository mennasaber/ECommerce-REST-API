using FirstCore.Data.Dtos;
using FirstCore.IRepos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCore.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersRepo ordersRepo = null;
        public OrdersController(IOrdersRepo Repo)
        {
            ordersRepo = Repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var OrdersDto = await ordersRepo.GetAllAsync();
            return Ok(OrdersDto);
        }
        [HttpGet]
        [Route("details")]
        public async Task<IActionResult> GetOrderAsync(int id)
        {
            var OrderDto = await ordersRepo.FindAsync(id);
            if (OrderDto != null)
                return Ok(OrderDto);
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> AddOrderAsync(OrderDto OrderDto)
        {
            if (ModelState.IsValid)
            {
                OrderDto = await ordersRepo.AddAsync(OrderDto);
                if (OrderDto != null)
                    return Created(Request.Path + "/" + OrderDto.Id, OrderDto);
                else
                    return BadRequest();
            }
            return BadRequest();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateOrderAsync(int id)
        {
            if (ModelState.IsValid)
            {
                var isFoundAndUpdated = await ordersRepo.UpdateAsync(id);
                if (isFoundAndUpdated)
                    return Ok();
                else
                    return NotFound();
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            var isFoundAndDeleted = await ordersRepo.DeleteAsync(id);
            if (isFoundAndDeleted)
                return Ok();
            return NotFound();
        }
    }
}
