using Application.Core.Wrappers;
using Domain.OrderAggregate;
using Domain.Shared.Repositories;
using DTO.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IOrderRepository : IEntityRepository<Order>
    {
        Task<IResponse<int>> CreateOrder(CreateOrderDto orderModel);
    }
}
