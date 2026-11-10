using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using FocusPoint.Models.ViewModel;
using FocusPoint.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace FocusPointWeb.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = SD.Role_Photographer)]
    public class PackegeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _he;
        public PackegeController(IUnitOfWork unitOfWork, IWebHostEnvironment he)
        {
            _unitOfWork = unitOfWork;
            _he = he;

        }
        public IActionResult Index()
        {
            //IEnumerable<Packege> packeges = _unitOfWork.Packege.GetAll();
            return View();
        }


        //UpsertGet
        public IActionResult Upsert(int? id)
        {
            PackegeVM packegeVM = new()
            {
                Packege = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

            };

            if (id == null || id == 0)
            {
                //Upload Photo
                return View(packegeVM);
            }
            else
            {
                //Update Photo
                packegeVM.Packege = _unitOfWork.Packege.GetFirstOrDefault(u => u.Id == id);
                return View(packegeVM);
            }
        }

        //Upsert Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Photographer)]
        public IActionResult Upsert(PackegeVM obj, IFormFile file)
        {

            if (file != null)
            {
                string wwwRootPath = _he.WebRootPath;
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".mp4", ".3gp", ".mkv" };
                string extension = Path.GetExtension(file.FileName);

                if (!allowedExtensions.Contains(extension.ToLower()))
                {
                    ModelState.AddModelError(string.Empty, "Only .jpg, .jpeg, .mp4, .mkv, .3gp and .png file types are allowed.");
                    TempData["error"] = "Please upload Image of Video!";
                    return View(obj);
                }

                string filename = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"Media\Packege");

                if (obj.Packege.FileUrl != null)
                {
                    var oldFilePath = Path.Combine(wwwRootPath, obj.Packege.FileUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                obj.Packege.FileUrl = @"\Media\Packege\" + filename + extension;
            }
            obj.Packege.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (obj.Packege.Id == 0)
            {
                _unitOfWork.Packege.Add(obj.Packege);
            }
            else
            {
                _unitOfWork.Packege.Update(obj.Packege);
            }

            _unitOfWork.Save();
            TempData["success"] = "Packege created successfully";
            return RedirectToAction("Index");
        }

		#region API CALLS

		public IActionResult GetAll()
		{
            List<Packege> packageList;
            if (User.IsInRole(SD.Role_Admin))
            {
                packageList = _unitOfWork.Packege
               .GetAll(includeProperties: "Category")
               .ToList();
            }
            else
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                 packageList = _unitOfWork.Packege
                    .GetAll(includeProperties: "Category")
                    .Where(pkg => pkg.ApplicationUserId == userId)
                    .ToList();
            }
               

			return Json(new { data = packageList });
		}


		[HttpPost]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var obj = _unitOfWork.Packege.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldFilePath = Path.Combine(_he.WebRootPath, obj.FileUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            _unitOfWork.Packege.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successfull" });

        }


        #endregion

    }
}
