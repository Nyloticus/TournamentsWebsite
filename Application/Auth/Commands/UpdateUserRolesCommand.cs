using Common;
using FluentValidation;
using Infrastructure;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands
{
    public class UpdateUserRolesCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public List<string> RolesIds { get; set; }


        public class Validator : AbstractValidator<UpdateUserRolesCommand>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty()
                    .WithMessage("UserId is required");

            }
        }
        public class Handler : IRequestHandler<UpdateUserRolesCommand, Result>
        {
            private readonly IIdentityService _identityService;
            public Handler(IIdentityService identityService)
            {
                _identityService = identityService;
            }

            public async Task<Result> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
            {
                var result = await _identityService.UpdateUserRoles(request.UserId, request.RolesIds);
                return result;
            }
        }
    }
}
