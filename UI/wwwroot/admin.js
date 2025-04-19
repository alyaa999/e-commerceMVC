// Initialize DataTables for all tables
$(document).ready(function () {
    $('table').DataTable({
        responsive: true,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });

    // Seller filter for products page
    $('#sellerFilter').change(function () {
        const sellerId = $(this).val();
        window.location.href = `/Admin/Products?sellerId=${sellerId}`;
    });

    // Search functionality
    $('#searchButton').click(function () {
        const searchString = $('#searchInput').val();
        // Implement search logic or form submission
    });
});