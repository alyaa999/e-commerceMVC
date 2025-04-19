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
        Processing,       // Order is being prepared (inventory check, packaging)
        Shipped,          // Order dispatched with tracking infoالاورد خلص وجاهز انه يتشحن 

        // Delivery states
        OutForDelivery,   // Courier has the order
        Delivered,        // Order received by customer

        // Cancellation/return states
        Cancelled,        // Order cancelled before shipping
    }

}
