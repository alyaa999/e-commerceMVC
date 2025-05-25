using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Infrastructure.Repository
{
    public class custRepo : ICustRepo
    {
        private ECommerceDBContext context;

        public custRepo(ECommerceDBContext _context)
        {
            context = _context;
        }
        public Customer? getcustomerid(string claimID)
        {
            return context.Customers.FirstOrDefault(c=>c.ApplicationUserId==claimID);
        }
    }
}
