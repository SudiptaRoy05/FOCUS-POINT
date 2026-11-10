using FocusPoint.DataAccess;
using FocusPoint.DataAccess.Repository;
using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using FocusPoint.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FocusPointWeb.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class PhotographerController : Controller
    {
        private readonly ApplicationDbContext _db;
        public readonly UserManager<ApplicationUser> _userManager;
        public PhotographerController(ApplicationDbContext db,UserManager<ApplicationUser> userManager)
        {
            _db = db; 
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<ApplicationUser> photographers = await _db.ApplicationUsers
                .Where(user => user.Type == 0)
                .ToListAsync();

            return View(photographers);
        }





        public async Task<IActionResult> Profile(string id)
        {
            //ApplicationUser user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == id);
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var mediaItems = _db.MediaItems.Where(mi => mi.ApplicationUserId == user.Id).ToList();
            var packages = _db.Packeges.Where(p => p.ApplicationUserId == user.Id).ToList();

            var userVM = new UserVM
            {
                ApplicationUser = user,
                MediaItems = mediaItems,
                Packeges = packages
            };


            return View(userVM);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // You can add any additional data you need to edit here
            var userVM = new UserVM
            {
                ApplicationUser = user
            };

            return View(userVM);
        }



        public async Task<IActionResult> Search( string query )
        {
            var users = _db.ApplicationUsers.ToList();

            if (!string.IsNullOrEmpty(query))
            {
                query = query.ToLower(); // Convert the query to lowercase for case-insensitive search

                users = users.Where(item =>
                    item.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    item.Email.Contains(query, StringComparison.OrdinalIgnoreCase) 
                ).ToList();
            }

            return View("Index", users);
        }

       /* [HttpGet]
        public IActionResult Search(string query)
        {
           

        }*/



        /*


                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> Edit(UserVM userVM, IFormFile profilePicture)
                {
                    // Check if the user is authenticated
                    if (!User.Identity.IsAuthenticated)
                    {
                        return RedirectToAction("Login"); // Redirect to the login page if the user is not authenticated
                    }

                    // Get the current logged-in user
                    var currentUser = await _userManager.GetUserAsync(User);

                    if (currentUser == null)
                    {
                        return NotFound("User not found"); // Handle the case if the current user is not found
                    }

                    // Check if the user is trying to edit their own profile
                    if (currentUser.Id != userVM.ApplicationUser.Id)
                    {
                        return Unauthorized(); // Return an unauthorized status code if the user is not allowed to edit this profile
                    }

                    // Check if the model state is valid
                    if (!ModelState.IsValid)
                    {
                        return View(userVM); // Return the view with validation errors if the model state is not valid
                    }

                    // Update the user's profile information
                    currentUser.Name = userVM.ApplicationUser.Name;
                    currentUser.Address = userVM.ApplicationUser.Address;
                    currentUser.City = userVM.ApplicationUser.City;
                    currentUser.District = userVM.ApplicationUser.District;
                    currentUser.PostalCode = userVM.ApplicationUser.PostalCode;
                    currentUser.Bio = userVM.ApplicationUser.Bio;
                    currentUser.SocialLink1 = userVM.ApplicationUser.SocialLink1;
                    currentUser.SocialLink2 = userVM.ApplicationUser.SocialLink2;
                    currentUser.SocialLink3 = userVM.ApplicationUser.SocialLink3;

                    // Check if a new profile picture is uploaded
                    if (profilePicture != null)
                    {
                        // Handle the profile picture upload logic here, and save the new file
                        // For example, you can use a file storage service or save it to the server
                        // You can replace "HandleProfilePictureUpload" with your logic
                        currentUser.ProfilePic = await HandleProfilePictureUpload(profilePicture);
                    }

                    // Update other user properties as needed

                    var updateResult = await _userManager.UpdateAsync(currentUser);

                    if (updateResult.Succeeded)
                    {
                        // Redirect to the user's profile page after a successful update
                        return RedirectToAction("Profile", new { id = currentUser.Id });
                    }
                    else
                    {
                        // Handle the case when there are errors in updating the user's profile
                        ModelState.AddModelError("", "Error updating the user profile.");
                        return View(userVM);
                    }
                }

                private async Task<string> HandleProfilePictureUpload(IFormFile profilePicture)
                {
                    // Implement your logic to handle profile picture upload here
                    // You can save the file to a storage service, server, or other desired location
                    // Return the file path or URL to be saved in the user's profile
                    // Example: Save to a local server
                    if (profilePicture != null && profilePicture.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(profilePicture.FileName);
                        var filePath = Path.Combine("wwwroot/profilepics", fileName); // Replace with your desired file path
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await profilePicture.CopyToAsync(stream);
                        }
                        return "/profilepics/" + fileName; // Update the path as needed
                    }

                    return null;
                }*/


    }
}
