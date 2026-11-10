using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models
{
    public class MediaItem
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string Description { get; set; }

        public string FileUrl { get; set; }

        public string Thumbnail1 { get; set; }
        
        public string Thumbnail2 { get; set; }

        public DateTime UploadDate { get; set; } = DateTime.Now;

        public bool IsPublic { get; set; }

        //public bool IsHidden { get; set; }

        [Required]
        public double Price { get; set; }

        public MediaType Type { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [Required]
        public int CompositionId { get; set; }
        [ForeignKey("CompositionId")]
        [ValidateNever]
        public Composition Composition { get; set; }

        [Required]
        [ForeignKey("ApplicationUserId")]
        public string ApplicationUserId { get; set; }


        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        public enum MediaType
        {
            Photograph,
            Videograph
        }
    }
}
