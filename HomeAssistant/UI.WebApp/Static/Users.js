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