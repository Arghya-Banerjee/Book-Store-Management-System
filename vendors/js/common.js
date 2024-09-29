$(document).ajaxStart(function () {
    $.blockUI({
        message: '<img src="../img/loader-large.gif"/><br/>Processing your request.', baseZ: 999999999, css: {
            border: '5px solid #3c8dbc',
            borderRadius: '5px',
            padding: '20px 0'

        },
    })

}).ajaxStop($.unblockUI);

function validate_numeric(e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        e.preventDefault();
    }
}

function emailvalidate(email) {
    //var re = /[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}/igm;
    //if (re.test(email)) {
    //    return true
    //}
    //else {
    //    return false;
    //}
    re = /^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;
    if (email.trim() != '' && !email.trim().match(re)) {
        isValid = false;
    }
    else {
        isValid = true;
    }
    return isValid;
}
function minmax(value, min, max)  // if user input value is > max value, then input value will set to max value
{
    if (parseInt(value) < min || isNaN(parseInt(value)))
        return min;
    else if (parseInt(value) > max)
        return max;
    else return value;
}
var alertType = {
    success: "success",
    warning: "warning",
    error: "error",
    info: "info",
    question: "question"
};

function DeviceAlert(message, type, redirectpage) {
    redirectpage = redirectpage || null;
    if (redirectpage == null) {
        swal("", message, type, {
            button: "Ok",
            closeOnClickOutside: false
        });
    } else {
        swal("", message, type, {
            button: "Ok",
            closeOnClickOutside: false
        }).then(function (x) {
            window.location.href = redirectpage;
        });
    }
}

function UpScroll() {
    $('html, body').animate({
        scrollTop: (0)
    }, 500);
}



