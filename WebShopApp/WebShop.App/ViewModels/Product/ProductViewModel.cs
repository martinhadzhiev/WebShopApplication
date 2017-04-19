﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebShop.App.ViewModels.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Review { get; set; }
    }
}