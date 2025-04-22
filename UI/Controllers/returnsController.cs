using e_commerce.Application.Common.Interfaces;
using e_commerce.Domain.Enums;
using e_commerce.Infrastructure.Entites;
using e_commerce.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Security.Claims;

namespace e_commerce.Web.Controllers
{
    [Authorize(Roles = "Customer")]
    [ServiceFilter(typeof(LayoutDataFilterAttribute))]

    public class returnsController : Controller
    {

        // GET: returnsController
        IOrderRepository orderRepository;
        IReturnRepository returnRepository;
        private ICustRepo custrepo;
        public returnsController(IOrderRepository _repo,IReturnRepository repository, ICustRepo _custrepo)
        {
            orderRepository = _repo;
            returnRepository = repository;
            custrepo = _custrepo;
        } 
        public ActionResult returnsIndexForm()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(returnRepository.getAllCustomerReturns(custrepo.getcustomerid(userId).Id));
        }

        // GET: returnsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: returnsController/Create
        public ActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.OrderId = new SelectList(returnRepository.getOrdersCanReturn(custrepo.getcustomerid(userId).Id), "Id", "Id");
            return View();
        }
        [HttpGet]
        public JsonResult GetProductsByOrderId(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = orderRepository.getOrderByOrderID(custrepo.getcustomerid(userId).Id, orderId);
            var products = order?.OrderProducts.Select(p => new { p.ProductId, p.Product.Name }).ToList();
            if (products == null) return Json(new List<object>());
            return Json(products);
        }

        // POST: returnsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int OrderId, List<int> ProductId, List<string> Reason)
        {
            decimal amountOfReturns = 0;
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var returnList = new List<Return>();
                for (int i = 0; i < ProductId.Count; i++)
                {
                    amountOfReturns += orderRepository.getOrderByOrderID((custrepo.getcustomerid(userId).Id), OrderId).OrderProducts
                        .Where(p => p.ProductId == ProductId[i]).FirstOrDefault().ItemTotal;
                    var returnRequest = new Return
                    {
                        OrderId = OrderId,
                        ProductId = ProductId[i],
                        Reason = Reason[i],
                        custId = custrepo.getcustomerid(userId).Id, // Assuming a static customer ID for now
                        Status = ReturnStatusEnum.Pending,
                        ReturnDate = DateTime.Now,
                        AmountRefunded = amountOfReturns
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
