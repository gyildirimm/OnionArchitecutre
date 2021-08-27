using Application.Core.Wrappers;
using DTO.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task<IResponse<int>> Create(CreateOrderDto orderModel);
        Task<IResponse<List<OrderDto>>> ListAllOrderAsync();
        IResponse<List<OrderDto>> ListAllOrder();
    }
}
