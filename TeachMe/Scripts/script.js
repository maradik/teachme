var addUploadedFileInput = function () {
    var inputsCount = $("input[name^='uploadedFiles']").length;
    var newInput = $('<input type="file" name="uploadedFiles[' + inputsCount + ']" class="form-control"/>');
    $("#uploadedFileInputContainer").append(newInput);
};

var removeAttachment = function (link) {
    var attachmentBlock = $(link).parents(".attachment");
    attachmentBlock.hide("slow");
    attachmentBlock.find("input").val("");
};

$(function () {
    $("#addUploadedFile").click(function () {
        addUploadedFileInput();
        return false;
    });
    addUploadedFileInput();
    $("[data-action=removeAttachment]").click(function () {
        removeAttachment(this);
        return false;
    });
});