using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tycoon.Models.ViewModels
{
    public class ServiceViewModel
    {
        public Service Service { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<SubCategory> SubCategory { get; set; }
    }
}
