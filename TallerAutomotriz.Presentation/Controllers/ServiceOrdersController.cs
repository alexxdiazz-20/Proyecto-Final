﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using TallerAutomotriz.Application.DTOs;
using TallerAutomotriz.Application.Interfaces;

namespace TallerAutomotriz.Presentation.Controllers
{
    public class ServiceOrdersController : Controller
    {
        private readonly IServiceOrderService _serviceOrderService;
        private readonly IVehicleService _vehicleService;
        private readonly IMechanicService _mechanicService;
        private readonly IServiceService _serviceService;

        public ServiceOrdersController(
            IServiceOrderService serviceOrderService,
            IVehicleService vehicleService,
            IMechanicService mechanicService,
            IServiceService serviceService)
        {
            _serviceOrderService = serviceOrderService;
            _vehicleService = vehicleService;
            _mechanicService = mechanicService;
            _serviceService = serviceService;
        }

        // GET: ServiceOrders
        public async Task<IActionResult> Index()
        {
            var serviceOrders = await _serviceOrderService.GetAllAsync();
            return View(serviceOrders);
        }

        // GET: ServiceOrders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var serviceOrder = await _serviceOrderService.GetByIdAsync(id);

            if (serviceOrder == null)
            {
                return NotFound();
            }

            return View(serviceOrder);
        }

        // GET: ServiceOrders/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropDowns();
            return View();
        }

        // POST: ServiceOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateServiceOrderDto serviceOrderDto)
        {
            if (ModelState.IsValid)
            {
                await _serviceOrderService.CreateAsync(serviceOrderDto);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDowns();
            return View(serviceOrderDto);
        }

        // GET: ServiceOrders/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var serviceOrder = await _serviceOrderService.GetByIdAsync(id);

            if (serviceOrder == null)
            {
                return NotFound();
            }

            var updateDto = new UpdateServiceOrderDto
            {
                CompletionDate = serviceOrder.CompletionDate,
                Description = serviceOrder.Description,
                Status = serviceOrder.Status,
                VehicleId = serviceOrder.VehicleId,
                MechanicId = serviceOrder.MechanicId
            };

            await PopulateDropDowns(serviceOrder.VehicleId, serviceOrder.MechanicId);
            return View(updateDto);
        }

        // POST: ServiceOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateServiceOrderDto serviceOrderDto)
        {
            if (ModelState.IsValid)
            {
                var updatedServiceOrder = await _serviceOrderService.UpdateAsync(id, serviceOrderDto);

                if (updatedServiceOrder == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropDowns(serviceOrderDto.VehicleId, serviceOrderDto.MechanicId);
            return View(serviceOrderDto);
        }

        // GET: ServiceOrders/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var serviceOrder = await _serviceOrderService.GetByIdAsync(id);

            if (serviceOrder == null)
            {
                return NotFound();
            }

            return View(serviceOrder);
        }

        // POST: ServiceOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceOrderService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropDowns(int? selectedVehicleId = null, int? selectedMechanicId = null)
        {
            var vehicles = await _vehicleService.GetAllAsync();
            var mechanics = await _mechanicService.GetAllAsync();
            var services = await _serviceService.GetAllAsync();

            ViewBag.Vehicles = new SelectList(
                vehicles,
                "Id",
                "LicensePlate",
                selectedVehicleId);

            ViewBag.Mechanics = new SelectList(
                mechanics,
                "Id",
                "Name",
                selectedMechanicId);

            ViewBag.Services = new SelectList(
                services,
                "Id",
                "Name");
        }
    }
}