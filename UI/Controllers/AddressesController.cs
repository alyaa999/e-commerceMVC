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

namespace e_commerce.Web.Controllers
{
    public class AddressesController : Controller
    {
        List<string> cities;
        private readonly IMapper mapper;
        private readonly IAdressRepo repo;
        public AddressesController(IMapper mapper, IAdressRepo adressRepo)
        {
            this.mapper = mapper;
            repo = adressRepo;
            cities = new List<string>
            {
                "Cairo", "Alexandria", "Giza", "Shubra El Kheima", "Port Said",
                "Suez", "Mansoura", "Tanta", "Aswan", "Fayoum", "Luxor", "Ismailia"
            };
        }

        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            return View(repo.GetAllAddressAsync());
        }

        // GET: Addresses/Create
        public IActionResult Create()
        {
            ViewBag.Cities = new SelectList(cities);
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddressVM address)
        {
            if (ModelState.IsValid)
            {
                repo.AddAddressAsync(mapper.Map<Address>(address), 1);
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var address = repo.GetAddressById(id, 1);
            ViewBag.Cities = new SelectList(cities);
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddressVM address)
        {
            var address_ = mapper.Map<Address>(address);
            try{
               if (ModelState.IsValid)
               {
                    try
                    {
                       address_.Id = id;
                       address_.CustomerId = 1;
                       repo.UpdateAddress(address_,1,id);
                   }
                   catch (Exception ex)
                   {
                       ModelState.AddModelError("", ex.Message);
                       return View(address_);
                    }
                   return RedirectToAction(nameof(Index));
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
                if (id != null)
                {
                    repo.DeleteAddressAsync(id,1);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }

            return Json(new { success = true });
        }
        
    }
}
