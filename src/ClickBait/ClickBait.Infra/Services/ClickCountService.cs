using ClickBait.Application.Services;
using ClickBait.Domain.Entities;
using ClickBait.Domain.Repositories;
using System.Linq.Expressions;

namespace ClickBait.Infra.Services
{
    internal class ClickCountService : IClickCountService
    {
        private readonly IClickCountRepository _clickCountRepository;

        public ClickCountService(IClickCountRepository clickCountRepository)
        {
            _clickCountRepository = clickCountRepository;
        }
        public Task<IEnumerable<ClickCount>> Get(Expression<Func<ClickCount, bool>> expression)
        {
            return _clickCountRepository.Get(expression);
        }
    }
}
