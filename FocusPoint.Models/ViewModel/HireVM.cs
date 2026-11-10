using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models.ViewModel
{
    public class HireVM
    {
        public IEnumerable<Hire> ListHire { get; set; }
        //public double HireTotal { get; set; }
        public BookingHeder BookingHeder { get; set; }
        public DateTime StartDate { get; set; }

		public string ApplicationUserId { get; set; }

		[ForeignKey(nameof(ApplicationUserId))]
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }
	}
}
