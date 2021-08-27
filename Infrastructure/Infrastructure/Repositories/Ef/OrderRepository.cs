using Application.Core.Wrappers;
using Domain.OrderAggregate;
using Domain.Repositories;
using DTO.Dtos;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Ef
{
    public class OrderRepository : GenericEntityRepository<Order, OrderDbContext>, IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context) : base (context)
        {
            _context = context;
        }

        public async Task<IResponse<int>> CreateOrder(CreateOrderDto orderModel)
        {
                var newAddress = new Address(orderModel.Address.Province, orderModel.Address.District, orderModel.Address.Street, orderModel.Address.ZipCode, orderModel.Address.Line);

                Domain.OrderAggregate.Order newOrder = new Domain.OrderAggregate.Order(orderModel.BuyerId, newAddress);

                orderModel.OrderItems.ForEach(x =>
                {
                    newOrder.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl);
                });

                await _context.Orders.AddAsync(newOrder);

                await _context.SaveChangesAsync();

                return Response<int>.Success(newOrder.Id, "Sipariş Oluşturuldu");
        }
    }
}
