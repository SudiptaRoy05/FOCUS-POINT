using FocusPoint.DataAccess;
using FocusPoint.Models;
using FocusPoint.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FocusPointWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            var usersWithRoles = new List<AdminVM>();

            foreach (var user in _userManager.Users)
            {
                var userRoles = _userManager.GetRolesAsync(user).Result;

                var userViewModel = new AdminVM
                {
                    User = user,
                    Roles = userRoles
                };

                usersWithRoles.Add(userViewModel);
            }

            return View(usersWithRoles);
        }
        public IActionResult MediaItem()
        {
            int totalMediaItemCount = _db.MediaItems.Count(); // Assuming _db is your application's DbContext

            ViewBag.TotalMediaItemCount = totalMediaItemCount;

            return View();
        }


      /*  public IActionResult PhotographerList()
        {
            var photographerUsers = _db.Users.Where(u => u.Type == 0).ToList();
            return View(photographerUsers);
        }
*/

    }
}
