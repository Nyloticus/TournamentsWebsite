using Common;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Queries
{
    public class GetAllPermissionQuery : IRequest<Result>
    {

        class Handler : IRequestHandler<GetAllPermissionQuery, Result>
        {
            private readonly IPermissionService _service;
            private readonly IAppDbContext _context;
            private readonly IAuditService _auditService;

            public Handler(IPermissionService service, IAuditService auditService, IAppDbContext context)
            {
                _service = service;
                _context = context;
                _auditService = auditService;
            }
            public async Task<Result> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
            {
                if (string.IsNullOrEmpty(_auditService.TenantId))
                    return Result.Successed(_service.GetGroupedPermissions());


                var user = await _context.Users
                    .FirstOrDefaultAsync(e => e.Id == _auditService.UserId, cancellationToken);

                var modules = user.Modules;

                return Result.Successed(_service.GetModulesPermissions(modules));
            }
        }
    }

}
