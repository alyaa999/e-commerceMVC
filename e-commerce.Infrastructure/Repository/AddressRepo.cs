using AutoMapper;
using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Infrastructure.Repository
{
    public class AddressRepo : IAdressRepo
    {
        private readonly ECommerceDBContext context;
        public AddressRepo(ECommerceDBContext _context)
        {
            context = _context;
        }

        public void AddAddressAsync(Address addess,int cid)  
        {
            addess.CustomerId = cid;
            var custumorAddresses = context.Addresses.Where(x => x.CustomerId == cid).ToList();
            if(custumorAddresses.Count == 0)
            {
                addess.IsDefault = true;
            }

            for (int i = 0; i < custumorAddresses.Count; i++)
            {
                if (custumorAddresses[i].City == addess.City && custumorAddresses[i].Street == addess.Street && custumorAddresses[i].DeptNo == addess.DeptNo)
                {
                    throw new Exception("Address already exists");
                }
            }
            if(context.Addresses.Any(x => x.CustomerId == cid && x.IsDefault == true) && addess.IsDefault == true)
            {
                throw new Exception("You can not add more than one default address");
            }
            context.Addresses.Add(addess);
            context.SaveChanges();
        }

        public void DeleteAddressAsync(int id,int cid)
        {
            var res = context.Addresses.Find(id);
            if(res != null)
            {
                if(res.CustomerId == cid)
                {
                    context.Addresses.Remove(res);
                    context.SaveChanges(); 
                }
                else
                {
                    throw new Exception($"Address not found to this user {cid}");
                }
            }
            else
            {
                throw new Exception("Address not found");
            }
            context.SaveChanges();
        }

        public List<Address> GetAllAddressAsync(int userID)
        {
            return context.Addresses.Include(x => x.Customer).Where(ad=>ad.CustomerId==userID).ToList();
        }

        public void UpdateAddress(Address entity,int cid,int addID)
        {

            if(cid == entity.CustomerId)
            {
                var res = context.Addresses.Find(addID);
                if (res != null)
                {
                    res.City = entity.City;
                    res.Street = entity.Street;
                    res.DeptNo = entity.DeptNo;
                    res.IsDefault = entity.IsDefault;
                    var defualtAddress = context.Addresses.FirstOrDefault(x => x.IsDefault == true);
                    if (context.Addresses.Any(x => x.CustomerId == cid && x.IsDefault == true) && res.IsDefault == true && (defualtAddress.Id != res.Id))
                    {
                        throw new Exception("You can not add more than one default address");
                    }
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Address not found");
                }
                
            }
        }
        public Address GetAddressById(int id,int cID)
        {
            var res = context.Addresses.Find(id);
            if (res != null)
            {
                if (res.CustomerId == cID)
                {
                    return res;
                }
                else
                {
                    throw new Exception($"Address not found to this user {cID}");
                }
            }
            else
            {
                throw new Exception("Address not found");
            }
        }
        public bool isAddressConnectedToOrder(int AddressID)
        {
            return context.Orders.Any(o => o.ShippingAddressId == AddressID && o.Status != Domain.Enums.orderstateEnum.Cancelled);
        }


    }
}
