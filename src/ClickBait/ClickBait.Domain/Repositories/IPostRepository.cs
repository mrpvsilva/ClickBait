using ClickBait.Domain.Entities;
using System.Linq.Expressions;

namespace ClickBait.Domain.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> Get(Expression<Func<Post, bool>> predicate);
    }
}
