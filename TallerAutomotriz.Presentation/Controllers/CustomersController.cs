﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TallerAutomotriz.Application.DTOs;
using TallerAutomotriz.Application.Interfaces;

namespace TallerAutomotriz.Presentation.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllAsync();
            return View(customers);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerDto customerDto)
        {
            if (ModelState.IsValid)
            {
                await _customerService.CreateAsync(customerDto);
                return RedirectToAction(nameof(Index));
            }

            return View(customerDto);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateCustomerDto
            {
                Name = customer.Name,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                Address = customer.Address
            };

            return View(updateDto);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCustomerDto customerDto)
        {
            if (ModelState.IsValid)
            {
                var updatedCustomer = await _customerService.UpdateAsync(id, customerDto);

                if (updatedCustomer == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(customerDto);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _customerService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}