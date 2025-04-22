$(document).ready(function () {
    $('.add-to-wishlist').click(function (e) {
        e.preventDefault();
        var productId = $(this).data('product-id');
        var $button = $(this);

        // Add loading state
        $button.prop('disabled', true);
        $button.find('i').replaceWith('<i class="fas fa-spinner fa-spin"></i>');

        $.ajax({
            url: '/wishlists/AddToWishlist',
            type: 'POST',
            data: { productId: productId },
            success: function (response) {
                if (response.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Added to Wishlist!',
                        showConfirmButton: false,
                        timer: 1500,
                        position: 'top-end',
                        toast: true
                    });
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Faild to add!',
                        showConfirmButton: false,
                        timer: 3000,
                        position: 'top-end',
                        toast: true,
                        text: response.message
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Could not add item to Wishlist'
                });
            },
            complete: function () {
                $button.prop('disabled', false);
                $button.find('i').replaceWith('<i class="fi fi-rs-heart"></i>');
            }
        });
    });
});

$(document).on('click', '.btn-delete', function (e) {
    e.preventDefault();

    var itemId = $(this).data('id');
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
                url: '/wishlists/delete',
                data: { id: itemId },
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

//$(document).on('click', '.btn-delete', function (e) {
//    e.preventDefault();
//    var itemId = $(this).data('id');
//    var $row = $(this).closest('tr'); // optional: for removing row after delete

//    if (!itemId) {
//        console.error("itemId is undefined");
//        return;
//    }

//    Swal.fire({
//        title: 'Are you sure?',
//        text: "This item will be deleted. This action cannot be undone!",
//        icon: 'warning',
//        showCancelButton: true,
//        confirmButtonColor: '#d33',
//        cancelButtonColor: '#3085d6',
//        confirmButtonText: 'Yes, delete it!'
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: '/wishlists/delete',
//                type: 'POST',
//                data: { id: itemId },
//                success: function () {
//                    Swal.fire({
//                        icon: 'success',
//                        title: 'Deleted!',
//                        showConfirmButton: false,
//                        timer: 1500,
//                        position: 'top-end',
//                        toast: true
//                    });

//                    // Optionally remove the deleted row from the DOM
//                    $row.remove();
//                },
//                error: function () {
//                    Swal.fire({
//                        icon: 'error',
//                        title: 'Error',
//                        text: 'Could not delete the item.'
//                    });
//                }
//            });
//        }
//    });
//});
//});