using CarRental.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using UserInterface.MicroserviceCommunication.Abstract;
using UserInterface.Models.Orders;

namespace UserInterface.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        private readonly ISharedIdentityService _sharedIdentityService;
        

        public OrderController(IBasketService basketService, IOrderService orderService,ISharedIdentityService identity)
        {
            _basketService = basketService;
            _orderService = orderService;
            _sharedIdentityService = identity;  
        }

        public async Task<IActionResult >PayPage()
        {
            var basket = await _basketService.GetBasket();
            ViewBag.basket = basket;
            
            return View(new CheckOutModel());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckOutModel checkOut)
        {
            var orderStatus = await _orderService.QueueOrder(checkOut);
            if (!orderStatus.IsSuccessful)
            {
                var basket = await _basketService.GetBasket();
                ViewBag.basket = basket;
                ViewBag.error = orderStatus.Error;
                return View();

            }
            return RedirectToAction(nameof(SuccessfulOut), new { orderId = new Random().Next(1,10000) });

        }

        public IActionResult SuccessfulOut(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
        public async Task<IActionResult >OrderHistory()
        {

            return View(await _orderService.GetOrder());
        }
    }
}
