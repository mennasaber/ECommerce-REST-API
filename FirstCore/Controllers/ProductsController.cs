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
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepo productsRepo = null;
        public ProductsController(IProductsRepo Repo)
        {
            productsRepo = Repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetProductAsync()
        {
            var productsDto = await productsRepo.GetAllAsync();
            return Ok(productsDto);
        }
        [HttpGet]
        [Route("details")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var productDto = await productsRepo.FindAsync(id);
            if (productDto != null)
                return Ok(productDto);
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> AddProductAsync(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                productDto = await productsRepo.AddAsync(productDto);
                return Created(Request.Path + "/" + productDto.Id, productDto);
            }
            return BadRequest();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProductAsync(int id, ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var isFoundAndUpdated = await productsRepo.UpdateAsync(id, productDto);
                if (isFoundAndUpdated)
                    return Ok();
                else
                    return NotFound();
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var isFoundAndDeleted = await productsRepo.DeleteAsync(id);
            if (isFoundAndDeleted)
                return Ok();
            return NotFound();
        }
    }
}
