function ValidateEmail(email)
{
    var re = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
    var res = email != '' && re.test(email);
    return email != '' && re.test(email);
}

function EmailOnChange() {
    var str = document.getElementById('userEmail').value;
    $('#userEmailDiv')
        .removeClass('has-error')
        .removeClass('has-success')
        .children('span').remove();
    if (str == "") {
        return;
    }
    if (!ValidateEmail(str)) {
        $('#userEmailDiv')
            .addClass('has-error')
            .append('<span class="glyphicon glyphicon-remove form-control-feedback"></span>');
    } else {
        $('#userEmailDiv')
            .addClass('has-success')
            .append('<span class="glyphicon glyphicon-ok form-control-feedback"></span>');
    }
}

function setMark(movieId, mark) {
    $.ajax('/Movie/SetMarkAjax', {
        type: 'POST',
        data: { movieId: movieId, mark: mark },
        success: function (data) {
            $('#rating').html(data.Rating);
        }
    });
}

function refreshActors() {
    $('#actorsList').html('');
    $('#inputActors').val('');

    $.ajax('/Movie/GetAllActors', {
        type: 'POST',
        data: {},
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var actor = data[i];
                $('#actorsList').append(
                    '<li class="ui-state-default text-center" style="color: green; cursor: pointer"><label style="cursor: pointer">'
                    + actor.Surname + ", " + actor.Name + '</label></li>');
            }
        }
    });
}