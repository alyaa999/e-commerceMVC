$(document).on('click', '.cancel-order-btn', function () {
    var orderId = $(this).data('order-id');
    var msg = $(this).data('message');
    console.log(orderId)
    if (msg.includes("page")) {
        Swal.fire({
            title: 'Are you sure?',
            text: msg,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, Go to it!',
            cancelButtonText: 'No, cancel process'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = '/returns/create?orderId=' + orderId;
            }
        });
    }
    else if (msg.includes("request")) {
            Swal.fire({
                title: 'Attention?',
                text: msg,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'OK!',
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = '/_Orders/getAllCustOrder';
                }
            });
        }
        else if (msg.includes("cancelled")) {
            Swal.fire({
                title: 'Aleady Cancelled!',
                text: msg,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Ok',
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: '/_Orders/getAllCustOrder',
                        type: 'POST'
                    })
                }
            });
        }
        else { 
        Swal.fire({
            title: 'Are you sure?',
            text: msg,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, cancel it!',
            cancelButtonText: 'No, keep it'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/_Orders/Delete',
                    type: 'POST',
                    data: { id: orderId },
                    success: function (response) {
                        Swal.fire({
                            title: response.success ? 'Success!' : 'Error!',
                            text: response.message,
                            icon: response.success ? 'success' : 'error'
                        }).then(() => {
                            $("#ordersTable").load(location.href + " #ordersTable");
                            console.log(response.message)
                            if (response.success) {
                                if(response.message.includes("Refund")) {
                                    $.ajax({
                                        url: '/Payment/Refund',
                                        type: 'POST',
                                        data: { orderId: orderId },
                                        success: function (refundResponse) {
                                            Swal.fire({
                                                title: refundResponse.success ? 'Refund Success!' : 'Refund Failed!',
                                                text: refundResponse.message,
                                                icon: refundResponse.success ? 'success' : 'error'
                                            }).then(() => {
                                                // Reload the orders table after refund is processed
                                                $("#ordersTable").load(location.href + " #ordersTable");
                                            });
                                        }
                                    });
                                }
                            }
                        });
                    },
                    error: function () {
                        Swal.fire('Error!', 'Something went wrong with the request.', 'error');
                    }
                });
            }
        });
}
    });
