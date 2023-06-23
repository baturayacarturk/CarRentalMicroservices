using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Services.Order.Application.DTOs
{
    public class AddressDto
    {
        public string Provience { get;  set; }//il
        public string District { get;  set; }//ilce
        public string Street { get;  set; }
        public string ZipCode { get;  set; }
        public string Line { get;  set; }//adress satırı

    }
}
