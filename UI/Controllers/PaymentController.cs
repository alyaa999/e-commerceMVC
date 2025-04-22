using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using e_commerce.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Session = Stripe.Checkout.Session;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;

namespace e_commerce.Web.Controllers
{
    [Authorize(Roles = "Customer")]
    [ServiceFilter(typeof(LayoutDataFilterAttribute))]

    public class PaymentController : Controller
    {
        private readonly IcartRepository repo;
        IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;
        private readonly IRepository<Return> _returnRepository;
        public PaymentController(IConfiguration configuration,IcartRepository icartRepository,IOrderRepository orderRepository, IRepository<Return> returnRepository)
        {
            _configuration = configuration;
            repo = icartRepository;
            _orderRepository = orderRepository;
            _returnRepository = returnRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Checkout([FromBody] OrderData data)
        {
            var userEmail = User.Identity.IsAuthenticated
            ? User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value
            : null;

            decimal shippingFees = 50;
            var cart_ = repo.GetCartByCustomerId(data.customerID);
            decimal total;
            if (_orderRepository.viewAllOrders(data.customerID).Count != 0)
                total = cart_.TotalPrice + shippingFees;
            else
                total = cart_.TotalPrice;

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
                    UnitAmountDecimal = total * 100 
                },

                Quantity = 1
            }
            },
                Mode = "payment",
                SuccessUrl = _configuration["Stripe:SuccessUrl"] + "?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = _configuration["Stripe:CancelUrl"],
                CustomerEmail = userEmail,
                Metadata = new Dictionary<string, string>
        {
            { "CustomerId", data.customerID.ToString() },
            { "ShippingId", data.shippingID.ToString() },
            { "Total", total.ToString() }
        },
               
            };

            var service = new SessionService();
            var session = service.Create(options);

            return Json(session.Url);
        }


        public IActionResult Success(string session_id)
        {
            var service = new SessionService();
            var session = service.Get(session_id);

            if (session.PaymentStatus != "paid")
            {
                return RedirectToAction("Cancel");
            }

            var customerId = int.Parse(session.Metadata["CustomerId"]);
            var shippingId = int.Parse(session.Metadata["ShippingId"]);
            var total = decimal.Parse(session.Metadata["Total"]);
            var cart_ = repo.GetCartByCustomerId(customerId);
            var paymentIntentId = session.PaymentIntentId;

            var order = new Order
            {
                CustomerId = customerId,
                ShippingAddressId = shippingId,
                TotalPrice = total,
                OrderDate = DateTime.Now,
                PaymentMethod =Domain.Enums.PaymentMethod.card,
                Status = (Domain.Enums.orderstateEnum)Domain.Enums.orderstateEnum.Pending,
                PaymentStatus = Domain.Enums.PaymentStatusEnum.Paid,
                PaymentIntentId = paymentIntentId,
                OrderProducts = cart_.CartProducts.Select(cp => new OrderProduct
                {
                    ProductId = cp.ProductCode,
                    Quantity = cp.Quantity,
                    UnitPrice = cp.UnitPrice,
                    ItemTotal = cp.ItemTotal
                }).ToList() 
            };

            _orderRepository.AddOrder(order);
            repo.RemoveAllFromCart(cart_.Id, customerId);


            return View("OrderCreated"); 
        }

        public IActionResult Cancel()
        {
            return View("CancelPayment");
        }
        [HttpPost]
        public IActionResult Refund(int orderId)
        {
            var order = _orderRepository.GetOrderById(orderId);
            if (order == null || string.IsNullOrEmpty(order.PaymentIntentId))
            {
                return Json(new { success = false, message = "Order with this PaymentIntent not found." });
            }

            try
            {
                var refundOptions = new Stripe.RefundCreateOptions
                {
                    PaymentIntent = order.PaymentIntentId,
                    Amount = (long)(order.TotalPrice * 100),
                    Reason = "requested_by_customer"
                };

                var refundService = new Stripe.RefundService();
                var refund = refundService.Create(refundOptions);

                order.PaymentStatus = Domain.Enums.PaymentStatusEnum.Refunded;
                order.Status = Domain.Enums.orderstateEnum.Cancelled;
                _orderRepository.UpdateOrder(order);

                return Json(new { success = true, message = $"Your money has been refunded (${order.TotalPrice}) successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error during refund: {ex.Message}" });
            }
        }
        //marwa

        [HttpGet]
        public async Task<IActionResult> ProcessRefund(int orderId, int returnId)
        {
            var order = _orderRepository.GetOrderById(orderId);
            var returnRequest = await _returnRepository.GetByIdAsync(returnId);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction("Index", "AdminReturns");
            }

            
            return View(new RefundViewModel
            {
                OrderId = orderId,
                ReturnId = returnId,
                TotalPrice = returnRequest.AmountRefunded
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ProcessRefund(RefundViewModel model)
        {
            Stripe.StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
            var order = _orderRepository.GetOrderById(model.OrderId);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Order not found.";
                return RedirectToAction("Index", "AdminReturns");
            }

            try
            {
                if (string.IsNullOrEmpty(order.PaymentIntentId))
                {
                    TempData["ErrorMessage"] = "Cannot process refund - payment information missing";
                    return RedirectToAction("Details", "AdminReturns", new { id = model.ReturnId });
                }

                var refundOptions = new Stripe.RefundCreateOptions
                {
                    PaymentIntent = order.PaymentIntentId,
                    Amount = (long)(model.TotalPrice * 100),
                    Reason = "requested_by_customer"
                };

                var refundService = new Stripe.RefundService();
                var refund = refundService.Create(refundOptions);

                order.PaymentStatus = Domain.Enums.PaymentStatusEnum.Refunded;
                _orderRepository.UpdateOrder(order);

                TempData["SuccessMessage"] = $"Successfully refunded ${order.TotalPrice}";
                return RedirectToAction("Details", "AdminReturns", new { id = model.ReturnId });
            }
            catch (StripeException ex)
            {
                TempData["ErrorMessage"] = $"Stripe error: {ex.Message}";
                return RedirectToAction("Details", "AdminReturns", new { id = model.ReturnId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("Index", "AdminReturns");
            }
        }

    }
}
/*
 * Stripe.StripeConfiguration.ApiKey = "sk_test_1234567890abcdef";
 * */