using CarRental.Common.Messages;
using CarRental.Services.Payment.Models;
using CarRental.Shared.CustomController;
using CarRental.Shared.Dtos;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Services.Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : CustomBaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public PaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentDto paymentDto)
        {
            var send = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:order-service"));

            var orderMessageCommand = new CreateOrderMessageCommand();
            orderMessageCommand.RenterId = paymentDto.Order.RenterId;
            orderMessageCommand.Provience = paymentDto.Order.Address.Provience;
            orderMessageCommand.District = paymentDto.Order.Address.District;
            orderMessageCommand.ZipCode = paymentDto.Order.Address.ZipCode;
            orderMessageCommand.Street = paymentDto.Order.Address.Street;
            orderMessageCommand.Line = paymentDto.Order.Address.Line;

            paymentDto.Order.OrderItems.ForEach(
                x =>
                {
                    orderMessageCommand.OrderItems.Add(new OrderItem
                    {
                        PictureUrl = x.PictureUrl,
                        Price = x.Price,
                        ProductId = x.ProductId,
                        ProductName = x.ProductName,
                        LeaveDate = x.LeaveDate

                    });
                }

                );

            await send.Send<CreateOrderMessageCommand>(orderMessageCommand);  
            return CreateActionResultInstance(ResponseDto<NoContent>.Success(200));
        }
    }
}
