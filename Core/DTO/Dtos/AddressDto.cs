using DTO.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Dtos
{
    [Serializable()]
    public class AddressDto : IDto
    {
        public string Province { get; set; }

        public string District { get; set; }

        public string Street { get; set; }

        public string ZipCode { get; set; }

        public string Line { get; set; }
    }
}
