using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tycoon.Models
{
    public class Cart
    {

        public Cart()
        {
            Count = 1;
        }
        public int Id { get; set; }
        public string UserId { get; set; }

        [NotMapped]
        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }
        public int ServiceId { get; set; }

        [NotMapped]
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage ="Please enter value greater than or equal to {1}")]
        public int Count { get; set; }
    }
}
