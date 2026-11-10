using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models.ViewModel
{
    public class PurchaseVM
    {
        public IEnumerable<MediaItem> Items { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}
