using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using e_commerce.Infrastructure.Repository;
using e_commerce.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using e_commerce.Infrastructure.Entites;

using Stripe;
using Stripe.Climate;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace e_commerce.Web.Controllers
{
    [Authorize(Roles = "Customer")]
    [ServiceFilter(typeof(LayoutDataFilterAttribute))]

    public class _OrdersController : Controller
    {
        public IOrderRepository IOrderRepo { get; }
        private readonly IcartRepository repo;
        private readonly IAdressRepo ADDrepo;
        private readonly ICustRepo custrepo;

        // GET: orderController
        public _OrdersController(IOrderRepository _orderrepo, IcartRepository _repo, IAdressRepo aDDrepo, ICustRepo custrepo)
        {
            IOrderRepo = _orderrepo;
            this.repo = _repo;
            ADDrepo = aDDrepo;
            this.custrepo = custrepo;
        }
        public ActionResult getAllCustOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(IOrderRepo.viewAllOrders(custrepo.getcustomerid(userId).Id));
        }


        // GET: orderController/Details/5
        public ActionResult Details(int custID, int orderID, int addressID)
        {
            Infrastructure.Entites.Order order = IOrderRepo.viewCustOrder(custID, orderID);
            ViewBag.isFirstTime = (order.TotalPrice == order.OrderProducts.Sum(op => op.ItemTotal) ? true : false);
            ViewBag.Addresse = ADDrepo.GetAddressById(addressID, custID);
            return View(order);
        }

        // GET: orderController/Create
        [HttpPost]
        public ActionResult Create([FromBody] OrderData data)
        {
            decimal shippingFees = 50;
            var cart_ = repo.GetCartByCustomerId(data.customerID);
            decimal total;
            if (IOrderRepo.viewAllOrders(data.customerID).Count != 0)
                total = cart_.TotalPrice + shippingFees;
            else
                total = cart_.TotalPrice;
            Infrastructure.Entites.Order order = new Infrastructure.Entites.Order
            {
                CustomerId = data.customerID,
                ShippingAddressId = data.shippingID,
                TotalPrice = total,
                OrderDate = DateTime.Now,
                PaymentMethod = Domain.Enums.PaymentMethod.cash,
                Status = (Domain.Enums.orderstateEnum)Domain.Enums.PaymentStatusEnum.PaymentPending,
                OrderProducts = cart_.CartProducts.Select(cp => new OrderProduct
                {
                    ProductId = cp.ProductCode,
                    Quantity = cp.Quantity,
                    UnitPrice = cp.UnitPrice,
                    ItemTotal = cp.ItemTotal
                }).ToList()
            };

            IOrderRepo.AddOrder(order);
            repo.RemoveAllFromCart(cart_.Id, data.customerID);
            return Json(new
            {
                success = true,
                redirectUrl = Url.Action("OrderSuccess", "_Orders")
            });
        }



        // GET: orderController/Edit/5
        public ActionResult OrderSuccess()
        {
            return View("OrderCreated");
        }

        // POST: orderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

      
        // POST: orderController/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var canceledOrder = IOrderRepo.GetOrderById(id);
            if (DateTime.Now.Day - canceledOrder.OrderDate.Value.Day <= 3 && canceledOrder.OrderDate.Value.Year == DateTime.Now.Year && DateTime.Now.Month == canceledOrder.OrderDate.Value.Month)
            {
                if (canceledOrder.PaymentStatus == Domain.Enums.PaymentStatusEnum.Paid)
                {
                    try
                    {
                        return Json(new { success = true, message = "Refund requested. Proceed with the refund process." });
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
                    }
                }
                else if (canceledOrder.PaymentStatus == Domain.Enums.PaymentStatusEnum.PaymentPending)
                {
                    IOrderRepo.DeleteOrder(id);
                    return Json(new { success = true, message = "Your order has been cancelled successfully." });
                }
            }
            return Json(new { success = false, message = "You cannot cancel the order after 3 days." });
        }
    }
}