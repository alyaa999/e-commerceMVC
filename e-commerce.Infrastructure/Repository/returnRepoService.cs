using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Infrastructure.Repository
{
    
    public class returnRepoService :IReturnRepository
    {
        private ECommerceDBContext context;

        public returnRepoService(ECommerceDBContext _context)
        {
            context = _context;
        }

        public void AddReturnRequest(List<Return> returns)
        {
            if (returns != null) {
                for (int i = 0; i < returns.Count; i++) { 
                 context.Returns.Add(returns[i]);
                }
            }
        }

        public List<Return> getAllCustomerReturns(int custID)
        {
            return context.Returns.Include(r=>r.Order).Include(r=>r.Product).Where(re=>re.custId==custID).ToList();
        }
        
    }
}
