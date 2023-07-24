using Common;
using Infrastructure;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Queries
{
    public class GetActiveUsersQuery : IRequest<Result>
    {

        class Handler : IRequestHandler<GetActiveUsersQuery, Result>
        {
            private readonly IIdentityService _identityService;

            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }
            public async Task<Result> Handle(GetActiveUsersQuery request, CancellationToken cancellationToken)
            {

                var activeUsers = await _identityService.GetActiveUsers();
                return Result.Successed(activeUsers);
            }
        }
    }

}
