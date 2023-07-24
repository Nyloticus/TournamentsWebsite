using Microsoft.AspNetCore.Mvc;
using MVC.Contracts;
using MVC.Models;

namespace MVC.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService _service;
        private readonly ILocalStorageService _localStorageService;

        public TeamController(ITeamService service, ILocalStorageService localStorageService)
        {
            _service = service;
            _localStorageService = localStorageService;
        }

        //user view 
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var Teams = await _service.GetAllTeams();
            return View(Teams);
        }
        //admin view
        [HttpGet]
        public async Task<IActionResult> AdminIndex()
        {
            var Teams = await _service.GetAllTeams();
            return View(Teams);
        }
        //create view
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        //AddTeam
        [HttpPost]
        public async Task<IActionResult> AddTeam(CreateTeamVm createTeamVm)
        {
            var response = await _service.AddTeam(createTeamVm);
            if (response.Success)
                return RedirectToAction("AdminIndex", "Team");

            // Failed to create team
            ModelState.AddModelError("string.Empty", response.Message);
            TempData["Message"] = response.Message;
            return RedirectToAction("Create", "Team");
        }
        //delete team
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _service.DeleteTeam(id);
            if (result.Success)
            {              
                return RedirectToAction("AdminIndex", "Team");
            }
            else
            {          
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("AdminIndex", "Team");
            }
        }

        //Edit view
        public async Task<IActionResult> Edit(string Id)
        {
            var response = await _service.GetTeamById(Id);
            return View(response);
        }

        //Edit Command
        [HttpPost]
        public async Task<IActionResult> EditTeam(TeamVM EditTeamVm)
        {
            var response = await _service.UpdateTeam(EditTeamVm);
            if (response.Success)
                return RedirectToAction("AdminIndex", "Team");

            // Failed to create team
            ModelState.AddModelError("string.Empty", response.Message);
            TempData["Message"] = response.Message;
            return RedirectToAction("Edit", "Team", new { Id = EditTeamVm.Id });
        }

        //unassigned teams
        [HttpGet]
        public async Task<IActionResult> UnassignedTeams(string tournamentId)
        {
            var Teams = await _service.GetUnassignedTeams(tournamentId);
            return View(Teams);
        }
    }
}
