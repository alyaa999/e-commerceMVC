using e_commerce.Infrastructure.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Application.Common.Interfaces
{
    public interface IAdressRepo
    {
        void AddAddressAsync(Address address,int cid);
        void DeleteAddressAsync(int id,int cid);
        List<Address> GetAllAddressAsync(int userID);
        void UpdateAddress(Address address,int cid,int addID);
        public Address GetAddressById(int id, int cID);
        public bool isAddressConnectedToOrder(int AddressID);
    }
}
