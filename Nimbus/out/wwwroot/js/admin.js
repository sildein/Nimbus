var load_server_settings = function () {
    var xhr = new XMLHttpRequest();
    xhr.open("GET", "/Admin/ServerSettings");
    xhr.addEventListener("load", function () {
        if (xhr.status == 200)
            document.getElementById("SettingsView").innerHTML = xhr.responseText;
    });
    xhr.send();
}

var load_user_settings = function() {
    var xhr = new XMLHttpRequest();
    xhr.open("GET", "/Admin/UserSettings");
    xhr.addEventListener("load", function () {
        if (xhr.status == 200)
            document.getElementById("SettingsView").innerHTML = xhr.responseText;
    });
    xhr.send();
}

var send_server_settings = function() {
    var xhr = new XMLHttpRequest();
    var formdata = new FormData();
    formdata.append("Title", document.getElementById("TitleBox").value);
    formdata.append("Prefix", document.getElementById("PrefixBox").value);
    formdata.append("Port", document.getElementById("PortBox").value);

    var admin_password = document.getElementById("AdminPassword").value;
    var admin_password_confirm =
        document.getElementById("AdminPasswordConfirm").value;

    if (admin_password == admin_password_confirm) {
        formdata.append("AdminPassword", admin_password);
    } else {
        formdata.append("AdminPassword", "_MISMATCH_");
    }

    xhr.open("POST", "/Admin/SaveServerSettings");
    xhr.addEventListener("load", function () {
        window.location.reload();
    });
    xhr.send(formdata);
}

var send_user_settings = function () {
    var users = document.getElementById("Users");

    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/Admin/SaveUserSettings");
    var formdata = new FormData();

    for (var i = 0; i < users.children.length; i++) {
        var username = users.children[i].id;
        var password = document.getElementById("Users").children[i].children[4].value;
        console.log(password);
        var confirm_password = document.getElementById("Users").children[i].children[9].value;
        console.log(confirm_password);
        if (password == confirm_password) {
            formdata.append(username, password);
        } else {
            formdata.append(username, "_MISMATCH_");
        }
    }
    xhr.addEventListener("load", function () {
        window.location.reload();
    });
    xhr.send(formdata);
}

var new_user = function () {
    var xhr = new XMLHttpRequest();
    var username = document.getElementById("NewUserName").value;
    var password = document.getElementById("NewUserPassword").value;
    var password_confirm = document.getElementById("NewUserPasswordConfirm").value;

    xhr.open("POST", "/Admin/NewUser");
    var formdata = new FormData();
    if (password == password_confirm) {
        formdata.append("Username", username);
        formdata.append("Password", password);
        xhr.addEventListener("load", function () {
            window.location.reload();
        });
        xhr.send(formdata);
    } else {
        document.getElementById("ErrorMessage").innerHTML = "Passwords do not match";
    }
}

var delete_user = function (user) {
    var xhr = new XMLHttpRequest();
    var formdata = new FormData();
    formdata.append("Username", user);
    xhr.open("POST", "/Admin/DeleteUser");
    xhr.addEventListener("load", function () {
        window.location.reload();
    });
    var confirmation = confirm("Are you sure you want to delete this user?\n\
    THIS CANNOT BE UNDONE.");
    if (confirmation) {
        xhr.send(formdata);
    }
}

var show_modal = function () {
    var add_user_dialog = document.getElementById("NewUserModal");
    add_user_dialog.style.display = "block";
}

var close_modal = function () {
    var add_user_dialog = document.getElementById("NewUserModal");
    add_user_dialog.style.display = "none";
}

document.onload = load_server_settings();
