using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using TeachMe.Models.Jobs;

namespace TeachMe.ViewModels.JobChat
{
    public class JobMessageAttachmentViewModel : IJobMessageAttachmentViewModel
    {
        public JobMessageAttachmentViewModel(JobAttachment entity, Guid messageId)
        {
            Id = entity.Id;
            Type = entity.Type;
            FileName = entity.FileName;
            OriginFileName = entity.OriginFileName;
            MessageId = messageId;
        }

        public Guid Id { get; set; }
        public JobAttachmentType Type { get; set; }

        private string FileName { get; }
        private string OriginFileName { get; }
        private Guid MessageId { get; }

        public MvcHtmlString GetHtml(HtmlHelper htmlHelper)
        {
            return Type == JobAttachmentType.Image ? GetImageHtml(htmlHelper) : GetDefaultHtml(htmlHelper);
        }

        private MvcHtmlString GetImageHtml(HtmlHelper htmlHelper)
        {
            return new MvcHtmlString(
                    $"<a href=\"/Uploads/{htmlHelper.Encode(FileName)}?width=1280\" class=\"gallery-item\" data-gallery=\"jobcomment{htmlHelper.Encode(MessageId)})\">" +
                    $"<img src=\"/Uploads/{htmlHelper.Encode(FileName)}?width=200\" title=\"{htmlHelper.Encode(OriginFileName)}\" style=\"max-width: 100%;\" />" +
                    $"</a>");
        }

        private MvcHtmlString GetDefaultHtml(HtmlHelper htmlHelper)
        {
            return htmlHelper.ActionLink($"Скачать {OriginFileName}", "DownloadAttachment", "JobChat", new { area = "", messageId = MessageId, attachmentId = Id }, new { title = $"Скачать {OriginFileName}" });
        }
    }
}