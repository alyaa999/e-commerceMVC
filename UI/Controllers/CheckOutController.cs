using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using e_commerce.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace e_commerce.Web.Controllers
{
    [Authorize(Roles = "Customer")]
    [ServiceFilter(typeof(LayoutDataFilterAttribute))]

    public class CheckOutController : Controller
    {
        // GET: CheckOutController
        private IcartRepository cartreposervice;
        private readonly IAdressRepo repo;
        private readonly IOrderRepository OrderRepo;
        private ICustRepo custrepo;

        public CheckOutController(IcartRepository _cartreposervice, IAdressRepo adressRepo,IOrderRepository _orderRepo, ICustRepo _custrepo)
        {
            cartreposervice = _cartreposervice;
            repo = adressRepo;
            OrderRepo = _orderRepo;
            custrepo = _custrepo;
        }
        public ActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Cart cart = cartreposervice.GetCartByCustomerId(custrepo.getcustomerid(userId).Id);
            ViewBag.isFirstTime = (OrderRepo.viewAllOrders(custrepo.getcustomerid(userId).Id).Count==0?true:false);
            ViewBag.Addresses = repo.GetAllAddressAsync(custrepo.getcustomerid(userId).Id);
            return View(cart);
        }

        // GET: CheckOutController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CheckOutController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckOutController/Create
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

        // GET: CheckOutController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheckOutController/Edit/5
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

        // GET: CheckOutController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckOutController/Delete/5
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
