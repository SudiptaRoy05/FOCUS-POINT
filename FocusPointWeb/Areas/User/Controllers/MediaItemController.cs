using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using FocusPoint.Models.ViewModel;
using FocusPoint.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Claims;

namespace FocusPointWeb.Areas.User.Controllers
{
    [Area("User")]
    public class MediaItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _he;
        public MediaItemController(IUnitOfWork unitOfWork, IWebHostEnvironment he)
        {
            _unitOfWork = unitOfWork;
            _he = he;
        }
        public IActionResult Index()
        {
            IEnumerable<MediaItem> mediaItems = _unitOfWork.MediaItem.GetAll();
            return View(mediaItems);
        }

        //Upsert Get
        public IActionResult Upsert(int? id)
        {
            MediaVM mediaVM = new()
            {
                MediaItem = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CompositionList = _unitOfWork.Composition.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                MediaType = Enum.GetValues(typeof(MediaItem.MediaType))
                .Cast<MediaItem.MediaType>()
                .Select(m => new SelectListItem
                {
                    Text = m.ToString(),
                    Value = ((int)m).ToString()
                })

            };

            if (id == null || id == 0)
            {
                //Upload Photo
                return View(mediaVM);
            }
            else
            {
                //Update Photo
                mediaVM.MediaItem = _unitOfWork.MediaItem.GetFirstOrDefault(u => u.Id == id);
                return View(mediaVM);
            }
        }

        //Upsert Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Photographer)]
        public IActionResult Upsert(MediaVM obj, IFormFile file)
        {
            if (file != null)
            {
                string wwwRootPath = _he.WebRootPath;
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png",  ".webp" };
                string extension = Path.GetExtension(file.FileName);

                if (!allowedExtensions.Contains(extension.ToLower()))
                {
                    ModelState.AddModelError(string.Empty, "Only .jpg, .jpeg, .webp,  and .png file types are allowed.");
                    TempData["error"] = "Please upload an image or video!";
                    return View(obj);
                }

                string filename = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"Media\Gallery");

                if (obj.MediaItem.FileUrl != null)
                {
                    var oldFilePath = Path.Combine(wwwRootPath, obj.MediaItem.FileUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                obj.MediaItem.FileUrl = @"\Media\Gallery\" + filename + extension;

                // Create thumbnail1 (200px x 300px)
                string thumbnail1Path = Path.Combine(uploads, filename + "_thumbnail1" + extension);
                using (var image = new Bitmap(Path.Combine(uploads, filename + extension)))
                {
                    int thumbnail1Width = 300;
                    int thumbnail1Height = 200;
                    var thumbnail1 = new Bitmap(thumbnail1Width, thumbnail1Height);
                    using (var graphics = Graphics.FromImage(thumbnail1))
                    {
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.DrawImage(image, 0, 0, thumbnail1Width, thumbnail1Height);
                    }
                    thumbnail1.Save(thumbnail1Path);
                }
                obj.MediaItem.Thumbnail1 = "/Media/Gallery/" + filename + "_thumbnail1" + extension;

                // Create thumbnail2 (700px x 400px)
                string thumbnail2Path = Path.Combine(uploads, filename + "_thumbnail2" + extension);
                using (var image = new Bitmap(Path.Combine(uploads, filename + extension)))
                {
                    int thumbnail2Width = 700;
                    int thumbnail2Height = 400;
                    var thumbnail2 = new Bitmap(thumbnail2Width, thumbnail2Height);
                    using (var graphics = Graphics.FromImage(thumbnail2))
                    {
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.DrawImage(image, 0, 0, thumbnail2Width, thumbnail2Height);
                    }
                    thumbnail2.Save(thumbnail2Path);
                }
                obj.MediaItem.Thumbnail2 = "/Media/Gallery/" + filename + "_thumbnail2" + extension;
            }

            obj.MediaItem.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (obj.MediaItem.Id == 0)
            {
                _unitOfWork.MediaItem.Add(obj.MediaItem);
            }
            else
            {
                _unitOfWork.MediaItem.Update(obj.MediaItem);
            }

            _unitOfWork.Save();
            TempData["success"] = "Media Uploaded successfully";
            return RedirectToAction("Index");

        }




        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {

            List<MediaItem> mediaList;
            if (User.IsInRole(SD.Role_Admin))
            {
                 mediaList = _unitOfWork.MediaItem
                .GetAll(includeProperties: "Category,Composition")
                .ToList();
            }
            else
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                 mediaList = _unitOfWork.MediaItem
                    .GetAll(includeProperties: "Category,Composition")
                    .Where(media => media.ApplicationUserId == userId)
                    .ToList();
            }
           

            return Json(new { data = mediaList });
        }

        [HttpPost]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var obj = _unitOfWork.MediaItem.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldFilePath = Path.Combine(_he.WebRootPath, obj.FileUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            _unitOfWork.MediaItem.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successfull" });

        }
        #endregion
    }
}
