using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using e_commerce.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Web.Controllers
{
    public class CartController : Controller
    {
        private IcartRepository cartreposervice;


        public CartController(IcartRepository _cartreposervice) {
            cartreposervice = _cartreposervice;
        }
        // GET: CartController
        public ActionResult viewcartproducts()
        {
            Cart cart = cartreposervice.GetCartByCustomerId(1);
            return View(cart);
        }

        // GET: CartController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CartController/Create
        public ActionResult Create()
        { 

            return View();
        }

        // POST: CartController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddItemToCart(int productId)
        {
            Cart cart = cartreposervice.GetCartByCustomerId(1);
            cartreposervice.AddItemToCart(cart.Id,productId,1);
            return Json(new
            {
                success = true,
            });
        }

        // GET: CartController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CartController/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateCart([FromBody] List<CartUpdateDto> updates)
        {
            try
            {

                foreach (var item in updates)
                {
                    cartreposervice.UpdateItemQuantity(
                        1,
                        item.ProductId,
                        item.Quantity
                    );
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // GET: CartController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CartController/Delete/5
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
