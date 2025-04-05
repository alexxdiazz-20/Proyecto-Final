using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TallerAutomotriz.Application.DTOs;
using TallerAutomotriz.Application.Interfaces;
using TallerAutomotriz.Domain.Entities;
using TallerAutomotriz.Infrastructure.Interfaces;

namespace TallerAutomotriz.Application.Services
{
    public class ServiceOrderService : IServiceOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiceOrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceOrderDto>> GetAllAsync()
        {
            var serviceOrders = await _unitOfWork.ServiceOrderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ServiceOrderDto>>(serviceOrders);
        }

        public async Task<ServiceOrderDto> GetByIdAsync(int id)
        {
            var serviceOrder = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(id);
            return _mapper.Map<ServiceOrderDto>(serviceOrder);
        }

        public async Task<IEnumerable<ServiceOrderDto>> GetByVehicleIdAsync(int vehicleId)
        {
            var serviceOrders = await _unitOfWork.ServiceOrderRepository.GetByVehicleIdAsync(vehicleId);
            return _mapper.Map<IEnumerable<ServiceOrderDto>>(serviceOrders);
        }

        public async Task<IEnumerable<ServiceOrderDto>> GetByMechanicIdAsync(int mechanicId)
        {
            var serviceOrders = await _unitOfWork.ServiceOrderRepository.GetByMechanicIdAsync(mechanicId);
            return _mapper.Map<IEnumerable<ServiceOrderDto>>(serviceOrders);
        }

        public async Task<ServiceOrderDto> CreateAsync(CreateServiceOrderDto serviceOrderDto)
        {
            var serviceOrder = _mapper.Map<ServiceOrder>(serviceOrderDto);
            serviceOrder.CreatedAt = DateTime.UtcNow;
            serviceOrder.Status = ServiceOrderStatus.Pending;

            // Calcular el total
            decimal totalAmount = 0;
            var details = new List<ServiceOrderDetail>();

            foreach (var detailDto in serviceOrderDto.Details)
            {
                var service = await _unitOfWork.ServiceRepository.GetByIdAsync(detailDto.ServiceId);
                if (service == null)
                    continue;

                var detail = new ServiceOrderDetail
                {
                    ServiceId = detailDto.ServiceId,
                    Quantity = detailDto.Quantity,
                    UnitPrice = service.Price,
                    Subtotal = service.Price * detailDto.Quantity,
                    Notes = detailDto.Notes,
                    CreatedAt = DateTime.UtcNow
                };

                totalAmount += detail.Subtotal;
                details.Add(detail);
            }

            serviceOrder.TotalAmount = totalAmount;
            serviceOrder.Details = details;

            await _unitOfWork.ServiceOrderRepository.AddAsync(serviceOrder);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ServiceOrderDto>(serviceOrder);
        }

        public async Task<ServiceOrderDto> UpdateAsync(int id, UpdateServiceOrderDto serviceOrderDto)
        {
            var existingServiceOrder = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(id);

            if (existingServiceOrder == null)
                return null;

            _mapper.Map(serviceOrderDto, existingServiceOrder);
            existingServiceOrder.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.ServiceOrderRepository.Update(existingServiceOrder);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ServiceOrderDto>(existingServiceOrder);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var serviceOrder = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(id);

            if (serviceOrder == null)
                return false;

            _unitOfWork.ServiceOrderRepository.Remove(serviceOrder);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}