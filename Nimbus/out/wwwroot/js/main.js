/*
 * Main.js
 * This file is a part of Nimbus. Copyright (c) 2017-present Jesse Jones.
 */

// Navigation
var nav = function (folder) {
    var xhr = new XMLHttpRequest();
    var formdata = new FormData();
    formdata.append("Folder", folder);
    xhr.open("POST", "/Main/Explorer");
    xhr.addEventListener("load", function () {
        document.getElementById("Explorer").innerHTML = xhr.responseText;
    });
    xhr.send(formdata);
}

var download = function () {
    var downloads = [];
    var items = document.getElementById("Explorer").children;

    for (var i = 1; i < items.length; i++) {
        var item = items[i];
        if (item.firstElementChild.checked && item.id.startsWith("file_")) {
            var progress_elem =
                info_panel.appendChild(document.createElement("div"));
            progress_elem.style.borderBottom = "3px solid";
            progress_elem.innerHTML = "<br/>Downloading item<br/>\"" +
                item.id.substr(5) + "\"<br/><br/>";
            window.open("/IO/Download?file=" + item.id.substr(5), "_blank");
        }
    }
}

var slice_and_upload_file = function (file) {
    var file_chunks = [];
    var chunk_size = 1 * 1024 * 1024;
    var file_stream_pos = 0;
    var end_pos = chunk_size;
    var size = file.size;

    while (file_stream_pos < size) {
        file_chunks.push(file.slice(file_stream_pos, end_pos));
        file_stream_pos = end_pos;
        end_pos = file_stream_pos + chunk_size;
    }

    var progress_elem = info_panel.appendChild(document.createElement("div"));
    progress_elem.style.borderBottom = "3px solid";

    var total_chunks = file_chunks.length;

    var send_chunk = function (chunk = 0) {
        var xhr = new XMLHttpRequest();
        var formdata = new FormData();
        var file_chunk = file_chunks[chunk];
        var chunk_name = file.name + ".part_" + (chunk + 1) + "." +
            total_chunks;
        formdata.append(chunk_name, file_chunk, chunk_name);

        xhr.open("POST", "/IO/Upload");
        xhr.addEventListener("load", function () {
            progress_elem.innerHTML = "<br/>Uploading item<br/>\"" + file.name +
                "\"<br/>" + (chunk + 1) + "MB /" + total_chunks + "MB<br/><br/>";
            if (chunk + 1 < total_chunks) send_chunk(chunk + 1);
            if (chunk + 1 == total_chunks) nav("");
        });
        xhr.send(formdata);
    }
    send_chunk();
}

// Upload files without submitting a form
var file_input = document.getElementById("FilesToUpload");
file_input.addEventListener('change', function () {
    var files = file_input.files;
    for (var i = 0; i < files.length; i++) {
        slice_and_upload_file(files[i]);
    }
});

// Create a new folder
var new_folder = function () {
    var folder_name = prompt("Enter a name for the new folder");
    if (folder_name.length) {
        var progress_elem = info_panel.appendChild(document.createElement("div"));
        progress_elem.style.borderBottom = "3px solid";
        var xhr = new XMLHttpRequest();
        var formdata = new FormData();

        xhr.open("POST", "/IO/NewFolder");
        formdata.append("FolderName", folder_name);
        xhr.addEventListener("load", function (result) {
            progress_elem.innerHTML = "<br/>Created new folder<br/>\"" +
            folder_name + "\"<br/><br/>";
            nav("");
        });
        xhr.send(formdata);
    }
}

var delete_files = function () {
    var confirmation = confirm("Are you absolutely sure you want to delete the selected items?\n\
Folders will be deleted recursively. THIS CANNOT BE UNDONE.");
    var items = document.getElementById("Explorer").children;
    
    if (!confirmation) return;

    for (var i = 1; i < items.length; i++) {
        if (items[i].firstElementChild.checked) {
            var id = items[i].id;
            var xhr = new XMLHttpRequest();
            var formdata = new FormData();
            formdata.append("DeletThis", id);
            xhr.open("POST", "/IO/Delete", false);
            xhr.addEventListener("load", function () {
                var progress_elem =
                    info_panel.appendChild(document.createElement("div"));
                progress_elem.style.borderBottom = "3px solid";
                progress_elem.innerHTML = "<br/>Deleted item<br/>\"" +
                    id.substring(id.indexOf("_") + 1) + "\"<br/><br/>";
            });
            xhr.send(formdata);
        }
    }
    nav("");
}

// This really should be global
var info_panel = document.getElementById("InfoPanelContent");

document.onload = nav("/");