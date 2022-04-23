using ClickBait.Domain.Entities;
using System.Linq.Expressions;

namespace ClickBait.Domain.Repositories
{
    public interface IClickCountRepository : IRepository<ClickCount>
    {
        Task<IEnumerable<ClickCount>> Get(Expression<Func<ClickCount, bool>> expression);
    }
}
