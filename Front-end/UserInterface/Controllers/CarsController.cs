using CarRental.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.Models.CatalogModels;

namespace UserInterface.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CarsController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _catalogService.GetAllCarsByUserId(_sharedIdentityService.GetUserId));
        }
        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
            return View();
        }
        public async Task<IActionResult> CurrentlyRentedCars()
        {
            var userId = _sharedIdentityService.GetUserId;
            var response = await _catalogService.GetCurrentlyRentedCars(userId);
            return View(response);
        }

        public async Task<IActionResult> GetCarsBasedOnPassive()
        {
            var userId = _sharedIdentityService.GetUserId;
            var response = await _catalogService.GetAllPassiveCarsBasedOnUserId(userId);
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CarCreate carCreate)
        {
            var categories = await _catalogService.GetAllCategoryAsync();

            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
            carCreate.UserId = _sharedIdentityService.GetUserId;
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }

            await _catalogService.AddCarAsync(carCreate);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(string id)
        {
            var cars = await _catalogService.GetByCarId(id);
            var categories = await _catalogService.GetAllCategoryAsync();

            if (cars == null)
            {
                RedirectToAction(nameof(Index));
            }
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
            CarUpdate carUpdate = new CarUpdate
            {
                Id = cars.Id,
                Name = cars.Name,
                UserId = cars.UserId,
                Price = cars.Price,
                Description = cars.Description,
                Feature = cars.Feature != null && cars.Feature.Duration != null ? new FeatureViewModel { Duration = cars.Feature.Duration } : null,


                CategoryId = cars.CategoryId,
                Picture = cars.Picture,
                Location = cars.Location

            };

            return View(carUpdate);

        }
        [HttpPost]
        public async Task<IActionResult> Update(CarUpdate carUpdate)
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", carUpdate.Id);
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _catalogService.UpdateCarAsync(carUpdate);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string id)
        {
            await _catalogService.DeleteCarAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
