var nav = function (folder) {
    $.ajax({
        url: "/Main/Explorer",
        data: {
            folder: folder,
        },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#Explorer").html(data);
        }
    });
}

var download = function (filename, pwd) {
    var xhr = new XMLHttpRequest();
    xhr.open("GET", pwd + filename, true);
    var params = "filename=" + filename + "&pwd=" + pwd;
    xhr.send(params);
}

var show_upload_dialog = function () {
    var dialog = document.getElementById("UploadDialog");
    dialog.style.display = "block";
    dialog.style.position = "absolute";
}