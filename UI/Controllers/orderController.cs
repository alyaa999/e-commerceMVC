using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using e_commerce.Infrastructure.Repository;
using e_commerce.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using e_commerce.Infrastructure.Entites;

using Stripe;
using Stripe.Climate;

namespace e_commerce.Web.Controllers
{
    public class orderController : Controller
    {
        public IOrderRepository IOrderRepo { get; }
        private readonly IcartRepository repo;
        private readonly IAdressRepo ADDrepo;

        // GET: orderController
        public orderController(IOrderRepository _orderrepo, IcartRepository _repo, IAdressRepo aDDrepo)
        {
            IOrderRepo = _orderrepo;
            this.repo = _repo;
            ADDrepo = aDDrepo;
        }
        public ActionResult getAllCustOrder()
        {
            
            return View(IOrderRepo.viewAllOrders(1));
        }


        // GET: orderController/Details/5
        public ActionResult Details(int custID,int orderID,int addressID)
        {
            Infrastructure.Entites.Order order = IOrderRepo.viewCustOrder(custID, orderID);
            ViewBag.isFirstTime = (order.TotalPrice==order.OrderProducts.Sum(op=>op.ItemTotal) ? true : false);
            ViewBag.Addresse = ADDrepo.GetAddressById(addressID,custID);
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
                Status = Domain.Enums.orderstateEnum.PaymentPending,
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
            return View();
        }

       

        // GET: orderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
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

        // GET: orderController/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var canceledOrder = IOrderRepo.GetOrderById(id);
            if (DateTime.Now.Day - canceledOrder.OrderDate.Value.Day <= 3 && canceledOrder.OrderDate.Value.Year == DateTime.Now.Year && DateTime.Now.Month == canceledOrder.OrderDate.Value.Month)
            {
                if (canceledOrder.Status == Domain.Enums.orderstateEnum.Paid)
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
                else if (canceledOrder.Status == Domain.Enums.orderstateEnum.PaymentPending)
                {
                    IOrderRepo.DeleteOrder(id);
                    return Json(new { success = true, message = "Your order has been cancelled successfully." });
                }
            }
            return Json(new { success = false, message = "You cannot cancel the order after 3 days." });
        }



    }
}
