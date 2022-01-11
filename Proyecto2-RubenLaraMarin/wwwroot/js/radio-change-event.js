$(document).ready(function () {
    $('input[type=radio]').change(function () {
        if ($('#opcion1').is(':checked')) {
            $('#formato').text("0#-####-####");
        }
        else if ($('#opcion2').is(':checked')) {
            $('#formato').text("#-###-######")
        }
    });
});
