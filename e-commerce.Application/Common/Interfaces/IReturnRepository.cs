using e_commerce.Infrastructure.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Application.Common.Interfaces
{
    public interface IReturnRepository
    {
        List<Return> getAllCustomerReturns(int custID);
        void AddReturnRequest(List<Return> returns);
        List<Order> getOrdersCanReturn(int userID);

    }
}
