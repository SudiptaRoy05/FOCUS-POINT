using FocusPoint.DataAccess.Repository;
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
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, 
                includeProperties: "MediaItem"),
                OrderHeader = new()
            };
            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = cart.MediaItem.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price;
            }
            return View(ShoppingCartVM);
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32(SD.SessionCart,count);
            return RedirectToAction("Index");
        }


        //Summary Get
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "MediaItem"),
                OrderHeader = new()
            };
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(
                u => u.Id == claim.Value);

            var applicationUser = ShoppingCartVM.OrderHeader.ApplicationUser;
            if (applicationUser != null)
            {
                ShoppingCartVM.OrderHeader.Name = applicationUser.Name;
                ShoppingCartVM.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
                ShoppingCartVM.OrderHeader.Address = applicationUser.Address;
                ShoppingCartVM.OrderHeader.City = applicationUser.City;
                ShoppingCartVM.OrderHeader.District = applicationUser.District;
                ShoppingCartVM.OrderHeader.PostalCOde = applicationUser.PostalCode;
            }

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = cart.MediaItem.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price;
            }
            return View(ShoppingCartVM); 
        }

        //Summary Post
        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value,
                includeProperties: "MediaItem");

            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.MediaItem.Price);
            }


            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    MediaItemId = cart.MediaItemId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.MediaItem.Price,
                    //Count = cart.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }




            //Stripe setting
            var domain = "https://localhost:44358/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>(),

                Mode = "payment",
                SuccessUrl = domain + $"user/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"user/cart/index",
            };
            foreach (var item in ShoppingCartVM.ListCart)
            {

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.MediaItem.Price),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.MediaItem.FileName,
                        },
                    },
                    Quantity = 1,
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            _unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);


            /* _unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
             _unitOfWork.Save();
             return RedirectToAction("Index","Gallery");*/
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id, includeProperties: "ApplicationUser");
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            //Check Stripe STSTUS
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateStripePaymentID(id, orderHeader.SessionId, session.PaymentIntentId);
                _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApprove, SD.PaymentStatusApproved);
                _unitOfWork.Save();
            }
           // _emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order - LensCrafted", "<p>New Order Created</p>");
            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            foreach (var cartItem in shoppingCarts)
            {
                var purchasedItem = new PurchasedItem
                {
                    MediaItemId = cartItem.MediaItemId,
                    ApplicationUserId = cartItem.ApplicationUserId
                };

                _unitOfWork.PurchasedItem.Add(purchasedItem);
                _unitOfWork.Save();
            }
            _emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order - Focus Point", "<p>Thank You for purchase</p>");
            HttpContext.Session.Clear();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(id);
        }
    }
}
