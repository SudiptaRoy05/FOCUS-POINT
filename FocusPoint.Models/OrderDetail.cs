using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }

        public int MediaItemId { get; set; }

        [ForeignKey(nameof(MediaItemId))]
        [ValidateNever]
        public MediaItem MediaItem { get; set; }

        public int Count { get; set; }

        public double Price { get; set; }
    }
}
