    $(document).on('click', '.cancel-order-btn', function () {
        var orderId = $(this).data('order-id');
        console.log(orderId)
        Swal.fire({
            title: 'Are you sure?',
            text: "You are about to cancel the order and request a refund.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, cancel it!',
            cancelButtonText: 'No, keep it'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/order/Delete',
                    type: 'POST',
                    data: { id: orderId },
                    success: function (response) {
                        Swal.fire({
                            title: response.success ? 'Success!' : 'Error!',
                            text: response.message,
                            icon: response.success ? 'success' : 'error'
                        }).then(() => {
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
    });

