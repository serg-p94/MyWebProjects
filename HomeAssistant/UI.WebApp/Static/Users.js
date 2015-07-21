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

function EmailOnChange() {
    var str = this.value;
    var parentDiv = $(this).parent();
    $(parentDiv)
        .removeClass('has-error')
        .removeClass('has-success')
        .children('span').remove();
    if (str == "") {
        return;
    }
    if (!ValidateEmail(str)) {
        parentDiv.addClass('has-error')
            .append('<span class="glyphicon glyphicon-remove form-control-feedback"></span>');
        $('#btn-submit').attr('disabled', 'disabled');
    } else {
        parentDiv.addClass('has-success')
            .append('<span class="glyphicon glyphicon-ok form-control-feedback"></span>');
        $('#btn-submit').removeAttr('disabled');
    }
}

function checkLoginIsEmpty(loginDiv, url) {
    var input = $(loginDiv).find('input');
    var login = input.val();

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

    if (valid) {
        $(inputDiv).addClass('has-success')
            .append('<span class="glyphicon glyphicon-ok form-control-feedback"></span>');
        $('#btn-submit').removeAttr('disabled');
    } else {
        $(inputDiv).addClass('has-error')
            .append('<span class="glyphicon glyphicon-remove form-control-feedback"></span>');
        $('#btn-submit').attr('disabled', 'disabled');
    }
}

$(function() {
    $('input.email').change(EmailOnChange);
    //$('.login-div').change(checkLoginIsEmpty);
})