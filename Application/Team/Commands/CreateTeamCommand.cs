using Common;
using Common.Interfaces;
using Domain.Repositories;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Team.Commands
{
    public class CreateTeamCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string OfficialWebsiteURL { get; set; }
        public IFormFile Logo { get; set; }
        public DateTime FoundationDate { get; set; }

        public class Validator : AbstractValidator<CreateTeamCommand>
        {
            public Validator()
            {
                RuleFor(r => r.Name).NotEmpty()
                    .WithMessage("Team Name is required");
                RuleFor(r => r.Description).NotEmpty()
                    .WithMessage("Description");
                RuleFor(r => r.Logo).NotNull()
                    .WithMessage("Logo is required");
                RuleFor(r => r.Logo.FileName)
                  .Must(r => r.ToLower().EndsWith(".jpg") || r.ToLower().EndsWith(".png") || r.ToLower().EndsWith(".jpeg"))
                  .WithMessage("only jpg, jpeg and png files are allowed")
                  .When(r => r.Logo != null);
                RuleFor(r => r.OfficialWebsiteURL).NotNull()
                    .WithMessage("Website URL is required");
            }
        }
        public class Handler : IRequestHandler<CreateTeamCommand, Result>
        {
            private readonly ITeamRepository _teamRepository;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly IUrlHelper _urlHelper;
            public Handler(ITeamRepository teamRepository, IWebHostEnvironment webHostEnvironment, IUrlHelper urlHelper)
            {
                _teamRepository = teamRepository;
                _webHostEnvironment = webHostEnvironment;
                _urlHelper = urlHelper;
            }

            public async Task<Result> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
            {
                var Team = request.Adapt<Domain.Entities.Models.Team>();

                //add logo 
                var Foldername = "TeamLogos";
                var Filename = request.Logo.FileName;

                var path = Path.Combine(_webHostEnvironment.WebRootPath, Foldername, Filename);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await request.Logo.CopyToAsync(stream, cancellationToken);
                }

                //Generate url
                var Location = _urlHelper.GenerateUrl(Foldername, Filename);
                if (Location == null)
                    return Result.Failure(ApiExeptionType.ValidationError, "Failed adding logo");

                Team.LogoURL = Location.AbsoluteUri;
                Team.LogoPath = Location.AbsolutePath;

                var result = await _teamRepository.AddAsync(Team);

                return Result.Successed("Created");
            }
        }

    }
}
