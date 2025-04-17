using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Web.Controllers
{
    public class returnsController : Controller
    {
        // GET: returnsController
        public ActionResult returnsIndexForm()
        {
            return View();
        }

        // GET: returnsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: returnsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: returnsController/Create
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
