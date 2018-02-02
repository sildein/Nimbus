var login_button = document.getElementById("LoginButton");
var error_message = document.getElementById("ErrorMessage");

login_button.addEventListener("click", function () {
    var xhr = new XMLHttpRequest();
    var formdata = new FormData();
    formdata.append("Username", document.getElementById("Username").value);
    formdata.append("Password", document.getElementById("Password").value);
    xhr.open("POST", "/Auth/TryLogin");
    xhr.addEventListener("load", function () {
        if (xhr.status == 200) window.location = "/Main";
        else error_message.innerText = "Invalid login info";
    });
    xhr.send(formdata);
});