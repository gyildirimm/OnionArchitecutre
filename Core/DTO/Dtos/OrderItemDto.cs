using DTO.Interfaces;
using System;

namespace DTO.Dtos
{
    [Serializable()]
    public class OrderItemDto : IDto
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string PictureUrl { get; set; }

        public Decimal Price { get; set; }
    }
}
