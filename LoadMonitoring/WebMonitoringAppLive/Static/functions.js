function Clear(dest) {
    dest.children("div").remove();
}

function AddCol(dst, h, w) {
    var col = $(document.createElement('div'));
    col.addClass('paint-col');
    col.addClass('text-center');
    col.css('width', '0');
    col.css('height', h);
    col.css('margin-top', painter.maxHeight - h - 2);
    col.animate({ "width": w + "%" });
    $(dst).append(col);
    return col;
}

function RemoveCol(dst) {
    var col = $(dst).find('.paint-col').first();
    col.animate({ width: 0 }, {
        complete: function () {
            col.remove();
        }
    });
}