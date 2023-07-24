using Common;
using FluentValidation;
using Infrastructure;
using Infrastructure.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands
{
    public class RegisterCommand : IRequest<Result>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string userName { get; set; }
        public string Password { get; set; }
        public string email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> RolesIds { get; set; }


        public class Validator : AbstractValidator<RegisterCommand>
        {
            public Validator()
            {
                RuleFor(x => x.FirstName).NotEmpty()
                .WithMessage("First Name is required");
                RuleFor(x => x.LastName).NotEmpty()
                    .WithMessage("Last Name is required");
                RuleFor(r => r.userName).NotEmpty()
                   .WithMessage("Name is Required");
                RuleFor(r => r.Password).NotEmpty()
                    .WithMessage("Password is required");
            }
        }
        public class Handler : IRequestHandler<RegisterCommand, Result>
        {
            private readonly IIdentityService _identityService;
            private readonly IAppDbContext _context;
            public Handler(IIdentityService identityService, IAppDbContext context)
            {
                _identityService = identityService;
                _context = context;
            }

            public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {

                var result = await _identityService
                    .RegisterAsync(request.FirstName, request.LastName, request.email, request.PhoneNumber, request.userName, request.Password, request.RolesIds);
                return result;

            }
        }
    }
}
