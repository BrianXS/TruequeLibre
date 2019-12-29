using System.Collections.Generic;
using API.Entities;

namespace API.Repositories.Interfaces
{
    public interface IDetailRepository
    {
        void RemoveRange(List<Detail> details);
        void AddRange(List<Detail> details);
    }
}