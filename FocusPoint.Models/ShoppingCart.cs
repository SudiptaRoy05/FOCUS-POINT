using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public int MediaItemId { get; set; }

        [ForeignKey(nameof(MediaItemId))]
        [ValidateNever]
        public MediaItem MediaItem { get; set; }

        [Range(1,1, ErrorMessage = "1")]
        public int Count { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        [NotMapped]
        public double Price { get; set; }
    }
}
