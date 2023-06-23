using CarRental.Services.Order.Application.DTOs;
using CarRental.Services.Order.Application.Mappings;
using CarRental.Services.Order.Application.Queries;
using CarRental.Services.Order.Infrastructure;
using CarRental.Shared.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Services.Order.Application.Handlers
{
    internal class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrderByUserIdQuery, ResponseDto<List<OrderDto>>>
    {
        private readonly OrderDbContext _context;

        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDto<List<OrderDto>>> Handle(GetOrderByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(x => x.OrderItems).Where(x => x.RenterId == request.UserId).ToListAsync();

            if (!orders.Any())
            {
                return ResponseDto<List<OrderDto>>.Success(new List<OrderDto>(), 200);
            }

            var ordersDto = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);

            return ResponseDto<List<OrderDto>>.Success(ordersDto, 200);
        }
    }
}