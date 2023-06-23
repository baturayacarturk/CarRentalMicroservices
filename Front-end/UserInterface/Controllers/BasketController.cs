using CarRental.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.Models.Baskets;
using UserInterface.Models.CatalogModels;
using UserInterface.Models.Discount;

namespace UserInterface.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BasketController(ICatalogService catalogService, IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService; 
        }

        public async Task<IActionResult> Index()
        {
            return View(await _basketService.GetBasket());
        }

        public async Task<IActionResult> ApplyDiscount(DiscountViewModel discountViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["discountError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).First();
            }
            var status = await _basketService.ApplyDiscount(discountViewModel.Code);
            TempData["status"] = status;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CancelAppliedDiscount()
        {
            await _basketService.CancelTheDiscount();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> AddBasketItems(string carId, DateTime rentDate,DateTime leaveDate)
        {
            var car = await _catalogService.GetByCarId(carId);
            TimeSpan duration = leaveDate - rentDate;
            var days = duration.Days;
            var basketItem = new BasketItemViewModel
            {
                UserId=_sharedIdentityService.GetUserId,
                CarId = car.Id,
                CarName = car.Name,
                Price = car.Price*days,
                RentDate=rentDate,
                LeaveDate=leaveDate,    

            };

            await _basketService.AddBasketItem(basketItem);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteBasketItems(string carId)
        {
            await _basketService.RemoveBasketItem(carId);
            return RedirectToAction(nameof(Index));
        }
    }
}
