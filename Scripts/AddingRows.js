var rowCount = 1;

$('#add').click(function () {
    rowCount++;
    $('#orders').append('<tr id="row' + rowCount + '"><td><input class="form-control product_price" type="number" data-type="product_price" id="product_price_' + rowCount + '" name="product_price[]" for="' + rowCount + '"/></td><input class="form-control" type="hidden" data-type="product_id" id="product_id_' + rowCount + '" name="product_id[]" for="' + rowCount + '"/><td><input class="form-control quantity" type="number" class="quantity" id="quantity_' + rowCount + '" name="quantity[]" for="' + rowCount + '"/> </td><td><input class="form-control total_cost" type="text" id="total_cost_' + rowCount + '" name="total_cost[]"  for="' + rowCount + '" readonly/> </td><td><button type="button" name="remove" id="' + rowCount + '" class="btn btn-danger btn_remove cicle">-</button></td></tr>');
});

// Add a generic event listener for any change on quantity or price classed inputs
$("#orders").on('input', 'input.quantity,input.product_price', function () {
    getTotalCost($(this).attr("for"));
});

$(document).on('click', '.btn_remove', function () {
    var button_id = $(this).attr('id');
    $('#row' + button_id + '').remove();
});

// Using a new index rather than your global variable i
function getTotalCost(ind) {
    var qty = $('#quantity_' + ind).val();
    var price = $('#product_price_' + ind).val();
    var totNumber = (qty * price);
    var tot = totNumber.toFixed(2);
    $('#total_cost_' + ind).val(tot);
    calculateSubTotal();
}

function calculateSubTotal() {
    var subtotal = 0;
    $('.total_cost').each(function () {
        subtotal += parseFloat($(this).val());
    });
    $('#subtotal').val(subtotal);
}