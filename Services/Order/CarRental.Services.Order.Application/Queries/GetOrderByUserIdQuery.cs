using CarRental.Services.Order.Application.DTOs;
using CarRental.Shared.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Services.Order.Application.Queries
{
    public class GetOrderByUserIdQuery:IRequest<ResponseDto<List<OrderDto>>>
    {
        public string? UserId { get; set; }
    }
}
