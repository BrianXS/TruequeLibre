using System.Collections.Generic;
using API.Entities;

namespace API.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        Address GetAddress(int id);
        List<Address> GetUserAddresses(int id);
        Address GetAddressWithCity(int id);
        void SaveAddress(Address address);
        void EditAddress(Address address);
        void DeleteAddress(int id);

        bool Exists(int id);
    }
}