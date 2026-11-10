using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models.ViewModel
{
    public class UserVM
    {
        public ApplicationUser ApplicationUser { get; set; }

        public IEnumerable<MediaItem> MediaItems { get; set; }
        public IEnumerable<Packege> Packeges { get; set; }
        
       
    }
}
