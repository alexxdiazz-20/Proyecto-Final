﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TallerAutomotriz.Application.DTOs;
using TallerAutomotriz.Application.Interfaces;
using TallerAutomotriz.Domain.Entities;
using TallerAutomotriz.Infrastructure.Interfaces;

namespace TallerAutomotriz.Application.Services
{
    public class MechanicService : IMechanicService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MechanicService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MechanicDto>> GetAllAsync()
        {
            var mechanics = await _unitOfWork.MechanicRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MechanicDto>>(mechanics);
        }

        public async Task<MechanicDto> GetByIdAsync(int id)
        {
            var mechanic = await _unitOfWork.MechanicRepository.GetByIdAsync(id);
            return _mapper.Map<MechanicDto>(mechanic);
        }

        public async Task<MechanicDto> CreateAsync(CreateMechanicDto mechanicDto)
        {
            var mechanic = _mapper.Map<Mechanic>(mechanicDto);
            mechanic.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.MechanicRepository.AddAsync(mechanic);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MechanicDto>(mechanic);
        }

        public async Task<MechanicDto> UpdateAsync(int id, UpdateMechanicDto mechanicDto)
        {
            var existingMechanic = await _unitOfWork.MechanicRepository.GetByIdAsync(id);

            if (existingMechanic == null)
                return null;

            _mapper.Map(mechanicDto, existingMechanic);
            existingMechanic.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.MechanicRepository.Update(existingMechanic);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<MechanicDto>(existingMechanic);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var mechanic = await _unitOfWork.MechanicRepository.GetByIdAsync(id);

            if (mechanic == null)
                return false;

            _unitOfWork.MechanicRepository.Remove(mechanic);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}