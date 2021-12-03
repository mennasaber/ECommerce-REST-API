using AutoMapper;
using FirstCore.Data;
using FirstCore.IRepos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCore.Repos
{
    public class BaseRepo<TEntity, TDto> : IBaseRepo<TDto> where TEntity : class where TDto : class
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public BaseRepo(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<TDto> AddAsync(TDto entityDto)
        {
            var entity = _mapper.Map<TEntity>(entityDto);
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            entityDto = _mapper.Map<TDto>(entity);
            return entityDto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public virtual async Task<TDto> FindAsync(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity != null)
            {
                var entityDto = _mapper.Map<TDto>(entity);
                return entityDto;
            }
            return null;
        }

        public virtual async Task<ICollection<TDto>> GetAllAsync()
        {
            var entities = await _context.Set<TEntity>().ToListAsync();
            var entitiesDto = entities.Select(e => _mapper.Map<TDto>(e)).ToList();
            return entitiesDto;
        }
    }
}
