using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCore.IRepos
{
    public interface IBaseRepo<TDto> where TDto : class
    {
        Task<ICollection<TDto>> GetAllAsync();
        Task<TDto> FindAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<TDto> AddAsync(TDto entityDto);
    }
}
