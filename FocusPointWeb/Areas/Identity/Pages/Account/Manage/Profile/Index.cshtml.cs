using FocusPoint.DataAccess;
using FocusPoint.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace FocusPointWeb.Areas.Identity.Pages.Account.Manage.Profile
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        public IndexModel(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public ApplicationUser ApplicationUser { get; set; }

        public List<MediaItem> MediaItemsToShow { get; set; }
        public List<MediaItem> AllMediaItems { get; set; }
        public bool ShowAllMediaItems { get; set; }
        public List<Packege> PackegesToShow { get; set; }
        public List<Packege> AllPackeges { get; set; }
        public bool ShowAllPackeges { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = _userManager.GetUserAsync(User).Result;

            if (user == null)
            {
                return NotFound();
            }

            ApplicationUser = user;

            // Load all media items for the user
            AllMediaItems = _db.MediaItems.Where(m => m.ApplicationUserId == user.Id).ToList();

            // Select the first 3 media items to show by default
            MediaItemsToShow = AllMediaItems.Take(3).ToList();

            // Set ShowAllMediaItems to false by default
            ShowAllMediaItems = false;



            AllPackeges = _db.Packeges.Where(m => m.ApplicationUserId == user.Id).ToList();

            // Select the first 3 packeges to show by default
            PackegesToShow = AllPackeges.Take(3).ToList();

            // Set ShowAllPackeges to false by default
            ShowAllPackeges = false;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile profilePic)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            if (profilePic != null && profilePic.Length > 0)
            {
                // Process and save the uploaded image
                using (var stream = new MemoryStream())
                {
                    await profilePic.CopyToAsync(stream);

                    // Load the uploaded image using ImageSharp
                    using (var image = Image.Load(stream.ToArray()))
                    {
                        if (image.Width > 200 || image.Height > 200)
                        {
                            // If the image is larger than 200x200, crop and resize it
                            image.Mutate(x => x
                                .Resize(new ResizeOptions
                                {
                                    Size = new Size(200, 200),
                                    Mode = ResizeMode.Crop
                                }));
                        }

                        // Generate a unique filename for the processed image
                        string uniqueFileName = $"{Guid.NewGuid()}_profile.jpg";

                        // Define the folder where the image will be saved
                        string uploadFolder = Path.Combine("wwwroot", "Media", "DP");

                        // Ensure the target folder exists
                        Directory.CreateDirectory(uploadFolder);

                        // Combine the folder path with the unique filename
                        string filePath = Path.Combine(uploadFolder, uniqueFileName);

                        // Save the processed image as a JPEG
                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            image.Save(fs, new JpegEncoder());
                        }

                        // Update the user's profile picture path
                        user.ProfilePic = $"/Media/DP/{uniqueFileName}";
                    }
                }

                // Update the user's profile picture path in the database
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Profile picture updated successfully
                    // You can add a success message to TempData if needed
                    TempData["SuccessMessage"] = "Profile picture updated successfully.";
                }
                else
                {
                    // Handle the case when there are errors in updating the user
                    // You can add an error message to TempData if needed
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    TempData["ErrorMessage"] = "An error occurred while updating the profile picture.";
                }
            }

            // Redirect back to the profile page
            return RedirectToPage();
        }
    }
}



