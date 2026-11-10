using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models.ViewModel
{
    public class AdminVM
    {
        public ApplicationUser User { get; set; }
        public IList<string> Roles { get; set; }

        public string ApplicationUserId { get; set; }
        // public ApplicationUser ApplicationUser { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public int MediaCount { get; set; }
        public int MediaItemCount { get; set; }
        public int PackageItemCount { get; set; }
    }
}
