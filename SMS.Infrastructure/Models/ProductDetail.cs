using System;
using System.Collections.Generic;

namespace SMS.Infrastructure.Models
{
    public partial class ProductDetail
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public int ProductPrice { get; set; }
        public int ProductStock { get; set; }
    }
}
