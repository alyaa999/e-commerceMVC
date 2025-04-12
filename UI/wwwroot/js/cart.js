$(document).ready(function () {
    $('.add-to-cart').click(function (e) {
        e.preventDefault();
        var productId = $(this).data('product-id');
        var $button = $(this);

        // Add loading state
        $button.prop('disabled', true);
        $button.find('i').replaceWith('<i class="fas fa-spinner fa-spin"></i>');

        $.ajax({
            url: '/Cart/AddItemToCart',
            type: 'POST',
            data: { productId: productId },
            success: function (response) {
                Swal.fire({
                    icon: 'success',
                    title: 'Added to cart!',
                    showConfirmButton: false,
                    timer: 1500,
                    position: 'top-end',
                    toast: true
                });
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Could not add item to cart'
                });
            },
            complete: function () {
                $button.prop('disabled', false);
                $button.find('i').replaceWith('<i class="fi fi-rs-shopping-bag-add"></i>');
            }
        });
    });
});


