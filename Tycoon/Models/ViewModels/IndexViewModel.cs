﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tycoon.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Service> Service { get; set; }
        public IEnumerable<Coupon> Coupon { get; set; }
        public IEnumerable<Category> Category { get; set; }
    }
}
