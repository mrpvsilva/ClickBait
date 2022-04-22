using MediatR;

namespace ClickBait.Application.Commands
{
    public class RegisterClickCommand : IRequest
    {
        public Guid PostId { get; private set; }

        public RegisterClickCommand(Guid postId)
        {
            PostId = postId;
        }
    }
}
