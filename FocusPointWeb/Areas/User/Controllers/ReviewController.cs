using FocusPoint.DataAccess;
using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FocusPointWeb.Areas.User.Controllers
{
    [Area("User")]
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ReviewController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult PhotoComment(int mediaItemId)
        {
            IEnumerable<Review> reviews = _db.Reviews.Where(u => u.EntityId == mediaItemId
            && u.EntityType == "MediaItem").ToList();

            return Ok(reviews);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PhotoComment([Bind("EntityId,EntityType,Text")] Review review)
        {
            // Validate the comment and save it to the database
            // Make sure to set the EntityType and EntityId appropriately
            // Typically, you would also check if the user is authenticated

            if (ModelState.IsValid)
            {
                _db.Add(review);
                _db.SaveChanges();
                return RedirectToAction("GetPhotoComment", new { mediaItemId = review.EntityId });
            }

            return View(review);
        }




        public IActionResult GetPackegeComment(int packegeId)
        {

            IEnumerable<Review> reviews = _db.Reviews.Where(u => u.EntityId == packegeId
            && u.EntityType == "Packege").ToList();

            return Ok(reviews);
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPackegeComment([Bind("EntityId,EntityType,Text")] Review review)
        {
            // Validate the comment and save it to the database
            // Make sure to set the EntityType and EntityId appropriately
            // Typically, you would also check if the user is authenticated

            if (ModelState.IsValid)
            {
                _db.Add(review);
                _db.SaveChanges();
                return RedirectToAction("GetPhotoComment", new { mediaItemId = review.EntityId });
            }

            return View(review);
        }
    }

}

