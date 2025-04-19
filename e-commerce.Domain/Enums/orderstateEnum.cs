using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Domain.Enums
{
    public enum orderstateEnum
    {

        Pending,
        Confirmed,
        Shipped,
        Delivered,
        Cancelled,
        Returned,
        PaymentPending,   //cash method
        Paid,            // Payment confirmed

        // Processing states
        Processing,       // Order is being prepared (inventory check, packaging)
              // Order dispatched with tracking infoالاورد خلص وجاهز انه يتشحن 

        // Delivery states
        OutForDelivery,   // Courier has the order
               // Order received by customer

        // Cancellation/return states
        ReturnRequested,  // Customer initiated a return
        Refunded,          // Money returned to customer
     


    }
}
