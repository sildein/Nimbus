/*
 * Main.js
 * This file is a part of Nimbus. Copyright (c) 2017-present Jesse Jones.
 */

// Navigation
var nav = function (folder) {
    var xhr = new XMLHttpRequest();
    xhr.open("GET", "/Main/Explorer?folder=" + folder, true);
    xhr.addEventListener("load", function () {
        document.getElementById("Explorer").innerHTML = xhr.responseText;
    });
    xhr.send();
}

var download = function () {
    var downloads = [];
    var items = document.getElementById("Explorer").children;

    for (var i = 1; i < items.length; i++) {
        var item = items[i];
        if (item.firstElementChild.checked && item.id.startsWith("file_")) {
            var progress_elem = info_panel.appendChild(document.createElement("div"));
            progress_elem.style.borderBottom = "3px solid";
            progress_elem.innerHTML = "<br/>Downloading item<br/>\"" + item.id.substr(5) + "\"<br/><br/>";
            window.open("/IO/Download?file=" + item.id.substr(5), "_blank");
        }
    }
}

// We can't send giant files in one request (or buffer them on modest servers,
// for that matter) so uploads are split into 1MB chunks
var slice_and_upload_file = function (file) {
    var file_chunks = [];
    var chunk_size = 1 * 1024 * 1024;
    var file_stream_pos = 0;
    var end_pos = chunk_size;
    var size = file.size;
    var progress_elem = info_panel.appendChild(document.createElement("div"));
    progress_elem.style.borderBottom = "3px solid";

    while (file_stream_pos < size) {
        file_chunks.push(file.slice(file_stream_pos, end_pos));
        file_stream_pos = end_pos;
        end_pos = file_stream_pos + chunk_size;
    } 

    var total_parts = file_chunks.length;
    for (var i = 0; i < total_parts; i++) {
        var xhr = new XMLHttpRequest();
        var formdata = new FormData();
        var file_chunk = file_chunks[i];
        var chunk_name = file.name + ".part_" + (i + 1) + "." + total_parts;
        formdata.append(chunk_name, file_chunk, chunk_name);
        xhr.open("POST", "/IO/Upload", false);
        xhr.addEventListener("load", function () {
            progress_elem.innerHTML = "<br/>Uploading item<br/>\"" + file.name + "\"<br/>" +
                (i + 1) + "MB /" + total_parts + "MB<br/><br/>";
        });
        xhr.send(formdata);
    }
}

// Upload files without submitting a form
var file_input = document.getElementById("FilesToUpload");
file_input.addEventListener('change', function () {
    var files = file_input.files;
    for (var i = 0; i < files.length; i++) {
        slice_and_upload_file(files[i]);
    }
    nav("");
});

// Create a new folder
var new_folder = function () {
    var folder_name = prompt("Enter a name for the new folder");
    if (folder_name.length) {
        var progress_elem = info_panel.appendChild(document.createElement("div"));
        progress_elem.style.borderBottom = "3px solid";
        $.ajax({
            url: "/IO/NewFolder",
            type: "POST",
            data: {
                name: folder_name,
            },
            success: function (result) {
                progress_elem.innerHTML = "<br/>Created new folder<br/>\"" + folder_name + "\"<br/><br/>";
                nav("");
            },
        });
    }
}

var delete_files = function () {
    var confirmation = prompt("Are you absolutely sure you want to delete the selected items?\n\
Folders will be deleted recursively. THIS CANNOT BE UNDONE.\n\nType \"yes\" to continue.");
    var items = document.getElementById("Explorer").children;
    
    if (confirmation != "yes") return;

    for (var i = 1; i < items.length; i++) {
        if (items[i].firstElementChild.checked) {
            var id = items[i].id;
            var xhr = new XMLHttpRequest();
            var formdata = new FormData();
            formdata.append("thing_to_delete", id);
            xhr.open("POST", "/IO/Delete", false);
            xhr.onload = function () {
                var progress_elem = info_panel.appendChild(document.createElement("div"));
                progress_elem.style.borderBottom = "3px solid";
                progress_elem.innerHTML = "<br/>Deleted item<br/>\"" + id.substring(id.indexOf("_") + 1) + "\"<br/><br/>";
            }
            xhr.send(formdata);
        }
    }
    nav("");
}

// This really should be global
var info_panel = document.getElementById("InfoPanelContent");
