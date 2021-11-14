using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretAgency.Models;
using SecretAgency.Services;

namespace SecretAgency.Repositories.Interfaces
{
    public interface IMissionRepository
    {
        Task<IResult<bool>> Delete(Guid id);
        Task<IResult<IEnumerable<Mission>>> GetAll();
        Task<IResult<Mission>> Get(Guid id);
        Task<IResult<Mission>> Create(Mission mission);
        Task<IResult<Mission>> Update(Guid id, Mission mission);
        Task<IResult<bool>> Exists(Guid id);
    }
}