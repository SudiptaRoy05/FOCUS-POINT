using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models.ViewModel
{
    public class MediaVM
    {
        public MediaItem MediaItem { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CompositionList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> MediaType { get; set; }
    }
}
