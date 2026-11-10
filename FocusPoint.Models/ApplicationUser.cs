using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FocusPoint.Models
{
    public class ApplicationUser : IdentityUser
    {
        

        [Required]
        public string Name { get; set; }

        public string? Bio { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }
        
        public string? District { get; set; }
        
        public string? PostalCode { get; set; }

        public string? ProfilePic { get; set; }

        public string? SocialLink1 { get; set; }

        public string? SocialLink2 { get; set; }
        
        public string? SocialLink3 { get; set; }

        public UserType Type { get; set; }

        public enum UserType
        {
            Photographer,
            NormalUser
        }

    }
}
