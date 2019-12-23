using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tycoon.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Popularity { get; set; }
        public enum EDemand { NA=0, NotPopular =1, Popular =2, VeryPopular=3}

        public string Image { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "SubCategory")]
        public int SubCategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage ="Price Should be greater than ${1}")]
        public double Price { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }



    }
}
