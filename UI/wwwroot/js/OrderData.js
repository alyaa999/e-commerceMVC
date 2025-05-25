$(document).ready(function () {
    $('.placeOrderBtn').click(function (e) {
        const customerID = $(this).data('customerid');
        e.preventDefault();

        // Use the selected address ID
        if (!selectedAddressId) {
            alert("Please select a shipping address first.");
            return;
        }

        const orderData = {
            customerId: customerID,
            shippingID: selectedAddressId
            // Add more order fields if needed
        };
        var paymentMethod = document.querySelector('input[name="radio"]:checked').id;
        if (paymentMethod == 'l2')
        {
            $.ajax({
                url: '/Payment/Checkout',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(orderData),
                success: function (url) {
                    window.location.href = url;
                },
                error: function () {
                    alert('Error placing order');
                }
            });

        }
        else if (paymentMethod == 'l3')
        {
            $.ajax({
                url: '/_Orders/Create',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(orderData),
                success: function (response) {
                    if (response.success) {
                        window.location.href = response.redirectUrl;
                    } 
                },
                error: function () {
                    alert('Error placing order from cash method');
                }
            });
        }
        console.log(orderData);
       
    });
});
