using CarRental.Services.Order.Application.Commands;
using CarRental.Services.Order.Application.DTOs;
using CarRental.Services.Order.Domain.OrderAggregate;
using CarRental.Services.Order.Infrastructure;
using CarRental.Shared.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Services.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ResponseDto<CreatedOrderDto>>
    {
        private readonly OrderDbContext _context;

        public CreateOrderCommandHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDto<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newAddress = new Address(request.AddressDto.Provience, request.AddressDto.District, request.AddressDto.Street, request.AddressDto.ZipCode, request.AddressDto.Line);
            Domain.OrderAggregate.Order newOrder = new Domain.OrderAggregate.Order(request.RenterId, newAddress);
            request.OrderItems.ForEach(x =>
            {

                newOrder.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl,x.LeaveDate);
            });
            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();
            return ResponseDto<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId = newOrder.Id }, 200);
        }
    }
}
