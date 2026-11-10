using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models
{
    public class Hire
    {
        public int Id { get; set; }

        [Range(1, 100, ErrorMessage = "Days must be a non-negative number")]
        public int Days { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }


        public int PackegesId { get; set; }

        [ForeignKey(nameof(PackegesId))]
        [ValidateNever]
        public Packege Packege { get; set; }

		public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; } = DateTime.Now;

		[NotMapped]
        public double Price { get; set; }
    }
}
