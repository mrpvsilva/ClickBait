using ClickBait.Domain.Entities;

namespace ClickBait.Application.Services
{
    public interface IClickService
    {
        Task<Click> Register(Click click);
    }
}
