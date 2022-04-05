
function errorHandler(data) {

    var result = jQuery.parseJSON(data.responseText);
    if (result.ErrorCode === 520 && result.StatusCode === 405) {
        $.toast({
            text: 'External API unavailable. please try again later.',
            heading: 'Api Error (' + result.StatusCode + ')',
            hideAfter: 4000,
            position: 'top-center',
            bgColor: '#fd6767',
        });
        return;
    }
    if (result.ErrorCode === 520 && result.StatusCode === 404) {
        $.toast({
            text: 'The user not found!',
            heading: 'Api Error (' + result.StatusCode + ')',
            hideAfter: 4000,
            position: 'top-center',
            bgColor: 'lightblue',
            textColor: 'darkblue',
        });
        return;
    }
    if (result.ErrorCode === 520) {
        $.toast({
            text: 'External API unknown error. please try again later.',
            heading: 'Api Error (' + result.StatusCode + ')',
            hideAfter: 4000,
            position: 'top-center',
            bgColor: '#fd6767',
        });
        return;
    }
    $.toast({
        text: result.Message,
        heading: result.StatusCode,
        hideAfter: 4000,
        position: 'top-center',
    });

}

function blockPage() {
    $.blockUI({
        css: {
            border: "none",
            padding: "15px",
            backgroundColor: "#000",
            '-webkit-border-radius': "10px",
            '-moz-border-radius': "10px",
            opacity: .5,
            color: "#fff"
        },
        message: "<p>Please wait ... </p>"
    });
}


function unblockPage() { setTimeout($.unblockUI, 0); }
