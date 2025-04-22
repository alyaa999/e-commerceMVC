using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using e_commerce.Application.Common.Interfaces;
using e_commerce.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using e_commerce.Infrastructure.Entites;
using e_commerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace e_commerce.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<Customer> _customerRepo;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(
            IRepository<Order> orderRepo,
            ILogger<OrdersController> logger,
            IRepository<Customer> customerRepo)
        {
            _orderRepo = orderRepo;
            _logger = logger;
            _customerRepo = customerRepo;
        }

        public async Task<IActionResult> Index(int? status = null, string customerSearch = null)
        {
            try
            {
                // Get orders with included Customer and ApplicationUser
                var orders = await _orderRepo.GetAllIncludingAsync(
                    o => o.Customer,
                    o => o.Customer.ApplicationUser,
                    o => o.OrderProducts
                );

                // Apply status filter if provided
                if (status.HasValue)
                {
                    orders = orders.Where(o => (int)o.Status == status.Value).ToList();
                }

                // Apply customer search filter if provided
                if (!string.IsNullOrEmpty(customerSearch))
                {
                    orders = orders.Where(o =>
                        o.Id.ToString().Contains(customerSearch) ||
                        (o.Customer?.ApplicationUser != null && (
                            o.Customer.ApplicationUser.FirstName.Contains(customerSearch, StringComparison.OrdinalIgnoreCase) ||
                            o.Customer.ApplicationUser.Email.Contains(customerSearch, StringComparison.OrdinalIgnoreCase))
                        ))
                    .ToList();
                }

                var model = new AdminOrdersViewModel
                {
                    Ordersbb = orders.ToList(),
                    StatusFilter = status,
                    CustomerSearch = customerSearch
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders");
                TempData["ErrorMessage"] = "An error occurred while loading orders.";
                return RedirectToAction("Error", "Home");
            }
        }




        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var order = await _orderRepo.GetAllIncludingAsync(
                    o => o.Customer,
                    o => o.Customer.ApplicationUser,
                    o => o.OrderProducts

                );


                var selectedOrder = order.FirstOrDefault(o => o.Id == id);

                if (selectedOrder == null)
                {
                    TempData["ErrorMessage"] = "Order not found.";
                    return RedirectToAction(nameof(Index));
                }

                return View(selectedOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving order details {id}");
                TempData["ErrorMessage"] = "Error loading order details.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExportToCsv(int? status = null)
        {
            try
            {
                var orders = await _orderRepo.GetAllAsync();

                if (status.HasValue)
                {
                    orders = orders.Where(o => (int)o.Status == status.Value).ToList();
                }

                var csv = GenerateOrdersCsv(orders);
                var fileName = $"Orders_{DateTime.Now:yyyyMMddHHmmss}.csv";

                return File(Encoding.UTF8.GetBytes(csv), "text/csv", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting orders to CSV");
                TempData["ErrorMessage"] = "Error exporting orders.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool CanBeConfirmed(int status)
        {
            return status == (int)orderstateEnum.Pending;
        }

        private bool CanBeCancelled(int status)
        {
            return status == (int)orderstateEnum.Pending || status == (int)orderstateEnum.Confirmed;
        }

        private string GenerateOrdersCsv(IEnumerable<Order> orders)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Order ID,Customer ID,Date,Amount,Status,Item Count");

            foreach (var order in orders)
            {
                var statusName = GetStatusName((int)order.Status);
                sb.AppendLine($"\"{order.Id}\",\"{order.CustomerId}\",\"{order.OrderDate:yyyy-MM-dd}\",\"{order.TotalPrice}\",\"{statusName}\",\"{order.OrderProducts?.Count ?? 0}\"");
            }

            return sb.ToString();
        }

        private string GetStatusName(int status)
        {
            return Enum.GetName(typeof(orderstateEnum), status) ?? status.ToString();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                var order = await _orderRepo.GetByIdAsync(id);
                if (order == null)
                {
                    TempData["ErrorMessage"] = "Order not found.";
                    return RedirectToAction(nameof(Index));
                }

                if (!CanBeConfirmed((int)order.Status))
                {
                    TempData["ErrorMessage"] = $"Order cannot be confirmed in its current state ({GetStatusName((int)order.Status)}).";
                    return RedirectToAction(nameof(Index));
                }

                order.Status = (Domain.Enums.orderstateEnum)orderstateEnum.Confirmed;
                _orderRepo.Update(order);
                await _orderRepo.SaveChangesAsync(); // Add this line

                TempData["SuccessMessage"] = $"Order #{id} confirmed successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error confirming order {id}");
                TempData["ErrorMessage"] = $"Error confirming order #{id}.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var order = await _orderRepo.GetByIdAsync(id);
                if (order == null)
                {
                    TempData["ErrorMessage"] = "Order not found.";
                    return RedirectToAction(nameof(Index));
                }

                if (!CanBeCancelled((int)order.Status))
                {
                    TempData["ErrorMessage"] = $"Order cannot be cancelled in its current state ({GetStatusName((int)order.Status)}).";
                    return RedirectToAction(nameof(Index));
                }

                order.Status = (Domain.Enums.orderstateEnum)orderstateEnum.Cancelled;
                _orderRepo.Update(order);
                await _orderRepo.SaveChangesAsync(); // Add this line

                TempData["SuccessMessage"] = $"Order #{id} cancelled successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error cancelling order {id}");
                TempData["ErrorMessage"] = $"Error cancelling order #{id}.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}