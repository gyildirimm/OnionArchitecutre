using DTO.Interfaces;
using System;
using System.Collections.Generic;

namespace DTO.Dtos
{
    [Serializable()]
    public class OrderDto : IDto
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public AddressDto Address { get; set; }

        public string BuyerId { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }
    }
}
