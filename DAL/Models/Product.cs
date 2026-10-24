using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }
}
