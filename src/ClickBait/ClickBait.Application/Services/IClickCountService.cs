using ClickBait.Domain.Entities;
using System.Linq.Expressions;

namespace ClickBait.Application.Services
{
    public interface IClickCountService : IService
    {
        Task<IEnumerable<ClickCount>> Get(Expression<Func<ClickCount, bool>> expression);
    }
}
