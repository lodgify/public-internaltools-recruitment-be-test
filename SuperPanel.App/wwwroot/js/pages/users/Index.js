﻿$(document).ready(function () {
    $('#table-user').DataTable();
    $('[data-toggle="tooltip"]').tooltip();
});

function selectAll(e, elem) {
    var table = $(e.target).closest('table');
    $('td input:checkbox', table).prop('checked', elem.checked);
}

function sendRequest(id) {
    $.ajax({
        type: "Post",
        url: "/Users/GDPR",
        data: { 'id': id },
        beforeSend: blockPage,
        success: function (data) {
            $.toast({
                text: data.firstName + ' ' + data.lastName + '<br />Email Address: ' + data.email,
                heading: 'Success',
                showHideTransition: 'fade',
                allowToastClose: true,
                loader: true,
                hideAfter: 7000,
                position: 'top-center',
                bgColor: 'green',
                textColor: '#eee',
            });
        },
        error: (xhr) => errorHandler(xhr),
        complete: unblockPage
    });
}
function processAll() {
    $('#table-user td input:checked').each(function (i, elem) {
        var tr = $(elem).closest('tr');
        var id = tr.find('td:eq(1)').text();
        var lastTd = tr.find('td:last-child');
        $.ajax({
            type: "Post",
            url: "/Users/GDPR",
            data: { 'id': id },
            beforeSend: () => lastTd.html('<i class="fa fa-spinner fa-spin-pulse fa-spin-reverse blue-color"></i>'),
            success: (data) => lastTd.html('<i class="fa fa-check green-color" data-toggle="tooltip" title="Success"></i>'),
            error: () => lastTd.html('<i class="fa fa-times red-color" data-toggle="tooltip" title="Error"></i>')
        });
    });
}