using AutoMapper;
using FirstCore.Data;
using FirstCore.Data.Dtos;
using FirstCore.Data.Models;
using FirstCore.IRepos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FirstCore.Repos
{
    public class CustomersRepo : BaseRepo<Customer, CustomerDto>, ICustomersRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public CustomersRepo(AppDbContext context, IMapper mapper) : base(mapper, context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> UpdateAsync(int id, CustomerDto customerDto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                //customer.Name = customerDto.Name;
                //customer.Email = customerDto.Email;
                //await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<ICollection<OrderDto>> GetConfirmedOrdersAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                var ordersDto = await _context.Orders
                    .Include(o => o.OrderProducts)
                    .Where(o => o.CustomerId == id && o.IsConfirmed)
                    .Select(o => _mapper.Map<OrderDto>(o))
                    .ToListAsync();
                return ordersDto;
            }
            return null;
        }
        public async Task<ICollection<OrderDto>> GetPendingOrdersAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                var ordersDto = await _context.Orders
                    .Include(o => o.OrderProducts)
                    .Where(o => o.CustomerId == id && !o.IsConfirmed)
                    .Select(o => _mapper.Map<OrderDto>(o))
                    .ToListAsync();
                return ordersDto;
            }
            return null;
        }
        public async Task<ICollection<OrderDto>> GetAllOrdersAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                var ordersDto = await _context.Orders
                    .Include(o => o.OrderProducts)
                    .Where(o => o.CustomerId == id)
                    .Select(o => _mapper.Map<OrderDto>(o))
                    .ToListAsync();
                return ordersDto;
            }
            return null;
        }
    }
}
