using AutoMapper;
using FirstCore.Data;
using FirstCore.Data.Dtos;
using FirstCore.Data.Models;
using FirstCore.IRepos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCore.Repos
{
    public class ProductsRepo : BaseRepo<Product,ProductDto>,IProductsRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ProductsRepo(AppDbContext context,IMapper mapper):base(mapper,context)
        {
            _context = context;
            _mapper = mapper;
        }        
        public async Task<bool> UpdateAsync(int id, ProductDto entityDto)
        {
            var product =await _context.Products.FindAsync(id);
            if(product!=null)
            {
                product.Name = entityDto.Name;
                product.Price = entityDto.Price;
                product.AvailableStock = entityDto.AvailableStock;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
