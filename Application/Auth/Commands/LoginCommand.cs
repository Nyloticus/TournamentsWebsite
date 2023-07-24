//using Application.Notifications;
using Common;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands
{
    public class LoginCommand : IRequest<Result>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DeviceToken { get; set; }

        class Handler : IRequestHandler<LoginCommand, Result>
        {
            private readonly IIdentityService _identityService;
            private readonly IAppDbContext _context;
            private readonly IMediator _mediator;

            public Handler(IIdentityService identityService, IAppDbContext context, IMediator mediator)
            {
                _identityService = identityService;
                _context = context;
                _mediator = mediator;
            }

            public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var result = await _identityService.LoginAsync(request.Username, request.Password, request.DeviceToken);
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == request.Username.ToLower(), cancellationToken);

                //if (result.Success &&!string.IsNullOrEmpty(request.DeviceToken)) {
                //  await _mediator.Publish(new DeviceTokenNotification() {
                //    Token = request.DeviceToken,
                //    UserId = user.Id
                //  },cancellationToken);
                //}
                return result;
            }
        }
    }
}