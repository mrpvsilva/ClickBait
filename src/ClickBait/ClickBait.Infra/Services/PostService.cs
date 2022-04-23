using ClickBait.Application.Services;
using ClickBait.Domain.Entities;
using ClickBait.Domain.Repositories;
using System.Linq.Expressions;

namespace ClickBait.Infra.Services
{
    internal class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public Task<IEnumerable<Post>> Get(Expression<Func<Post, bool>> predicate)
        {
            return _postRepository.Get(predicate);
        }
    }
}
