using Microsoft.AspNetCore.Mvc;
using MVC.Contracts;
using MVC.Models;

namespace MVC.Controllers
{
    public class TournamentController : Controller
    {
        private readonly ITournamentService _service;


        public TournamentController(ITournamentService service)
        {
            _service = service;
        }

        //user view 
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var Tournaments = await _service.GetAllTournaments();
            return View(Tournaments);
        }
        //admin view
        [HttpGet]
        public async Task<IActionResult> AdminIndex()
        {
            var Tournaments = await _service.GetAllTournaments();
            return View(Tournaments);
        }
        //create view
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        //AddTournament
        [HttpPost]
        public async Task<IActionResult> AddTournament(CreateTournamentVm createTournamentVm)
        {
            var response = await _service.AddTournament(createTournamentVm);
            if (response.Success)
                return RedirectToAction("AdminIndex", "Tournament");

            // Failed to create tournament
            ModelState.AddModelError("string.Empty", response.Message);
            TempData["Message"] = response.Message;
            return RedirectToAction("Create", "Tournament");
        }
        //delete Tournament
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _service.DeleteTournament(id);

            if (result.Success)
            {
                return RedirectToAction("AdminIndex", "Tournament");
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("AdminIndex", "Tournament");
            }
        }

        //Edit view
        public async Task<IActionResult> Edit(string Id)
        {
            var response = await _service.GetTournamentById(Id);
            return View(response);
        }

        //Edit Command
        [HttpPost]
        public async Task<IActionResult> EditTournament(TournamentVM EditTournamentVm)
        {
            var response = await _service.UpdateTournament(EditTournamentVm);
            if (response.Success)
                return RedirectToAction("AdminIndex", "Tournament");

            // Failed to create tournament
            ModelState.AddModelError("string.Empty", response.Message);
            TempData["Message"] = response.Message;
            return RedirectToAction("Edit", "Tournament", new { Id = EditTournamentVm.Id });
        }
        //show vid
        public async Task<IActionResult> ShowVideo(string Id)
        {
            var response = await _service.GetTournamentById(Id);
            var videoId = _service.GetYouTubeVideoId(response.TournamentVideo);
            var embedUrl = "https://www.youtube.com/embed/" + videoId;
            ViewBag.EmbedUrl = embedUrl;
            return View();
        }
    }
}
