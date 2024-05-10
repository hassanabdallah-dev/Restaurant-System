$(document).ready(function () {
    var imageIsSet = false;
    var lastFile = undefined;
    $("#customFile").on("change", function (e) {
        var lastFile = e.target.files[0];
        if (lastFile != null && lastFile != undefined) {
            if (!imageIsSet) {
                $("#image").html('<img class="mb-3" src="' + URL.createObjectURL(lastFile) + '"id="chosenimage" alt="Category" width="200" height="200"/>');
                imageIsSet = true;
            }
            else {
                $("#image img").attr('src', URL.createObjectURL(lastFile));
            }
        }
        else {
            imageIsSet = false;
            $("#image").html("");
        }
    })
});