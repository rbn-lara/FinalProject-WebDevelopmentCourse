
$(function () {
    $('#is-living-kitchen').change(function () {

        if ($(this).prop('checked')) {
            $(this).val('true');
        } else {
            $(this).val('false');
        }

    });

    $('#outdoors-sink').change(function () {

        if ($(this).prop('checked')) {
            $(this).val('true');
        } else {
            $(this).val('false');
        }

    });
});

