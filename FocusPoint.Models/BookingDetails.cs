using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models
{
    public class BookingDetails
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        [ForeignKey(nameof(BookingId))]
        [ValidateNever]
        public BookingHeder BookingHeder { get; set; }

        public int PackegeId { get; set; }

        [ForeignKey(nameof(PackegeId))]
        [ValidateNever]
        public Packege Packege { get; set; }

        public int Days { get; set; }

        public double Price { get; set; }
    }
}
