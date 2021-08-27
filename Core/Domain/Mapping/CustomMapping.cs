using AutoMapper;
using Domain.OrderAggregate;
using DTO.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Mapping
{
    public class CustomMapping : Profile
    {
        public CustomMapping()
        {
            CreateMap<Order, OrderDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>().ReverseMap();

            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
