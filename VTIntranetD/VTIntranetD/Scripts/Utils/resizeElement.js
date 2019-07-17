
function populateForm(frm, data) {
    $.each(data, function (key, value) {
        $('[name=' + key + ']', frm).val(value);
    });
}



///  Populate form to Labels
function populateFormLabels(frm, data) {
    $.each(data, function (key, value) {
        $('#' + key).append(value);
    });
}