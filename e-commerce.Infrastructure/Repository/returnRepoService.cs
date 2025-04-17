using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Infrastructure.Repository
{
    
    public class returnRepoService 
    {
        private ECommerceDBContext context;

        public returnRepoService(ECommerceDBContext _context)
        {
            context = _context;
        }
        //public List<Return> getAllCustomerReturns(int custID)
        //{
        //   return ;
        //}
    }
}
