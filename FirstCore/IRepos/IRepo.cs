using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCore.IRepos
{
    public interface IRepo<TDto>:IBaseRepo<TDto> where TDto : class
    {
        Task<bool> UpdateAsync(int id, TDto entityDto);
    }
}
