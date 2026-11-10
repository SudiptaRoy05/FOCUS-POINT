using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models
{
    public class Review
    {
        public int Id { get; set; }

        
        public string Text { get; set; }
       
        public int EntityId { get; set; }


        public string EntityType { get; set; }


       
        [ForeignKey("EntityId")]
        public MediaItem MediaItem { get; set; } 

        [ForeignKey("EntityId")]
        public Packege Packege { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
