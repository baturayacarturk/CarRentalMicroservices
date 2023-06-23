using CarRental.Shared.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UserInterface.Exceptions;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;



        public HomeController(ILogger<HomeController> logger, ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _logger = logger;
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<IActionResult> Index(string selectedLocation)
        {
            if (string.IsNullOrEmpty(selectedLocation))
            {
                return RedirectToAction("LocationList");
            }
            var response = await _catalogService.GetAllCarsBasedOnLocation(selectedLocation);
            try
            {
                var userId = _sharedIdentityService.GetUserId;
            }
            catch
            {

                return View(response);
            }


            var unWrap = response.Where(x => x.UserId != _sharedIdentityService.GetUserId).ToList();
            return View(unWrap);

        }
        public async Task<IActionResult> LocationList()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Details(string id)
        {
            return View(await _catalogService.GetByCarId(id));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errors = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (errors != null && errors.Error is UnauthorizedException)
            {
                return RedirectToAction(nameof(AuthenticationController.Logout), "Authentication");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}