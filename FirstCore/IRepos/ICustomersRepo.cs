using FirstCore.Data.Dtos;
using FirstCore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCore.IRepos
{
    public interface ICustomersRepo : IRepo<CustomerDto>
    {
        Task<ICollection<OrderDto>> GetAllOrdersAsync(int id);
        Task<ICollection<OrderDto>> GetConfirmedOrdersAsync(int id);
        Task<ICollection<OrderDto>> GetPendingOrdersAsync(int id);
    }
}
