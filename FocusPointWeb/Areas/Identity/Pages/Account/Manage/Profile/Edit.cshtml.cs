using FocusPoint.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;
using System.ComponentModel.DataAnnotations;

namespace FocusPointWeb.Areas.Identity.Pages.Account.Manage.Profile
{
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }


            [Required]
            public string Name { get; set; }

            /* [Required]
             public string UserName { get; set; }*/

            public string Address { get; set; }

            public string City { get; set; }

            public string District { get; set; }

            public string PostalCode { get; set; }
           

            public string Bio { get; set; }

            public string SocialLink1 { get; set; }

            public string SocialLink2 { get; set; }

            public string SocialLink3 { get; set; }


            public UserType Type { get; set; }

            public enum UserType
            {
                Photographer,
                NormalUser
            }

        }


        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var email = await _userManager.GetEmailAsync(user);


            var name = user.Name;
            var address = user.Address;
            var city = user.City;
            var district = user.District;
            var postalCode = user.PostalCode;
            // var profilePic = user.ProfilePic;
            var socialLink1 = user.SocialLink1;
            var socialLink2 = user.SocialLink2;
            var socialLink3 = user.SocialLink3;
            var bio = user.Bio;


           /* Username = userName;*/


            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Name = name,
                Address = address,
                City = city,
                District = district,
                PostalCode = postalCode,
               
                Bio = bio,
                SocialLink1 = socialLink1,
                SocialLink2 = socialLink2,
                SocialLink3 = socialLink3

            };
        }


        public async Task<IActionResult> OnGetAsync()
        {
            // Load the user's profile information for editing
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            // Repeat the above process for other fields such as Name, Address, City, etc.
            // Update user properties
            user.Name = Input.Name;
            user.Address = Input.Address;
            user.City = Input.City;
            user.District = Input.District;
            user.PostalCode = Input.PostalCode;
            user.Bio = Input.Bio;
            user.SocialLink1 = Input.SocialLink1;
            user.SocialLink2 = Input.SocialLink2;
            user.SocialLink3 = Input.SocialLink3;

            await _userManager.UpdateAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage("Index");
        }

    }
}


