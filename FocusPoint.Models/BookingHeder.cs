using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models
{
	public class BookingHeder
	{
		public int Id { get; set; }

		public string ApplicationUserId { get; set; }

		[ForeignKey(nameof(ApplicationUserId))]
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }

		[Required]
		public DateTime OrderDate { get; set; }

		public DateTime EventDate { get; set; }

		public double BookingTotal { get; set; }

		public string? BookingStatus { get; set; }

		public string? PaymentStatus { get; set; }

		public DateTime PaymentDate { get; set; }

		public DateTime PaymentDueDate { get; set; }

		public string? SessionId { get; set; }

		public string? PaymentIntetId { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Address { get; set; }

		[Required]
		public string PhoneNumber { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string District { get; set; }

		[Required]
		public string PostalCOde { get; set; }


       
    }
}
