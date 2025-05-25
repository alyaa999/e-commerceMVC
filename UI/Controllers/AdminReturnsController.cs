using Microsoft.AspNetCore.Mvc;

using e_commerce.Infrastructure.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using e_commerce.Application.Common.Interfaces;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;

namespace e_commerce.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminReturnsController : Controller
    {
        private readonly IRepository<Return> _returnRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly ILogger<AdminReturnsController> _logger;
        public AdminReturnsController(
            IRepository<Return> returnRepository,
            IRepository<Order> orderRepository,
            IRepository<Product> productRepository,
            ILogger<AdminReturnsController> logger)
        {
            _returnRepository = returnRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _logger = logger;
        }


        // GET: Admin/AdminReturns
        public async Task<IActionResult> Index()
        {
            // Debugging: Log the query execution
            _logger.LogInformation("Fetching returns data from repository");

            Expression<Func<Return, object>>[] includes =
            {
        r => r.Order,
        r => r.Product,
    };

            var returns = await _returnRepository.GetAllIncludingAsync(includes);

            // Debugging: Log the raw results
            _logger.LogInformation($"Found {returns.Count()} returns");
            foreach (var r in returns)
            {
                _logger.LogInformation($"Return ID: {r.Id}, Order: {r.Order?.Id}, Product: {r.Product?.Id}");
            }

            returns = returns.OrderByDescending(r => r.ReturnDate);
            return View(returns);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

      
            Expression<Func<Return, object>>[] includes =
            {
        r => r.Order,
        r => r.Order.Customer,
        r => r.Order.Customer.ApplicationUser, 
        r => r.Product,
    };

            var returnRequest = (await _returnRepository.FindAsync(r => r.Id == id, includes))
                .FirstOrDefault();

            if (returnRequest == null)
            {
                return NotFound();
            }

            return View(returnRequest);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Approve(int id)
        //{
        //    var returnRequest = await _returnRepository.GetByIdAsync(id);
        //    if (returnRequest == null)
        //    {
        //        return NotFound();
        //    }

        //    returnRequest.Status = Domain.Enums.ReturnStatusEnum.Approved;
        //    returnRequest.ReturnDate = DateTime.Now;
        //    _returnRepository.Update(returnRequest);
        //    await _returnRepository.SaveChangesAsync();


        //    TempData["SuccessMessage"] = "Return request has been approved successfully.";
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, string rejectionReason)
        {
            var returnRequest = await _returnRepository.GetByIdAsync(id);
            if (returnRequest == null)
            {
                return NotFound();
            }

            returnRequest.Status = Domain.Enums.ReturnStatusEnum.Rejected;
            returnRequest.Reason += $"\n\nRejection Reason: {rejectionReason}";
            _returnRepository.Update(returnRequest);
            await _returnRepository.SaveChangesAsync();

            // Here you would typically add logic for notifying customer

            TempData["SuccessMessage"] = "Return request has been rejected.";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var returnRequest = await _returnRepository.GetByIdAsync(id);
            if (returnRequest == null)
            {
                TempData["ErrorMessage"] = "Return request not found.";
                return RedirectToAction(nameof(Index));
            }

            returnRequest.Status = Domain.Enums.ReturnStatusEnum.Approved;
            returnRequest.ReturnDate = DateTime.Now;
            _returnRepository.Update(returnRequest);
            await _returnRepository.SaveChangesAsync();

            
            return RedirectToAction("ProcessRefund", "Payment", new
            {
                orderId = returnRequest.OrderId,
                returnId = returnRequest.Id
            });

        }
    }
}
