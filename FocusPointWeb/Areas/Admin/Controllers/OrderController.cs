using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using FocusPoint.Models.ViewModel;
using FocusPoint.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace FocusPointWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin +" , "+ SD.Role_Photographer)] 
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int orderId)
        {
            OrderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.Id == orderId, includeProperties: "MediaItem")

            };
            return View(OrderVM);
        }
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult UpdateOrderDetail()
        {

            var orderHEaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id, tracked: false);
            orderHEaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHEaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHEaderFromDb.Address = OrderVM.OrderHeader.Address;
            orderHEaderFromDb.City = OrderVM.OrderHeader.City;
            orderHEaderFromDb.District = OrderVM.OrderHeader.District;
            orderHEaderFromDb.PostalCOde = OrderVM.OrderHeader.PostalCOde;
            _unitOfWork.OrderHeader.Update(orderHEaderFromDb);
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = orderHEaderFromDb.Id });
        }*/



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;
           

            if (User.IsInRole(SD.Role_Admin))
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == claim.Value,includeProperties: "ApplicationUser");
            }

            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusPending);
                    break;

                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.StatusInProcess);
                    break;

                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.StatusApprove);
                    break;

                 
            }


            return Json(new { data = orderHeaders });
        }

        #endregion
    }
}
