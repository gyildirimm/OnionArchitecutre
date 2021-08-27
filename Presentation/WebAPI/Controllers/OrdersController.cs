using Application.Core.Wrappers;
using Application.Interfaces;
using DTO.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IResponse<int>> CreateOrder(CreateOrderDto order)
        {
            var response = await _orderService.Create(order);
            return response;
        }

        [HttpGet]
        [Route("GetAllOrder")]
        public IResponse<List<OrderDto>> GetAllOrder()
        {
            var response = _orderService.ListAllOrder();
            return response;
        }

        [HttpGet]
        [Route("GetAllOrderAsync")]
        public async Task<IResponse<List<OrderDto>>> GetAllOrderAsync()
        {
            var response = await _orderService.ListAllOrderAsync();

            return response;
        }

        [HttpGet]
        [Route("BlaBla")]
        public void BlaBla()
        {
            Type type = typeof(IResponse<List<OrderDto>>);

            var instance = Activator.CreateInstance(type) as IResponse<object>;

        }
    }
}
