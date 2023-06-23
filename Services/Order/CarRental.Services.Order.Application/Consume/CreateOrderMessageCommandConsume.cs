using CarRental.Common.Messages;
using CarRental.Services.Order.Infrastructure;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Services.Order.Application.Consume
{
    public class CreateOrderMessageCommandConsume : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderDbContext _context;

        public CreateOrderMessageCommandConsume(OrderDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            var newAdress = new Domain.OrderAggregate.Address(context.Message.Provience, context.Message.District, context.Message.Street, context.Message.ZipCode, context.Message.Line);
            Domain.OrderAggregate.Order order = new Domain.OrderAggregate.Order(context.Message.RenterId, newAdress);
            context.Message.OrderItems.ForEach(x =>
            {
                order.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl, x.LeaveDate);

            });
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();  
        }
    }
}
