using Common;
using Common.Interfaces;
using Domain.Repositories;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tournament.Commands
{
    public class UpdateTournamentCommand : IRequest<Result>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Logo { get; set; }
        public string TournamentVideo { get; set; }

        public class Validator : AbstractValidator<UpdateTournamentCommand>
        {
            public Validator()
            {
                RuleFor(r => r.Id).NotEmpty()
                    .WithMessage("Tournament Id Is Required");
                RuleFor(r => r.Name).NotEmpty()
                    .WithMessage("Tournament Name is required");
                RuleFor(r => r.Description).NotEmpty()
                    .WithMessage("Description");
                RuleFor(r => r.Logo).NotNull()
                    .WithMessage("Logo is required");
                RuleFor(r => r.Logo.FileName)
                  .Must(r => r.ToLower().EndsWith(".jpg") || r.ToLower().EndsWith(".png") || r.ToLower().EndsWith(".jpeg"))
                  .WithMessage("only jpg, jpeg and png files are allowed")
                  .When(r => r.Logo != null);

            }
        }
        public class Handler : IRequestHandler<UpdateTournamentCommand, Result>
        {
            private readonly ITournamentRepository _tournamentRepository;
            private readonly IWebHostEnvironment _webHostEnvironment;
            private readonly IUrlHelper _urlHelper;
            public Handler(ITournamentRepository tournamentRepository, IWebHostEnvironment webHostEnvironment, IUrlHelper urlHelper)
            {
                _tournamentRepository = tournamentRepository;
                _webHostEnvironment = webHostEnvironment;
                _urlHelper = urlHelper;
            }

            public async Task<Result> Handle(UpdateTournamentCommand request, CancellationToken cancellationToken)
            {
                var Tournament = await _tournamentRepository.GetByIdAsync(request.Id);
                if (!Tournament.Success)
                    return Tournament;

                var tTournament = request.Adapt(Tournament.Payload);

                //update logo 
                var Foldername = "TournamentLogos";
                var Filename = request.Logo.FileName;

                //delete old file
                var oldpath = Tournament.Payload.LogoPath;
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

                tTournament.LogoURL = Location.AbsoluteUri;
                tTournament.LogoPath = Location.AbsolutePath;


                var result = await _tournamentRepository.UpdateAsync(tTournament);
                return Result.Successed("Updated Successfully");

            }
        }
    }
}
