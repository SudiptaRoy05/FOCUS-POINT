using FocusPoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.DataAccess.Repository.IRepository
{
    public interface IBookingDetailsRepository : IRepository<BookingDetails>
    {
        public void Update(BookingDetails obj);
    }
}
