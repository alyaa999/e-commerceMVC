using e_commerce.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Web.Controllers
{
    public class orderController : Controller
    {
        public IOrderRepository IOrderRepo { get; }

        // GET: orderController
        public orderController(IOrderRepository _orderrepo)
        {
            IOrderRepo=_orderrepo;
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: orderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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
