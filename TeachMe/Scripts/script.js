//-------------------------------
var jQueryExtensions = {
    escapeMetaChars: function (input) {
        return input.replace(
            new RegExp('[\!\"\#\$\%\&\'\(\)\*\+\,\.\/\:\;\<\=\>\?\@\[\\\]\^\`\{\|\}\~]', "g"),
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
            10000);
    });
};

var loadingNewMessages = false;
var loadNewMessages = function (successCallback) {
    if (!loadingNewMessages) {
        loadingNewMessages = true;
        var jobId = $("input#jobId:first").val();
        $.ajax(
            "/JobChat/_GetMessages?jobId=" + jobId + "&afterTicks=" + lastMessageCreationTick,
            {
                success: function (data) {
                    data = $(data);
                    data.hide();
                    $("#jobMessageContainer").append(data);
                    data.show('slow');
                    touchImages(data);
                    if (data.length) {
                        lastMessageCreationTick = $("div.job-message:last").attr("data-creationticks");
                    }
                    if (successCallback && typeof successCallback == "function") {
                        successCallback();
                    }
                },
                complete: function () {
                    loadingNewMessages = false;
                }
            }
        );
    }
};

var doingJobAction = false;
var doJobAction = function (actionButton, projectArea) {
    actionButton = $(actionButton);
    var formData = new FormData();
    formData.append("__RequestVerificationToken", $("[name=__RequestVerificationToken]:first").val());
    var actionConfirmationText = actionButton.attr("data-jobactionconfirmation");
    if (!actionConfirmationText || confirm(actionConfirmationText)) {
        var jobActionType = actionButton.attr("data-jobactiontype");
        formData.append("jobActionType", jobActionType);
        var jobId = actionButton.attr("data-jobId");
        if (jobId) {
            formData.append("jobId", jobId);
        }
        if (doingJobAction === false) {
            doingJobAction = true;
            $.ajax(
                {
                    url: "/" + projectArea + "Job/_DoJobAction",
                    type: "POST",
                    data: formData,
                    contentType: false,
                    processData: false,
                    dataType: "json",
                    success: function (response) {
                        if (response.HasErrors === true) {
                            alert(response.ErrorMessage);
                        }
                        if (response.RedirectUrl) {
                            window.location = response.RedirectUrl;
                        } else {
                            window.location.reload();
                        }
                    },
                    complete: function () {
                        doingJobAction = false;
                    }
                }
            );
        }
    } 
    return false;
};

var touchImages = function (imageElements) {
    imageElements = imageElements || $(document);
    imageElements.find(".gallery-container").photobox("a.gallery-item", {
        time: 0,
        autoplay: false,
        loop: false,
        thumbs: false,
        zoomable: true
    });
};

var showConfirmation = function (text) {
    return !!confirm(text);
}

var initJobActionDropdownLoader = function (projectArea) {
    $("button[id^='actionDropdownMenu']").click(function () {
        var dropDownButton = $(this);
        var attributeOfLoaded = "data-actionsloaded";
        if (!dropDownButton.attr(attributeOfLoaded)) {
            var actionsContainer = dropDownButton.parent("div").find("ul");
            var actionElement = $("<li><a href='#'>Загрузка...</a></li>");
            actionsContainer.append(actionElement);
            var jobId = dropDownButton.attr("data-jobid");
            $.ajax(
                "/" + projectArea + "Job/_GetAvailableActions?jobId=" + jobId,
                {
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    data: "{}",
                    dataType: "json",
                    success: function (jobActions) {
                        actionsContainer.html("");
                        if (jobActions.length != 0) {
                            jobActions.forEach(function (jobAction) {
                                var actionElement = $("<a href='#' />");
                                actionElement.text(jobAction.Text);
                                actionElement.attr("data-jobactiontype", jobAction.Value);
                                actionElement.attr("data-jobid", jobId);
                                if (jobAction.Value == 101 /*delete*/) {
                                    actionElement.attr("data-jobactionconfirmation", 'Удалить задачу?');
                                }
                                actionElement.click(function () {
                                    return doJobAction(this, projectArea);
                                });
                                var actionElementLi = $("<li/>");
                                actionElementLi.html(actionElement);
                                actionsContainer.append(actionElementLi);
                            });
                        }
                        else {
                            var dummyActionElement = $("<li><a href='#'>Нет действий</a></li>");
                            dummyActionElement.click(function () { return false; });
                            actionsContainer.html(dummyActionElement);
                        }
                    }
                }
            );
            dropDownButton.attr(attributeOfLoaded, "true");
        }
        return true;
    });
};

var ProjectAreas = {
    Root: "",
    Teacher: "Teacher/",
    Student: "Student/",
    Admin: "Admin/"
};

$(function () {
    $("[data-action=removeAttachment]").click(function () {
        removeAttachment(this);
        return false;
    });

    $("[data-deletionconfirmation]").click(function () {
        return showConfirmation($(this).attr("data-deletionconfirmation"));
    });

    touchImages();
});
