using System.Collections.Generic;
using System.Threading.Tasks;
using курсовая2511.Models; 

namespace курсовая2511.TechAccounting.Application.Dtos.Interfaces
{
    public interface IEquipmentTypeService
    {
        Task<IEnumerable<EquipmentType>> GetAllAsync();
        Task CreateAsync(EquipmentType entity);
    }
}
