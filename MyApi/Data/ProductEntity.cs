using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApi.Data
{
    public class ProductEntity
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        public decimal price { get; set; }
    }

}