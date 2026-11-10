using FocusPoint.DataAccess.Repository.IRepository;
using FocusPoint.Models;
using FocusPoint.Models.ViewModel;
using FocusPoint.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace FocusPointWeb.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class HireController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public HireVM HireVM { get; set; }
        //public int OrderTotal { get; set; }
        public HireController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }
        public IActionResult Index( DateTime hireDate)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);



            HireVM = new HireVM()
            {
                ListHire = _unitOfWork.Hire.GetAll(
                    u => u.ApplicationUserId == claim.Value, includeProperties: "Packege"),
                BookingHeder = new()
            };

            foreach( var item in HireVM.ListHire)
            {
                item.Price = GetPriceBasedOnDays(item.Days, item.Packege.Price, item.Packege.Price3,
                    item.Packege.Price6, item.Packege.Price10);
                HireVM.BookingHeder.BookingTotal += (item.Price * item.Days);
                ;
            }

            return View(HireVM);
        }





        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            HireVM = new HireVM()
            {
                ListHire = _unitOfWork.Hire.GetAll(
                    u => u.ApplicationUserId == claim.Value, includeProperties: "Packege"),
                BookingHeder = new()
            };
            HireVM.BookingHeder.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(
                u => u.Id == claim.Value);


			HireVM.BookingHeder.Name = HireVM.BookingHeder.ApplicationUser.Name;
            HireVM.BookingHeder.PhoneNumber = HireVM.BookingHeder.ApplicationUser.PhoneNumber;
            HireVM.BookingHeder.Address = HireVM.BookingHeder.ApplicationUser.Address;
            HireVM.BookingHeder.City = HireVM.BookingHeder.ApplicationUser.City;
            HireVM.BookingHeder.District = HireVM.BookingHeder.ApplicationUser.District;
            HireVM.BookingHeder.PostalCOde = HireVM.BookingHeder.ApplicationUser.PostalCode;
            


			foreach (var hire in HireVM.ListHire)
			{
				hire.Price = GetPriceBasedOnDays(hire.Days, hire.Packege.Price,
					hire.Packege.Price3, hire.Packege.Price6, hire.Packege.Price10);
				HireVM.BookingHeder.BookingTotal += (hire.Price * hire.Days);
			}
			return View(HireVM);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
		public IActionResult SummaryPOST(HireVM HireVM)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            HireVM.ListHire = _unitOfWork.Hire.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "Packege");


            
            HireVM.BookingHeder.EventDate = System.DateTime.Now;
            HireVM.BookingHeder.ApplicationUserId = claim.Value;
            
		
			foreach (var hire in HireVM.ListHire)
			{
				hire.Price = GetPriceBasedOnDays(hire.Days, hire.Packege.Price,
					hire.Packege.Price3, hire.Packege.Price6, hire.Packege.Price10);
				HireVM.BookingHeder.BookingTotal += (hire.Price * hire.Days);
			}
			ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);
			if (applicationUser.Type == 0)
			{
				HireVM.BookingHeder.PaymentStatus = SD.PaymentStatusPending;
				HireVM.BookingHeder.BookingStatus = SD.StatusPending;
            }
            else
            {
				HireVM.BookingHeder.PaymentStatus = SD.PaymentStatusDelayedPayment;
				HireVM.BookingHeder.BookingStatus = SD.StatusApprove;
			}

			_unitOfWork.BookingHeader.Add(HireVM.BookingHeder);
            _unitOfWork.Save();

			foreach (var hire in HireVM.ListHire)
			{
                BookingDetails bookingDetails = new()
                {
                    PackegeId = hire.PackegesId,
                    BookingId= HireVM.BookingHeder.Id,
                    Price = hire.Price,
                    Days = hire.Days
                };
                _unitOfWork.BookingDetails.Add(bookingDetails);
                _unitOfWork.Save();
            }

			if (applicationUser.Type == 0)
            {
				//STRIPE
				var domain = "https://localhost:44358/";
				var options = new SessionCreateOptions
				{
					PaymentMethodTypes = new List<string>
				{
					"card",
				},
					LineItems = new List<SessionLineItemOptions>(),

					Mode = "payment",
					SuccessUrl = domain + $"user/hire/OrderConfirmation?id={HireVM.BookingHeder.Id}",
					CancelUrl = domain + $"user/hire/index",
				};
				foreach (var item in HireVM.ListHire)
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


				_unitOfWork.BookingHeader.UpdateStripePaymentID(HireVM.BookingHeder.Id, session.Id, session.PaymentIntentId);
				_unitOfWork.Save();

				Response.Headers.Add("Location", session.Url);
				return new StatusCodeResult(303);
			}
            else
            {
                return RedirectToAction("OrderConfirmation", "hire", new { id = HireVM.BookingHeder.Id });
            }




			/*_unitOfWork.Hire.RemoveRange(HireVM.ListHire);
            _unitOfWork.Save();

			return RedirectToAction("Index","PhotographyPackege");*/
		}


		public IActionResult OrderConfirmation(int id)
		{
			BookingHeder bookingHeder = _unitOfWork.BookingHeader.GetFirstOrDefault(u => u.Id == id, includeProperties: "ApplicationUser");
			
            if(bookingHeder.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
				var service = new SessionService();
				Session session = service.Get(bookingHeder.SessionId);
				//Check Stripe STSTUS
				if (session.PaymentStatus.ToLower() == "paid")
				{
					_unitOfWork.OrderHeader.UpdateStripePaymentID(id, bookingHeder.SessionId, session.PaymentIntentId);
					_unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApprove, SD.PaymentStatusApproved);
					_unitOfWork.Save();

				}
			}

           
			List<Hire> hire = _unitOfWork.Hire.GetAll(u => u.ApplicationUserId == bookingHeder.ApplicationUserId).ToList();
            _emailSender.SendEmailAsync(bookingHeder.ApplicationUser.Email, "New Booking - Focus Point", "<p>Your booking is confirm</p>");
            //HttpContext.Session.Clear();
            _unitOfWork.Hire.RemoveRange(hire);
			_unitOfWork.Save();

			return View(id);

		}



		public IActionResult Plus(int hireId)
        {
            var hire = _unitOfWork.Hire.GetFirstOrDefault(u => u.Id == hireId);
            _unitOfWork.Hire.IncrementCount(hire, 1);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        public IActionResult Minus(int hireId)
        {
            var hire = _unitOfWork.Hire.GetFirstOrDefault(u => u.Id == hireId);
            if(hire.Days <= 1)
            {
                _unitOfWork.Hire.Remove(hire);
            }
            else
            {
                _unitOfWork.Hire.DecrementCount(hire, 1);
            }
            
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        
        public IActionResult Remove(int hireId)
        {
            var hire = _unitOfWork.Hire.GetFirstOrDefault(u => u.Id == hireId);
            _unitOfWork.Hire.Remove(hire);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }




        private double GetPriceBasedOnDays(double day, double price, double price3, double price6, double price10)
        {
            if(day <= 2)
            {
                return price;
            }
            else
            {
                if(day <= 6)
                {
                    return price3;
                }
                else
                {
                    if(day <= 10)
                    {
                        return price6;
                    }
                    else
                    {
                        return price10;
                    }
                }
            }
           
        }

	}
}
