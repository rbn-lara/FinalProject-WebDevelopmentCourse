
$(document).ready(function () {
    $('#validate-btn').click(function () {
        var firstFactor, secondFactor, total;
        firstFactor = parseInt($('#room-count').val()) + parseInt($('#bathroom-count').val()) + parseInt($('#half-bathroom-count').val()) + parseInt($('input[name="terrace-size-radio"]:checked').val()) + parseInt($('input[name="floor-type-radio"]:checked').val()) + parseInt($('input[name="furniture-type-radio"]:checked').val());
        secondFactor = ($('#outdoors-sink').val() ? 2 : 3) * $('input[name="construction-size-radio"]:checked').val();
        total = (firstFactor + secondFactor) * 20000;
        if (isNaN(total)) {
            $('#price').val('');
            $('#validation-message').text('Valores incorrectos');
        } else {
            $('#price').val(total);
            $('#validation-message').text('');
        }
    });
});
$(document).ready(function (){
    $(':submit').click(function () {
        var firstFactor, secondFactor, total;
        firstFactor = parseInt($('#room-count').val()) + parseInt($('#bathroom-count').val()) + parseInt($('#half-bathroom-count').val()) + parseInt($('input[name="terrace-size-radio"]:checked').val()) + parseInt($('input[name="floor-type-radio"]:checked').val()) + parseInt($('input[name="furniture-type-radio"]:checked').val());
        secondFactor = ($('#outdoors-sink').val() ? 2 : 3) * $('input[name="construction-size-radio"]:checked').val();
        total = (firstFactor + secondFactor) * 20000;
        $('#hidden-price').val(total);
    });
});
