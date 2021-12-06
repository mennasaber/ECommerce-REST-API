using AutoMapper;
using FirstCore.Data;
using FirstCore.Data.Dtos;
using FirstCore.Data.Models;
using FirstCore.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCore.Repos
{
    public class OwnerRepo : IOwnerRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public OwnerRepo(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OwnerDto> AddAsync(OwnerDto ownerDto)
        {
            var owner = _mapper.Map<Owner>(ownerDto);
            await _context.Owners.AddAsync(owner);
            await _context.SaveChangesAsync();
            ownerDto = _mapper.Map<OwnerDto>(owner);
            return ownerDto;
        }
    }
}
