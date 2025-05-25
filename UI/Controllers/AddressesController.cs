using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_commerce.Infrastructure.Entites;
using AutoMapper;
using e_commerce.Application.Common.Interfaces;
using e_commerce.Web.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace e_commerce.Web.Controllers
{
    [Authorize(Roles = "Customer")]
    [ServiceFilter(typeof(LayoutDataFilterAttribute))]
    public class AddressesController : Controller
    {
        List<string> cities;
        private readonly IMapper mapper;
        private readonly IAdressRepo repo;
        private ICustRepo custrepo;
        public AddressesController(IMapper mapper, IAdressRepo adressRepo, ICustRepo _custrepo)
        {
            this.mapper = mapper;
            repo = adressRepo;
            cities = new List<string>
            {
                "Cairo", "Alexandria", "Giza", "Shubra El Kheima", "Port Said",
                "Suez", "Mansoura", "Tanta", "Aswan", "Fayoum", "Luxor", "Ismailia"
            };
            custrepo = _custrepo;
        }

        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(repo.GetAllAddressAsync(custrepo.getcustomerid(userId).Id));
        }

        // GET: Addresses/Create
        public IActionResult Create(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Cities = new SelectList(cities);
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddressVM address,string? returnUrl)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (ModelState.IsValid)
                {
                    try
                    {
                        repo.AddAddressAsync(mapper.Map<Address>(address), custrepo.getcustomerid(userId).Id);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                        ViewBag.Cities = new SelectList(cities);
                        return View(mapper.Map<Address>(address));
                    }
                }
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
            return View(address);
            }
            
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(int id,string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id == null)
            {
                return NotFound();
            }
            var address = repo.GetAddressById(id, custrepo.getcustomerid(userId).Id);
            ViewBag.Cities = new SelectList(cities);
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddressVM address,string? returnUrl)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var address_ = mapper.Map<Address>(address);
            try{
               if (ModelState.IsValid)
               {
                    try
                    {
                       address_.Id = id;
                       address_.CustomerId = custrepo.getcustomerid(userId).Id;
                       repo.UpdateAddress(address_, custrepo.getcustomerid(userId).Id, id);
                   }
                   catch (Exception ex)
                   {
                        ModelState.AddModelError("", ex.Message);
                        ViewBag.Cities = new SelectList(cities);
                        return View(address_);
                    }
                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index");
               }
               return View(address_);

            }
            catch(Exception ex)
            {
                return View(address);
            }
        }

        // GET: Addresses/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (id != null)
                {
                    repo.DeleteAddressAsync(id, custrepo.getcustomerid(userId).Id);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }

            return Json(new { success = true });
        }
        public async Task<IActionResult> checkifAddAlreadyAttatchToOrd(int id)
        {

            var result = repo.isAddressConnectedToOrder(id);
            return Json(result);
        }

    }
}
