$(document).ready(function () {
    // Handle Update Cart button click
    $('#update-cart-btn').click(function (e) {
        e.preventDefault();
        const updates = [];

        $('tbody tr.cart-item').each(function () {
            const productId = $(this).data('product-id');
            const newQuantity = parseInt($(this).find('.quantity-input').val());
            const originalQuantity = parseInt($(this).find('.quantity-input').data('original'));

            if (newQuantity !== originalQuantity) {
                updates.push({
                    ProductId: productId,
                    Quantity: newQuantity
                });
            }
        });
        console.log("Final updates array:", updates); // Should show all items

        if (updates.length > 0) {
            $.ajax({
                url: '/Cart/UpdateCart',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(updates),
                success: function (response) {
                    location.reload(); // Refresh to show updated cart
                },
                error: function () {
                    alert('Error updating cart');
                }
            });
        } else {
            alert('No changes to update');
        }
    });
});