using FocusPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository.IRepository
{
    public interface IBookingHeaderRepository : IRepository<BookingHeder>
    {
        public void Update(BookingHeder obj);

        public void UpdateStatus(int id, string bookingStatus, string? paymentStatus = null);

		public void UpdateStripePaymentID(int id, string sessionId, string paymentIntendId);
	}
}
