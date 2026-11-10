using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models.ViewModel
{
	public class BookingVM
	{
		public BookingHeder BookingHeder { get; set; }

		public IEnumerable<BookingDetails> BookingDetails { get; set; }
	}
}
