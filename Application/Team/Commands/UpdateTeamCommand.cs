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
    public class UpdateTeamCommand : IRequest<Result>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OfficialWebsiteURL { get; set; }
        public IFormFile Logo { get; set; }
        public DateTime FoundationDate { get; set; }

        public class Validator : AbstractValidator<UpdateTeamCommand>
        {
            public Validator()
            {
                RuleFor(r => r.Id).NotEmpty()
                    .WithMessage("Team Id Is Required");
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
        public class Handler : IRequestHandler<UpdateTeamCommand, Result>
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

            public async Task<Result> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
            {
                var Team = await _teamRepository.GetByIdAsync(request.Id);
                if (!Team.Success)
                    return Team;

                var tTeam = request.Adapt(Team.Payload);

                //update logo 
                var Foldername = "TeamLogos";
                var Filename = request.Logo.FileName;

                //delete old file
                var oldpath = Team.Payload.LogoPath;
                if (oldpath != null)
                {
                    FileInfo file = new FileInfo(oldpath);
                    if (file.Exists) file.Delete();
                }

                var path = Path.Combine(_webHostEnvironment.WebRootPath, Foldername, Filename);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await request.Logo.CopyToAsync(stream, cancellationToken);
                }

                //Generate url
                var Location = _urlHelper.GenerateUrl(Foldername, Filename);
                if (Location == null)
                    return Result.Failure(ApiExeptionType.ValidationError, "Failed adding logo");

                tTeam.LogoURL = Location.AbsoluteUri;
                tTeam.LogoPath = Location.AbsolutePath;


                var result = await _teamRepository.UpdateAsync(tTeam);
                return Result.Successed("Updated Successfully");

            }
        }
    }
}
