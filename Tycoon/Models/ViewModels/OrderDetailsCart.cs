﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tycoon.Models.ViewModels
{
    public class OrderDetailsCart
    {
        public List<Cart> listCart { get; set; }
        public Order Order { get; set; }

    }
}
