using Application.Core.Aspects.Autofac;
using Application.Core.CrossCuttingConcerns;
using Application.Core.CrossCuttingConcerns.Redis;
using Application.Core.Wrappers;
using Application.Interfaces;
using Domain.Mapping;
using Domain.Repositories;
using DTO.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICacheService _cacheService;
        public OrderService(IOrderRepository orderRepository, ICacheService cacheService)
        {
            _orderRepository = orderRepository;
            _cacheService = cacheService;
        }

        public Task<IResponse<int>> Create(CreateOrderDto orderModel)
        {
            return _orderRepository.CreateOrder(orderModel);
        }

        //[CacheAspect]
        public IResponse<List<OrderDto>> ListAllOrder()
        {
            var orderListResponse = _orderRepository.GetAll();
            var dtoList = ObjectMapper.Mapper.Map<List<OrderDto>>(orderListResponse.Data);

            return orderListResponse.IsSuccess ? Response<List<OrderDto>>.Success(dtoList) : Response<List<OrderDto>>.Error();
        }

        [CacheAspect]
        public async Task<IResponse<List<OrderDto>>> ListAllOrderAsync()
        {
            var orderListResponse = await _orderRepository.GetAllAsync();
            var dtoList = ObjectMapper.Mapper.Map<List<OrderDto>>(orderListResponse.Data);

            return orderListResponse.IsSuccess ? Response<List<OrderDto>>.Success(dtoList) : Response<List<OrderDto>>.Error();
        }
    }
}
