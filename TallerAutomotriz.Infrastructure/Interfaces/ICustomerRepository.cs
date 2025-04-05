using TallerAutomotriz.Domain.Entities;
using System.Threading.Tasks;

namespace TallerAutomotriz.Infrastructure.Interfaces
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        // Métodos específicos para Customer si los hay
    }
}