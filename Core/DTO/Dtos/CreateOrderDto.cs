using DTO.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace DTO.Dtos
{
    public class CreateOrderDto : IDto
    {
        public string BuyerId { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }

        public AddressDto Address { get; set; }
    }
}
