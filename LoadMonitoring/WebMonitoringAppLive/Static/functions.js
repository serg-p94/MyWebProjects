function Clear(dest) {
    dest.children("div").remove();
}

function AddCol(dst, h) {
    var col = $(document.createElement('div'));
    var areaHeight = 300;
    col.addClass('paint-col');
    col.css('width', '0');
    col.css('height', h);
    col.css('margin-top', areaHeight - h - 2);
    col.animate({ width: 99 });
    $(dst).append(col);
}

function RemoveCol(dst) {
    var col = $(dst).find('.paint-col').first();
    col.animate({ width: 0 }, function () {
        col.remove();
    });
}