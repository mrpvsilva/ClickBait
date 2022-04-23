using ClickBait.Domain.Entities;
using System.Linq.Expressions;

namespace ClickBait.Application.Services
{
    public interface IPostService : IService
    {
        Task<IEnumerable<Post>> Get(Expression<Func<Post, bool>> predicate);
    }
}
