using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models.ViewModel
{
    public class ReviewVM
    {
        IEnumerable<MediaItem> mediaItems {  get; set; }

        IEnumerable<Packege> packeges { get; set; }


    }
}
