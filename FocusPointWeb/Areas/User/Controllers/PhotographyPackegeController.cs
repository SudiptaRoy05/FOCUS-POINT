using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using FocusPoint.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FocusPointWeb.Areas.User.Controllers
{
    [Area("User")]
    public class PhotographyPackegeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PhotographyPackegeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public IActionResult Index()
        {
            IEnumerable<Packege> packegeList = _unitOfWork.Packege.GetAll(includeProperties:"ApplicationUser").OrderByDescending(u => u.CreatedDateTime);
            return View(packegeList);
        }

        //Get
        public IActionResult Details(int packegeId)
        {
            Hire cartObj = new()
            {
                Days = 1,
                PackegesId = packegeId,
                Packege = _unitOfWork.Packege.GetFirstOrDefault(x => x.Id == packegeId)
            };
            return View(cartObj);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(Hire hire)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            hire.ApplicationUserId = claim.Value;

            Hire hireFromDb = _unitOfWork.Hire.GetFirstOrDefault(
            u => u.ApplicationUserId == claim.Value && u.PackegesId == hire.PackegesId);

            

            if(hireFromDb == null)
            {
                _unitOfWork.Hire.Add(hire);
            }
            else
            {
                _unitOfWork.Hire.IncrementCount(hireFromDb, hire.Days);
            } 
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Search(string query)
        {
            var packege = _unitOfWork.Packege.GetAll(includeProperties: "ApplicationUser,Category");

            if (!string.IsNullOrEmpty(query))
            {
                packege = packege.Where(item =>
                    item.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    item.Category.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                );
            }

            return View("Index", packege);
        }
    }
}
