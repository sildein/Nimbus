/*
 * Main.js
 * This file is a part of Tachyon. Copyright (c) 2017-present Jesse Jones.
 */

// Navigation
var nav = function (folder) {
    $.ajax({
        url: "/Main/Explorer",
        data: {
            folder: folder,
        },
        success: function (data) {
            $("#Explorer").html(data);
        },
    });
}

var download = function () {
    var downloads = [];
    var items = document.getElementById("Explorer").children;
    for (var i = 0; i < items.length; i++) {
        var item = items[i];
        if (item.firstElementChild.checked && item.id.startsWith("file_")) {
            info_panel.innerHTML += "Downloading<br/>" + item.id.substr(5) + "<br/><br/>";
            window.open("/Main/Download?file=" + item.id.substr(5), "_blank");
        }
    }
}

// Upload files
var file_input = document.getElementById("FilesToUpload");
$("#FilesToUpload").change(function () {
    var files = file_input.files;
    for (var i = 0; i < files.length; i++) {
        var file = files[i];
        var formdata = new FormData();
        info_panel.innerHTML += "Uploading " + file.name + "<br/><br/>";
        console.log("Uploading " + file.name + "<br/><br/>");
        formdata.append("file" + i, file);
        $.ajax({
            //async: false,
            type: "POST",
            url: "/Main/Explorer",
            contentType: false,
            processData: false,
            data: formdata,
            success: function (result) {
                info_panel.innerHTML += "Done with<br/>" + file.name + "<br/><br/>";
                console.log("Done with<br/>" + file.name + "<br/><br/>");
            },
        });
    }
    nav("");
});

var new_folder = function () {
    var folder_name = prompt("Enter a name for the new folder");
    if (folder_name.length) {
        $.ajax({
            url: "/Main/NewFolder",
            type: "POST",
            data: {
                name: folder_name,
            },
            success: function (result) {
                info_panel.innerHTML += "Created new folder<br/>\"" + folder_name + "\"<br/><br/>";
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
    for (var i = 0; i < items.length; i++) {
        if (items[i].firstElementChild.checked) {
            $.ajax({
                url: "/Main/Delete",
                type: "POST",
                data: {
                    thing_to_delete: items[i].id,
                },
                success: function (result) {
                    info_panel.innerHTML += "Deleted item<br/>\"" + "\"<br/><br/>";
                },
            });
        }
    }
    nav("");
}

// This really should be global
var info_panel = document.getElementById("InfoPanelContent");
