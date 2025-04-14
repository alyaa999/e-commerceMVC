$(document).ready(function () {
    $('.placeOrderBtn').click(function (e) {
        e.preventDefault();

        const customerID = $(this).data('customerid');

        // Use the selected address ID
        if (!selectedAddressId) {
            alert("Please select a shipping address first.");
            return;
        }

        const orderData = {
            customerId: customerID,
            shippingAddressId: selectedAddressId
            // Add more order fields if needed
        };

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
    });
});
