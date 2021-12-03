using AutoMapper;
using FirstCore.Data;
using FirstCore.Data.Dtos;
using FirstCore.Data.Models;
using FirstCore.IRepos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCore.Repos
{
    public class OrdersRepo : BaseRepo<Order, OrderDto>, IOrdersRepo
    {
        private readonly AppDbContext _context = null;
        private readonly IMapper _mapper;
        public OrdersRepo(AppDbContext context, IMapper mapper) : base(mapper, context)
        {
            _context = context;
            _mapper = mapper;
        }
        public override async Task<OrderDto> FindAsync(int id)
        {
            var order = await _context.Orders.Include(o=>o.OrderProducts).SingleOrDefaultAsync(o=>o.Id==id);
            if (order != null)
            {
                var orderDto = _mapper.Map<OrderDto>(order);
                return orderDto;
            }
            return null;
        }
        public override async Task<ICollection<OrderDto>> GetAllAsync()
        {
            var orders = await _context.Orders.Include(o => o.OrderProducts).ToListAsync();
            var ordersDto = orders.Select(o => _mapper.Map<OrderDto>(o)).ToList();
            return ordersDto;
        }
        public async Task<bool> UpdateAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.IsConfirmed = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
