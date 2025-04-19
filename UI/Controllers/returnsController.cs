using e_commerce.Application.Common.Interfaces;
using e_commerce.Domain.Enums;
using e_commerce.Infrastructure.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace e_commerce.Web.Controllers
{
    public class returnsController : Controller
    {
        // GET: returnsController
        IOrderRepository orderRepository;
        IReturnRepository returnRepository;
        public returnsController(IOrderRepository _repo,IReturnRepository repository)
        {
            orderRepository = _repo;
            returnRepository = repository;
        } 
        public ActionResult returnsIndexForm()
        {
            return View(returnRepository.getAllCustomerReturns(1));
        }

        // GET: returnsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: returnsController/Create
        public ActionResult Create()
        {
            ViewBag.OrderId = new SelectList(returnRepository.getOrdersCanReturn(1), "Id", "Id");
            return View();
        }
        [HttpGet]
        public JsonResult GetProductsByOrderId(int orderId)
        {
            var order = orderRepository.getOrderByOrderID(1, orderId);
            var products = order?.OrderProducts.Select(p => new { p.ProductId, p.Product.Name }).ToList();
            if (products == null) return Json(new List<object>());
            return Json(products);
        }

        // POST: returnsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int OrderId, List<int> ProductId, List<string> Reason)
        {
            try
            {
                var returnList = new List<Return>();
                for (int i = 0; i < ProductId.Count; i++)
                {
                    var returnRequest = new Return
                    {
                        OrderId = OrderId,
                        ProductId = ProductId[i],
                        Reason = Reason[i],
                        custId = 1, // Assuming a static customer ID for now
                        Status = ReturnStatusEnum.Pending,
                        ReturnDate = DateTime.Now,
                    };
                    returnList.Add(returnRequest);
                }
                    returnRepository.AddReturnRequest(returnList);
                    return RedirectToAction(nameof(returnsIndexForm));
            }
            catch
            {
                return View();
            }
        }

        // GET: returnsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: returnsController/Edit/5
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

        // GET: returnsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: returnsController/Delete/5
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
