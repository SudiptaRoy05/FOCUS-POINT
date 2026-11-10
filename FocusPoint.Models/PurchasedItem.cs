using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models
{
    public class PurchasedItem
    {
        public int Id { get; set; }

        public int MediaItemId { get; set; }

        [ForeignKey(nameof(MediaItemId))]
        [ValidateNever]
        public MediaItem MediaItem { get; set; }



        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
