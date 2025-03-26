﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.ProductModule
{
    public class Product :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int ProductTypeId { get; set; }  //FK 
        public int ProductBrandId { get; set; } //FK 
        public ProductType ProductType { get; set; }
        public ProductBrand ProductBrand { get; set; }
    }
}
