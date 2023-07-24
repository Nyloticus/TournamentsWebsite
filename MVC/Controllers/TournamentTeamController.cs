using Microsoft.AspNetCore.Mvc;
using MVC.Contracts;
using MVC.Models;

namespace MVC.Controllers
{
    public class TournamentTeamController : Controller
    {
        private readonly ITournamentTeamService _service;
        private readonly ITournamentService _tournamentService;
        private readonly ITeamService _teamService;
        private readonly IConfiguration _configuration;
        private readonly ILocalStorageService _localStorageService;
        public TournamentTeamController(ITournamentTeamService service, ITournamentService tournamentService,
            ITeamService teamService, IConfiguration configuration, ILocalStorageService localStorageService)
        {
            _service = service;
            _tournamentService = tournamentService;
            _teamService = teamService;
            _configuration = configuration;
            _localStorageService = localStorageService;
        }
        //create view
        [HttpGet]
        public async Task<IActionResult> CreateView()
        {
            var tournaments = await _tournamentService.GetAllTournaments();
            //var teams = await _teamService.GetAllTeams();
            var teams = await _teamService.GetUnassignedTeams(tournaments?.FirstOrDefault().Id);
            var baseUrl = _configuration.GetSection("BaseUrl").Value;
            ViewBag.BaseUrl = baseUrl;
            ViewBag.Token = _localStorageService.GetStorageValue<string>("token");
            var model = new TournamentTeamView
            {
                Tournaments = tournaments,
                Teams = teams
            };
            return View(model);
        }
        //create command
        [HttpPost]
        public async Task<IActionResult> Assign(AssignTournamentTeamVm createTournamentTeamVm)
        {
            var response = await _service.AssignTournamentTeam(createTournamentTeamVm);
            if (response.Success)
                return RedirectToAction("AdminIndex", "Login");

            // Failed to create tournament
            ModelState.AddModelError("", response.Message);
            return View();
        }
        //view all
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _service.GetAllTournamentTeams();
            return View(response);
        }
        //view all
        [HttpGet]
        public async Task<IActionResult> AdminIndex()
        {
            var response = await _service.GetAllTournamentTeams();
            return View(response);
        }
        //remove assign
        [HttpPost]
        public async Task<IActionResult> RemoveAssign(AssignTournamentTeamVm removeTournamentTeamVm)
        {
            var response = await _service.RemoveTournamentTeam(removeTournamentTeamVm);
            if (response.Success)
                return RedirectToAction("AdminIndex", "TournamentTeam");

            ModelState.AddModelError("", response.Message);
            return View();
        }
    }
}
