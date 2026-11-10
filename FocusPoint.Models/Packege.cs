using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models
{
    public class Packege
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string FileUrl { get; set; }

        public double Price { get; set; }

        [Display(Name = "3-5 Days")]
        public double Price3 { get; set; }

        [Display(Name = "6-10 Days")]
        public double Price6 { get; set; }

        [Display(Name = "10 Days+")]
        public double Price10 { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        
        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
