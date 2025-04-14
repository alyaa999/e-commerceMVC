using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using Microsoft.AspNetCore.Mvc;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System;
using Session = Stripe.Checkout.Session;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;

namespace e_commerce.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IcartRepository repo;
        IConfiguration _configuration;
        public PaymentController(IConfiguration configuration,IcartRepository icartRepository)
        {
            _configuration = configuration;
            repo = icartRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Checkout(int shippingID, int customerID)
        {
            decimal shippingFees = 50;
            var cart_ = repo.GetCartByCustomerId(customerID);
            decimal total = cart_.TotalPrice + shippingFees;

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
            {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Shopping Cart Order"
                    },
                    UnitAmountDecimal = total * 100 // Stripe requires amount in cents
                },

                Quantity = 1
            }
        },
                Mode = "payment",
                SuccessUrl = _configuration["Stripe:SuccessUrl"] + "?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = _configuration["Stripe:CancelUrl"],
                CustomerEmail = "aliaa@gmail.com",
                Metadata = new Dictionary<string, string>
        {
            { "CustomerId", customerID.ToString() },
            { "ShippingId", shippingID.ToString() },
            { "Total", total.ToString() }
        }
            };

            var service = new SessionService();
            var session = service.Create(options);

            return Redirect(session.Url);
        }


        public IActionResult Success(string session_id)
        {
            var service = new SessionService();
            var session = service.Get(session_id);

            if (session.PaymentStatus != "paid")
            {
                // الحالة مش مدفوعة، نرجع على Error View
                return RedirectToAction("Cancel");
            }

            // قراءة البيانات من Metadata
            var customerId = int.Parse(session.Metadata["CustomerId"]);
            var shippingId = int.Parse(session.Metadata["ShippingId"]);
            var total = decimal.Parse(session.Metadata["Total"]);

            // إنشاء الطلب
            var order = new Order
            {
                CustomerId = customerId,
                ShippingAddressId = shippingId,
                TotalPrice = total,
                OrderDate = DateTime.Now,
                Status = 1 
            };

            //repo.AD(order); // هنا بتضيفي الطلب لقاعدة البيانات
            ////using (var context = new YourDbContext()) // لو مش بتستخدمي DI هنا
            ////{
            ////    context.Orders.Add(order);
            ////    context.SaveChanges();
            ////}

            return View("SuccessPayment"); // ممكن تعملي صفحة شكرًا هنا
        }

        public IActionResult Cancel()
        {
            return View("CancelPayment");
        }
    } 
}
