var nav = function (folder) {
    $.ajax({
        url: "/Main/Explorer",
        data: {
            folder: folder,
        },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $("#Explorer").html(data);
        },
    });
}

var download = function (file) {
    var xhr = new XMLHttpRequest();
    xhr.open("GET", file, true);
    var params = "filename=" + file;
    xhr.send(params);
}

var show_upload_dialog = function () {
    UploadDialog.style.display = "block";
    UploadDialog.style.position = "absolute";
}

var upload_files = function () {
    var files = document.getElementById("FilesToUpload").files;
    var formdata = new FormData();
    for (var i = 0; i < files.length; i++) {
        formdata.append("file" + i, files[i]);
    }
    $.ajax({
        type: "POST",
        url: "/Main/Explorer",
        contentType: false,
        processData: false,
        data: formdata,
        success: function (result) {
            console.log(result);
        },
    });
    nav("");
}

var UploadDialog = document.getElementById("UploadDialog");
