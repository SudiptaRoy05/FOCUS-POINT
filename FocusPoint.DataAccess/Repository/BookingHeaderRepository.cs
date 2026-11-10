using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository
{
    public class BookingHeaderRepository : Repository<BookingHeder>, IBookingHeaderRepository
    {
        private ApplicationDbContext _db;
        public BookingHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(BookingHeder obj)
        {
            _db.BookingHeders.Update(obj);
        }

        public void UpdateStatus(int id, string bookingStatus, string? paymentStatus = null)
        {
            var bookingFromDb = _db.BookingHeders.FirstOrDefault(u => u.Id == id);
            if (bookingFromDb != null)
            {
                bookingFromDb.BookingStatus = bookingStatus;
                if(paymentStatus != null)
                {
                    bookingFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

		public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
		{
			var orderFromDb = _db.BookingHeders.FirstOrDefault(x => x.Id == id);
			orderFromDb.PaymentDate = DateTime.Now;
			orderFromDb.SessionId = sessionId;
			orderFromDb.PaymentIntetId = paymentIntentId;
		}
	}
}
