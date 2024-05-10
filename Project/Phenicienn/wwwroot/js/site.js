// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function redirect(id) {
    $.ajax({
        url: id,
        dataType: "json",
        method: 'get',
        success: function (data) {
            alert("secces");
        },
        error: function (err) {
            console.log(err);
           // alert(err);
           // alert("seccessss");
        }
    });
}

$(document).ready(function () {




});