using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretAgency.Models;

namespace SecretAgency.Repositories.Interfaces
{
    public interface IMissionRepository
    {
        Task<bool> Delete(Guid id);
        Task<IEnumerable<Mission>> GetAll();
        Task<Mission> Get(Guid id);
        Task<Mission> Create(Mission mission);
        Task<Mission> Update(Guid id, Mission mission);
        Task<bool> Exists(Guid id);
    }
}