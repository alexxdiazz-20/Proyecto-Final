using TallerAutomotriz.Domain.Entities;
using TallerAutomotriz.Infrastructure.Data;
using TallerAutomotriz.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TallerAutomotriz.Infrastructure.Repositories
{
    public class ServiceOrderRepository : BaseRepository<ServiceOrder>, IServiceOrderRepository
    {
        public ServiceOrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<ServiceOrder> GetByIdAsync(int id)
        {
            return await _dbSet
                .Where(so => so.Id == id)
                .Include(so => so.Vehicle)
                .Include(so => so.Mechanic)
                .Include(so => so.Details)
                    .ThenInclude(d => d.Service)
                .FirstOrDefaultAsync();
        }

        public override async Task<IEnumerable<ServiceOrder>> GetAllAsync()
        {
            return await _dbSet
                .Include(so => so.Vehicle)
                .Include(so => so.Mechanic)
                .Include(so => so.Details)
                    .ThenInclude(d => d.Service)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceOrder>> GetByVehicleIdAsync(int vehicleId)
        {
            return await _dbSet
                .Where(so => so.VehicleId == vehicleId)
                .Include(so => so.Vehicle)
                .Include(so => so.Mechanic)
                .Include(so => so.Details)
                    .ThenInclude(d => d.Service)
                .ToListAsync();
        }

        public async Task<IEnumerable<ServiceOrder>> GetByMechanicIdAsync(int mechanicId)
        {
            return await _dbSet
                .Where(so => so.MechanicId == mechanicId)
                .Include(so => so.Vehicle)
                .Include(so => so.Mechanic)
                .Include(so => so.Details)
                    .ThenInclude(d => d.Service)
                .ToListAsync();
        }
    }
}