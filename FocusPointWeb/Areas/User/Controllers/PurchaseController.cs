using FocusPoint.DataAccess;
using FocusPoint.DataAccess.Repository;
using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using FocusPoint.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FocusPointWeb.Areas.User.Controllers
{
    [Area("User")]
    public class PurchaseController : Controller
    {
        private readonly ApplicationDbContext _db;
        public readonly IWebHostEnvironment _he;
        public PurchaseController(ApplicationDbContext db, IWebHostEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                var result = _db.PurchasedItems
                    .Where(purchase => purchase.ApplicationUserId == userId)
                    .Join(
                        _db.MediaItems,
                        purchase => purchase.MediaItemId,
                        mediaItem => mediaItem.Id,
                        (purchase, mediaItem) => new
                        {
                            MediaItemId = mediaItem.Id,
                            MediaItemName = mediaItem.FileName,
                            MediaItemFileUrl = mediaItem.FileUrl,
                        })
                        .ToList();

                if (result.Any())
                {
                    return View(result);
                }
            }

            return View("NoPurchasedItems"); // Display a view for users with no purchased items
        }

        private string GetContentType(string fileExtension)
        {
            // Default content type in case the file extension is not recognized
            string contentType = "application/octet-stream";

            if (fileExtension != null)
            {
                switch (fileExtension.ToLower())
                {
                    case ".jpg":
                        contentType = "image/jpg";
                        break;
                    case ".jpeg":
                        contentType = "image/jpeg";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".gif":
                        contentType = "image/gif";
                        break;
                        // Add more cases for other file types as needed
                }
            }

            return contentType;
        }

        
        public IActionResult DownloadFile(int mediaItemId)
        {
            var mediaItem = _db.MediaItems.FirstOrDefault(m => m.Id == mediaItemId);

            if (mediaItem != null)
            {
                // Construct the full file path on the server
                string filePath = Path.Combine(_he.WebRootPath, @"Media\Gallery", mediaItem.FileUrl);

                if (System.IO.File.Exists(filePath))
                {
                    // Determine the content type based on the file extension
                    string contentType = GetContentType(Path.GetExtension(filePath));

                    // Set the file name for the download
                    string fileName = mediaItem.FileName;

                    // Read the file as a byte array
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                    return File(fileBytes, contentType, fileName);
                }
            }

            // If the media item doesn't exist or there's an issue, return a 404 Not Found response or other appropriate error handling
            return NotFound();
        }


    }
}
