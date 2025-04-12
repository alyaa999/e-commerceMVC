$(document).on('click', '.btn-delete-cart', function (e) {
    e.preventDefault();

    var itemId = $(this).data('product-id');
    if (itemId === undefined) {
        console.error("itemId is undefined");
        return;
    }
    Swal.fire({
        title: 'Are you sure?',
        text: "This item will be deleted. This action cannot be undone!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Cart/DeleteItemFromCart',
                data: { PrdId : itemId },
                type: 'POST',
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Deleted!',
                            text: 'Your item has been deleted.',
                            timer: 1500,
                            showConfirmButton: false,
                            position: 'top-end',
                            toast: true
                        }).then(() => {
                            location.reload();
                        });
                    } else {
                        Swal.fire('Error', response.message || 'Failed to delete item', 'error');
                    }
                },
                error: function () {
                    Swal.fire('Error', 'Could not delete the item', 'error');
                }
            });
        }
    });
});