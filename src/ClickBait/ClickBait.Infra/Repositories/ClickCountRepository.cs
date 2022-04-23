using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClickBait.Domain.Entities;
using ClickBait.Domain.Repositories;
using ClickBait.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClickBait.Infra.Repositories
{
    internal class ClickCountRepository : IClickCountRepository
    {
        private readonly ClickBaitContext _ctx;
        private readonly IMapper _mapper;

        public ClickCountRepository(ClickBaitContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClickCount>> Get(Expression<Func<ClickCount, bool>> expression)
        {
            return await _ctx.ClicksCounts
                    .ProjectTo<ClickCount>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .Where(expression)
                    .ToListAsync();
        }
    }
}
