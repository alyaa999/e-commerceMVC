using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Domain.Enums
{
    public enum orderstateEnum
    {

        // Processing states
        Pending,
        Confirmed,
        Processing,
        Delivered,// Order is being prepared (inventory check, packaging)
              // Order dispatched with tracking infoالاورد خلص وجاهز انه يتشحن 

        // Delivery states
        OutForDelivery,   // Courier has the order
               // Order received by customer

        // Cancellation/return states
        Cancelled,        // Order cancelled before shipping
    }

}
