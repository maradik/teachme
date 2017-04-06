using System;
using System.Web.Mvc;
using TeachMe.Models.Jobs;

namespace TeachMe.ViewModels.JobChat
{
    public interface IJobMessageAttachmentViewModel
    {
        Guid Id { get; set; }
        JobAttachmentType Type { get; set; }

        MvcHtmlString GetHtml(HtmlHelper htmlHelper);
    }
}