using Common;
using Domain.Entities.Auth;
using Infrastructure.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Queries
{
    public class RolesDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Permissions { get; set; }
    }
    public class GetAppRolesQuery : IRequest<Result>
    {


        class Handler : IRequestHandler<GetAppRolesQuery, Result>
        {
            private readonly IAppDbContext _context;
            private readonly IPermissionService _permissionService;

            public Handler(IAppDbContext context, IPermissionService permissionService)
            {
                _context = context;
                _permissionService = permissionService;
            }
            public async Task<Result> Handle(GetAppRolesQuery request, CancellationToken cancellationToken)
            {

                var roles = _context.Set<Role>()
                         .Select(r => new { Id = r.Id, Name = r.Name })
                         .ToList();

                var rolesDto = new List<RolesDto>();
                foreach (var role in roles)
                {
                    var roleDto = new RolesDto
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Permissions = _permissionService.GetRolePermissions(role.Id)
                    };
                    rolesDto.Add(roleDto);
                }

                return Result.Successed(rolesDto);
            }
        }
    }

}
