using ClickBait.Domain.Entities;
using System.Linq.Expressions;

namespace ClickBait.Application.Services
{
    public interface IClickService : IService
    {
        Task<Click> Register(Click click);
    }
}
