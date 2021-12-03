using FirstCore.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCore.IRepos
{
    public interface IOrdersRepo : IBaseRepo<OrderDto>
    {
        Task<bool> UpdateAsync(int id);
    }
}
