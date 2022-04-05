
function errorHandler(data) {

    var result = jQuery.parseJSON(data.responseText);
    if (result.ErrorCode === 520) {
        $.toast({
            text: 'External API unavailable. please try again later.',//result.Message,
            heading: 'Api unavailable ' +  result.StatusCode,
            showHideTransition: 'fade',
            allowToastClose: true,
            loader: false,
            hideAfter: 3000,
            stack: 6,
            position: 'top-center',
            bgColor: '#fd6767',
            textColor: '#eee',
            textAlign: 'left',
        });
    }
    else {
        $.toast({
            text: result.Message,
            heading: result.StatusCode,
            showHideTransition: 'fade',
            allowToastClose: true,
            loader: false,
            hideAfter: 3000,
            stack: 6,
            position: 'top-center',
            bgColor: '#444',
            textColor: '#eee',
            textAlign: 'left',
        });
    }

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
