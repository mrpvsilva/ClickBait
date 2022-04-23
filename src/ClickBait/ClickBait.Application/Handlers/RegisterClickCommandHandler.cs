using ClickBait.Application.Commands;
using ClickBait.Application.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClickBait.Application.Handlers
{
    public class RegisterClickCommandHandler : IRequestHandler<RegisterClickCommand>
    {
        private readonly IClickService _clickService;
        private readonly ILogger<RegisterClickCommandHandler> _logger;

        public RegisterClickCommandHandler(IClickService clickService, ILogger<RegisterClickCommandHandler> logger)
        {
            _clickService = clickService;
            _logger = logger;
        }

        public async Task<Unit> Handle(RegisterClickCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _clickService.Register(new Domain.Entities.Click
                {
                    PostId = request.PostId
                });

                _logger.LogInformation($"Click registered {request.PostId} successfully");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on register click {request.PostId}");
                throw;
            }
        }
    }
}
