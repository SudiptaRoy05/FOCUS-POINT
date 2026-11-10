using FocusPoint.DataAccess;
using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using FocusPoint.Models.ViewModel;
using FocusPoint.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
using System.Security.Claims;

namespace FocusPointWeb.Areas.User.Controllers
{
	[Area("User")]
    [Authorize]
	public class BookingController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _db;
		
        [BindProperty]
        public BookingVM BookingVM { get; set; }
        public BookingController(IUnitOfWork unitOfWork,UserManager<ApplicationUser>userManager,ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;	
			_userManager = userManager;
			_db = db;
        }
        public IActionResult Index()
        {
            return View();
		}


        public IActionResult Details(int bookingId)
		{
            BookingVM = new BookingVM()
            {
                BookingHeder = _unitOfWork.BookingHeader.GetFirstOrDefault(u => u.Id == bookingId, includeProperties: "ApplicationUser"),
                BookingDetails = _unitOfWork.BookingDetails.GetAll(u => u.BookingId == bookingId , includeProperties: "Packege")
            };
			return View(BookingVM);
		}

		[ActionName("Details")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = SD.Role_Photographer + "," + SD.Role_User)]
		public IActionResult Details_Pay()
		{
			BookingVM.BookingHeder = _unitOfWork.BookingHeader.GetFirstOrDefault(u => u.Id == BookingVM.BookingHeder.Id, includeProperties: "ApplicationUser");
			BookingVM.BookingDetails = _unitOfWork.BookingDetails.GetAll(u => u.BookingId == BookingVM.BookingHeder.Id, includeProperties: "Packege");

            var domain = "https://localhost:44358/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>(),

                Mode = "payment",
                SuccessUrl = domain + $"user/booking/PaymentConfirmation?bookingHederid={BookingVM.BookingHeder.Id}",
                CancelUrl = domain + $"user/booking/details?bookingId ={BookingVM.BookingHeder.Id}",
            };
            foreach (var item in BookingVM.BookingDetails)
            {

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Packege.Price),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Packege.Name,
                        },
                    },
                    Quantity = item.Days,
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);
            //HireVM.BookingHeder.SessionId = session.Id;
            //HireVM.BookingHeder.PaymentIntetId = session.PaymentIntentId;


            _unitOfWork.BookingHeader.UpdateStripePaymentID(BookingVM.BookingHeder.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

          
		}

        public IActionResult PaymentConfirmation(int bookingHederid)
        {
            BookingHeder bookingHeder = _unitOfWork.BookingHeader.GetFirstOrDefault(u => u.Id == bookingHederid, includeProperties: "ApplicationUser");

            if (bookingHeder.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(bookingHeder.SessionId);
                //Check Stripe STSTUS
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(bookingHederid, bookingHeder.SessionId, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(bookingHederid, bookingHeder.BookingStatus, SD.PaymentStatusApproved);
                  //  _emailSender.SendEmailAsync(bookingHeder.ApplicationUser.Email, "Payment is successfull", "<p>Thank You for Hire</p>");
                    _unitOfWork.Save();

                }
            }

            return View(bookingHederid);

        }


        [HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = SD.Role_Photographer )]
		public IActionResult UpdateBookingDetails()
		{
			var bookingHeaderFromDb = _unitOfWork.BookingHeader.GetFirstOrDefault(u => u.Id == BookingVM.BookingHeder.Id,tracked:false);

			bookingHeaderFromDb.Name = BookingVM.BookingHeder.Name; 
			bookingHeaderFromDb.Address = BookingVM.BookingHeder.Address; 
			bookingHeaderFromDb.PhoneNumber = BookingVM.BookingHeder.PhoneNumber; 
			bookingHeaderFromDb.City = BookingVM.BookingHeder.City; 
			bookingHeaderFromDb.District = BookingVM.BookingHeder.District; 
			bookingHeaderFromDb.PostalCOde = BookingVM.BookingHeder.PostalCOde;
			
			_unitOfWork.BookingHeader.Update(bookingHeaderFromDb);
			_unitOfWork.Save();

			TempData["success"] = "Booking Details Updated Successfully";

			return RedirectToAction("Details", "Booking", new { bookingId = bookingHeaderFromDb.Id });
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = SD.Role_Photographer)]
		public IActionResult Attend()
		{
			_unitOfWork.BookingHeader.UpdateStatus(BookingVM.BookingHeder.Id, SD.StatusInProcess);	
			_unitOfWork.Save();

			TempData["success"] = "Booking Status Updated Successfully";

			return RedirectToAction("Details", "Booking", new { bookingId = BookingVM.BookingHeder.Id });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = SD.Role_Photographer)]
		public IActionResult Complete()
		{
			var bookingHeder = _unitOfWork.BookingHeader.GetFirstOrDefault(u => u.Id == BookingVM.BookingHeder.Id, tracked: false);

			bookingHeder.BookingStatus = SD.StatusCompleteEvent;
			bookingHeder.EventDate = DateTime.Now;
			if(bookingHeder.PaymentStatus == SD.PaymentStatusDelayedPayment)
			{
				bookingHeder.PaymentDueDate = DateTime.Now.AddDays(30);
			}


			_unitOfWork.BookingHeader.Update(bookingHeder);
			_unitOfWork.Save();

			TempData["success"] = "Event Attended Successfully";

			return RedirectToAction("Details", "Booking", new { bookingId = BookingVM.BookingHeder.Id });
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = SD.Role_Photographer +","+ SD.Role_User)]
		public IActionResult CancleBooking()
		{
			var bookingHeder = _unitOfWork.BookingHeader.GetFirstOrDefault(u => u.Id == BookingVM.BookingHeder.Id, tracked: false);

			if(bookingHeder.PaymentStatus == SD.PaymentStatusApproved)
			{
				var options = new RefundCreateOptions
				{
					Reason = RefundReasons.RequestedByCustomer,
					PaymentIntent = bookingHeder.PaymentIntetId
				};

				var service = new RefundService();
				Refund refund = service.Create(options);

				_unitOfWork.BookingHeader.UpdateStatus(bookingHeder.Id, SD.StatusCancelled, SD.StatusRefunded);
			}
			else
			{
				_unitOfWork.BookingHeader.UpdateStatus(bookingHeder.Id, SD.StatusCancelled, SD.StatusCancelled);
			}

			_unitOfWork.Save();

			TempData["success"] = "Booking cancle Successfully";

			return RedirectToAction("Details", "Booking", new { bookingId = BookingVM.BookingHeder.Id });
		}



		#region API CALLS
		[HttpGet]
		public IActionResult GetAll(string status)
		{
			IEnumerable<BookingHeder> bookingHeders;

			if (User.IsInRole(SD.Role_Admin))
			{
                bookingHeders = _unitOfWork.BookingHeader.GetAll(includeProperties: "ApplicationUser");
            }

            else if(User.IsInRole(SD.Role_Photographer))
            {

                /*bookingHeders = _db.BookingHeders.Include(bh => bh.ApplicationUser).Join(
								_db.BookingDetails,bh => bh.Id,bd => bd.Id,
								(bh, bd) => new { BookingHeader = bh, BookingDetail = bd }
								).Where(joined => joined.BookingHeader.ApplicationUserId == joined.BookingDetail.Packege.ApplicationUserId).ToList();*/
                /*     var claimsIdentity = (ClaimsIdentity)User.Identity;
                     var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                     string photographerId = claim.Value;


                     var packagesCreatedByPhotographer = _unitOfWork.Packege.GetAll(p => p.ApplicationUserId == photographerId);


                     var packageIds = packagesCreatedByPhotographer.Select(p => p.Id).ToList();


                     bookingHeders = _unitOfWork.BookingHeader.GetAll(bh => packageIds.Contains(bh.photographerId), includeProperties: "ApplicationUser");

     */
                /*var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                string photographerId = claim.Value;

                 bookingHeders = _db.BookingHeders
                    .Where(bh => bh.ApplicationUserId == photographerId)
                    .Include(bh => bh.BookingDetails)
                        .ThenInclude(bd => bd.Packege)
                    .ToList()
                    .Where(bh => bh.BookingDetails != null && bh.BookingDetails.Any(bd => bd.Packege != null && bd.Packege.ApplicationUserId == photographerId))
                    .ToList();*/






                bookingHeders = _unitOfWork.BookingHeader.GetAll(includeProperties: "ApplicationUser");

            }


            else 
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                bookingHeders = _unitOfWork.BookingHeader.GetAll(u => u.ApplicationUserId == claim.Value,includeProperties: "ApplicationUser");

            }

			

			switch (status)
			{
				case "pending":
					bookingHeders = bookingHeders.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
					break;
				case "inprocess":
					bookingHeders = bookingHeders.Where(u => u.BookingStatus == SD.StatusInProcess);
					break;
				case "completed":
					bookingHeders = bookingHeders.Where(u => u.BookingStatus == SD.StatusCompleteEvent);
					break;
				case "approved":
					bookingHeders = bookingHeders.Where(u => u.BookingStatus == SD.StatusApprove);
					break;
				default:
					break;
			}


			return Json(new { data = bookingHeders });
		}

		#endregion
	}
}
