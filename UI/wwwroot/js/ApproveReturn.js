
$(document).on('click', '.approve_return', function () {
    console.log("Approve button clicked");
    console.log(this)
    var orderId = $(this).data('orderid');
    var returnId = $(this).data('rid');
        console.log(orderId)
        Swal.fire({
            title: 'Are you sure?',
            text: "You are about to return this order request a refund.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, Return it!',
            cancelButtonText: 'No, cancel it'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/AdminReturns/Approve',
                    type: 'POST',
                    data: { id: returnId },
                    success: function (response) {
                        Swal.fire({
                            title: response.success ? 'Success!' : 'Error!',
                            text: response.message,
                            icon: response.success ? 'success' : 'error'
                        }).then(() => {
                            if (response.success) {
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
                                                window.location.href = '/AdminReturns/Index';

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

//
