using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using e_commerce.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace e_commerce.Web.Controllers
{
    public class orderController : Controller
    {
        public IOrderRepository IOrderRepo { get; }
        private readonly IcartRepository repo;

        // GET: orderController
        public orderController(IOrderRepository _orderrepo, IcartRepository _repo)
        {
            IOrderRepo = _orderrepo;
            this.repo = _repo;
        }
        public ActionResult getAllCustOrder()
        {
            
            return View(IOrderRepo.viewAllOrders(1));
        }
       

        // GET: orderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
            Order order = new Order
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: orderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
