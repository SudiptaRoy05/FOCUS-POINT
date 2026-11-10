using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository
{
    public class BookingDetailsRepository : Repository<BookingDetails>, IBookingDetailsRepository
    {
        private ApplicationDbContext _db;
        public BookingDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(BookingDetails obj)
        {
            _db.BookingDetails.Update(obj);
        }
    }
}
