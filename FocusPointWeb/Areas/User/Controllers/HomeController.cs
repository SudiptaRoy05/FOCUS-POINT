using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using FocusPoint.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FocusPointWeb.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
       
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {

            var mediaItems = _unitOfWork.MediaItem.GetAll(includeProperties: "Category,Composition").OrderByDescending( u=>u.UploadDate).ToList();
            var packages = _unitOfWork.Packege.GetAll(includeProperties: "ApplicationUser").OrderByDescending(u => u.CreatedDateTime).ToList();

            var homeVM = new HomeVM
            {
                MediaItems = mediaItems,
                Packeges = packages
            };

            return View(homeVM);

        }

        [HttpGet]
        public IActionResult Search(string query)
        {
            var mediaItems = _unitOfWork.MediaItem.GetAll(includeProperties: "Category,Composition");
            var packeges = _unitOfWork.Packege.GetAll(includeProperties:"ApplicationUser"); // Assuming you have a GetAll method for packages

            if (!string.IsNullOrEmpty(query))
            {
                mediaItems = mediaItems.Where(item =>
                    item.FileName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    item.Category.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                );

                packeges = packeges.Where(item =>
                    item.Name.Contains(query, StringComparison.OrdinalIgnoreCase) /*||
                    item.Category.Name.Contains(query, StringComparison.OrdinalIgnoreCase)*/
                );
            }

            var homeVM = new HomeVM
            {
                MediaItems = mediaItems.ToList(),
                Packeges = packeges.ToList()
            };

            return View("Index",homeVM);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}