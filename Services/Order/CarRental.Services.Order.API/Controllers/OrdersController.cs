using CarRental.Services.Order.Application.Commands;
using CarRental.Services.Order.Application.Queries;
using CarRental.Shared.CustomController;
using CarRental.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Services.Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrdersController(ISharedIdentityService sharedIdentityService, IMediator mediator)
        {
            _sharedIdentityService = sharedIdentityService;

            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _mediator.Send(new GetOrderByUserIdQuery { UserId = _sharedIdentityService.GetUserId });
            return CreateActionResultInstance(response);
        }
        [HttpPost]
        public async Task<IActionResult> SaveOrder(CreateOrderCommand request)
        {
            
            var response = await _mediator.Send(request);
            return CreateActionResultInstance(response);
        }
    }
}
