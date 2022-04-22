using ClickBait.Application.Commands;
using ClickBait.Application.Services;
using MediatR;

namespace ClickBait.Application.Handlers
{
    public class RegisterClickCommandHandler : IRequestHandler<RegisterClickCommand>
    {
        private readonly IClickService _clickService;

        public RegisterClickCommandHandler(IClickService clickService)
        {
            _clickService = clickService;
        }

        public async Task<Unit> Handle(RegisterClickCommand request, CancellationToken cancellationToken)
        {
            await _clickService.Register(new Domain.Entities.Click
            {
                PostId = request.PostId
            });

            return Unit.Value;
        }
    }
}
