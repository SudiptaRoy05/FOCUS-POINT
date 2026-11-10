using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models.ViewModel
{
    public class HomeVM
    {
        public List<MediaItem> MediaItems { get; set; }
        public List<Packege> Packeges { get; set; }
    }
}
