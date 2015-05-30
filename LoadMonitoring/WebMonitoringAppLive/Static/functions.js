function Clear(dest) {
    var n = $(dest).children("div").length;
    RemoveCol(dest, n);
}

function AddCol(dst, h, w) {
    var col = $(document.createElement('div'));
    var areaHeight = 300;
    col.addClass('paint-col');
    col.addClass('text-center');
    col.css('width', '0');
    col.css('height', h);
    col.css('margin-top', areaHeight - h - 2);
    col.animate({ "width": w + "%" });
    $(dst).append(col);
    return col;
}

function RemoveCol(dst, n) {
    var cols = $(dst).find('.paint-col').slice(0, n);
    cols.animate({ width: 0 }, function () {
        cols.remove();
    });
}