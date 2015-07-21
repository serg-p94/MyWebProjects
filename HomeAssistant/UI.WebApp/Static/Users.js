function setUserPermission(userId, permissionId, value, url) {
    var options = {
        url: url,
        method: "POST",
        data: {
            userId: userId,
            permissionId: permissionId,
            value: value
        }
    };
    $.ajax(options);
}

function ValidateEmail(email) {
    var re = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    var res = email != '' && re.test(email);
    return email != '' && re.test(email);
}

function EmailOnChange(emailDiv) {
    var email = $(emailDiv).find('input').val();

    if (email == "") {
        markInput(emailDiv);
    } else {
        markInput(emailDiv, ValidateEmail(email));
    }
}

function checkLoginIsEmpty(loginDiv, url) {
    var input = $(loginDiv).find('input');
    var login = input.val();
    if (login == "") {
        markInput(loginDiv);
        return;
    }

    var options = {
        url: url,
        data: { login: login },
        method: 'POST',
        success: function (data) {
            markInput(loginDiv, !data.result);
        }
    };
    $.ajax(options);
}

function markInput(inputDiv, valid) {
    $(inputDiv).removeClass('has-error')
        .removeClass('has-success')
        .children('span').remove();

    if (typeof valid != "undefined") {
        if (valid) {
            $(inputDiv).addClass('has-success')
                .append('<span class="glyphicon glyphicon-ok form-control-feedback"></span>');
        } else {
            $(inputDiv).addClass('has-error')
                .append('<span class="glyphicon glyphicon-remove form-control-feedback"></span>');
        }
    }

    if ($('.validate.has-error').length == 0) {
        $('#btn-submit').removeAttr('disabled');
    } else {
        $('#btn-submit').attr('disabled', 'disabled');
    }
}