//-------------------------------
var jQueryExtensions = {
    escapeMetaChars: function (input) {
        return input.replace(
            new RegExp(/[\!\"\#\$\%\&\'\(\)\*\+\,\.\/\:\;\<\=\>\?\@\[\\\]\^\`\{\|\}\~]/, "g"),
            function(match) { return "\\" + match; });
    }
};

var addUploadedFileInput = function (namePrefix) {
    namePrefix = namePrefix ? namePrefix + "." : "";
    var inputsCount = $("input[name^='" + jQueryExtensions.escapeMetaChars(namePrefix + "uploadedFiles") + "']").length;
    var newInput = $('<input type="file" name="' + namePrefix + 'uploadedFiles[' + inputsCount + ']" class="form-control"/>');
    $("#uploadedFileInputContainer").append(newInput);
};

var initUploadedFileBlock = function() {
    $("#addUploadedFile").click(function () {
        addUploadedFileInput($(this).attr("data-inputnameprefix"));
        return false;
    });
    $("#addUploadedFile").click();
}

var addSubjectSelector = function (name, jobSubjectsOptions) {
    var selectorsCount = $("select[name^='" + jQueryExtensions.escapeMetaChars(name) + "']").length;
    var newSelector = $('<select name="' + name + '[' + selectorsCount + ']" class="form-control">' + jobSubjectsOptions + '</select>');
    $("#subjectListContainer").append(newSelector);
};

var initSubjectSelectorBlock = function (jobSubjects) {
    jobSubjectsOptions = "";
    jobSubjects.forEach(function (item) {
        jobSubjectsOptions += "<option value='" + item.Id + "'>" + item.Title + "</option>";
    });
    $("#addSubjectSelector").click(function () {
        addSubjectSelector($(this).attr("data-controlname"), jobSubjectsOptions);
        return false;
    });
    $("#addSubjectSelector").click();
}


var removeAttachment = function (link) {
    var attachmentBlock = $(link).parents(".attachment");
    attachmentBlock.hide("slow");
    attachmentBlock.find("input").val("");
};

var lastMessageCreationTick = 0;

var autoLoadNewMessages = function() {
    loadNewMessages(function () {
        setTimeout(function() {
            autoLoadNewMessages();
            },
            5000);
    });
};

var loadNewMessages = function (successCallback) {
    var jobId = $("input#jobId:first").val();
    $.ajax(
        "/JobChat/_GetMessages?jobId=" + jobId + "&afterTicks=" + lastMessageCreationTick,
        {
            success: function (data) {
                data = $(data);
                data.hide();
                $("#jobMessageContainer").append(data);
                data.show('slow');
                if (data.length) {
                    lastMessageCreationTick = $("div.job-message:last").attr("data-creationticks");
                }
                if (successCallback && typeof successCallback == "function") {
                    successCallback();
                }
            }
        }
    );
};

var showConfirmation = function (text) {
    return !!confirm(text);
}

$(function () {
    $("[data-action=removeAttachment]").click(function () {
        removeAttachment(this);
        return false;
    });

    $("[data-deletionconfirmation]").click(function () {
        return showConfirmation($(this).attr("data-deletionconfirmation"));
    });
});

