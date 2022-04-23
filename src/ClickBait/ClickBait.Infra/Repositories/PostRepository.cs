using ClickBait.Domain.Entities;
using ClickBait.Domain.Repositories;
using ClickBait.Infra.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ClickBait.Infra.Repositories
{
    internal class PostRepository : IPostRepository
    {
        private readonly ClickBaitContext _ctx;
        private readonly IMapper _mapper;

        public PostRepository(ClickBaitContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Post>> Get(Expression<Func<Post, bool>> predicate)
        {
            return await _ctx.Posts
                    .ProjectTo<Post>(_mapper.ConfigurationProvider)
                    .AsNoTracking()
                    .Where(predicate)
                    .ToListAsync();
        }
    }
}
