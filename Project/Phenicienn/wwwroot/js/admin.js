$(document).ready(() => {
    var ImageChanged = false;
    var sendImage = false;
    var image = $("#image");
    var imagepath = image.find("img").attr('src');
    var imageupload = $("#imageupload");
    var imagehtml = imageupload.html();
    var change = $("#change");
    imageupload.hide();
    imageupload.html("");
    change.click(e => {
        e.preventDefault();
        if (!ImageChanged) {
            ImageChanged = true;
            imageupload.html(imagehtml);
            imageupload.show();
            change.html("Cancel");
        }
        else {
            ImageChanged = false;
            imageupload.html("");
            imageupload.hide();
            $("#image img").attr('src', imagepath);
            change.html("Change");
        }
    });
    $("#imageupload").on("change", "#fileupload", e => {
        var file = e.target.files[0];
        var change = $("#change");
        if (file != null && file != undefined) {
            change.html("Cancel");
            $("#image img").attr('src', URL.createObjectURL(file));
            sendImage = true;
        }
        else {
            sendImage = false;
            $("#image img").attr('src', imagepath);
            
        }
    });
    $("#editform").submit(e => {
        if (!sendImage) {
            $("#imageupload").remove();
        }
        return true;
    });
});