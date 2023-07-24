using Common;
using Common.Attributes;
using FluentValidation;
using Infrastructure.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands
{
    public class GetPermissionsCommand : IRequest<Result>
    {
        public class Validator : AbstractValidator<GetPermissionsCommand>
        {
            public Validator()
            {

            }
        }
        public class Handler : IRequestHandler<GetPermissionsCommand, Result>
        {
            private readonly IPermissionService _permissionService;
            private readonly IAppDbContext _context;
            public Handler(IPermissionService permissionService, IAppDbContext context)
            {
                _permissionService = permissionService;
                _context = context;
            }

            public async Task<Result> Handle(GetPermissionsCommand request, CancellationToken cancellationToken)
            {
                var result = _permissionService.GetPermissions(SystemModule.UserManagementModule);
                return Result.Successed(result);

            }
        }
    }
}
