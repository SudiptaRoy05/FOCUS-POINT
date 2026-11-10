using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using FocusPoint.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FocusPointWeb.Areas.User.Controllers
{
    [Area("User")]
    public class GalleryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public GalleryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<MediaItem> mediaItems = _unitOfWork.MediaItem.GetAll(includeProperties: "Category,Composition").OrderByDescending(u => u.UploadDate).ToList();
            return View(mediaItems);
        }

        //Details Get
       
        public IActionResult Details(int mediaItemId)
        {
            ShoppingCart cartObj = new()
            {
                Count = 1,
                MediaItemId = mediaItemId,
                MediaItem = _unitOfWork.MediaItem.GetFirstOrDefault(x => x.Id == mediaItemId, includeProperties: "Category,Composition")
            };
            return View(cartObj);
        }

     

        //Details Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart cartFromDb =_unitOfWork.ShoppingCart.GetFirstOrDefault(
                u => u.ApplicationUserId == claim.Value && u.MediaItemId == shoppingCart.MediaItemId);

            if (cartFromDb == null)
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count());
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Search(string query)
        {
            var mediaItems = _unitOfWork.MediaItem.GetAll(includeProperties: "Category,Composition");

            if (!string.IsNullOrEmpty(query))
            {
                mediaItems = mediaItems.Where(item =>
                    item.FileName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    item.Category.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                );
            }

            return View("Index", mediaItems);
        }

       
    }
}
