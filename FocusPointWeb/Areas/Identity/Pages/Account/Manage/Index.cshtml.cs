// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FocusPoint.DataAccess;
using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using FocusPoint.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FocusPointWeb.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _he;
        private readonly ApplicationDbContext _db;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext db,
            IWebHostEnvironment he)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
            _he = he;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }
		
		
		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
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

            public string Email { get; set; }
            
            public string ProfilePic { get; set; }

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
            var socialLink3 = user. SocialLink3;
            var bio = user.Bio;


			Username = userName;
           
            
            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Name = name,
                Address = address,
                City = city,
                District = district,
                PostalCode = postalCode,
                Email = email,
                //ProfilePic = profilePic,
                Bio = bio,
                SocialLink1 = socialLink1,
                SocialLink2 = socialLink2,
                SocialLink3 = socialLink3

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

           

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

            // Save the changes to the database
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Your profile has been updated";
            }
            else
            {
                // Handle the case when there are errors in updating the user
                StatusMessage = "Error updating the user profile.";
            }

            return RedirectToPage();
        }

    }
}
