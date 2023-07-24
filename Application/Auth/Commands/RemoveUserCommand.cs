using Common;
using FluentValidation;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands
{
    public class RemoveUserCommand : IRequest<Result>
    {
        public string UserId { get; set; }


        public class Validator : AbstractValidator<RemoveUserCommand>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty()
                .WithMessage("User Id is required");
            }
        }
        public class Handler : IRequestHandler<RemoveUserCommand, Result>
        {
            private readonly IIdentityService _identityService;
            private readonly IAppDbContext _context;
            private readonly IAuditService _auditService;
            public Handler(IIdentityService identityService, IAppDbContext context, IAuditService auditService)
            {
                _identityService = identityService;
                _context = context;
                _auditService = auditService;
            }

            public async Task<Result> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
            {
                if (request.UserId == _auditService.UserId)
                    return Result.Failure(ApiExeptionType.BadRequest, "can't remove the current logged user");

                var result = await _identityService.RemoveUserAsync(request.UserId);
                return result;

            }
        }
    }
}
