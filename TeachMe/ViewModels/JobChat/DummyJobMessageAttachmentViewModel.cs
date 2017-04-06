using System;
using System.Web.Mvc;
using TeachMe.Models.Jobs;

namespace TeachMe.ViewModels.JobChat
{
    public class DummyJobMessageAttachmentViewModel : IJobMessageAttachmentViewModel
    {
        public DummyJobMessageAttachmentViewModel(JobAttachment entity)
        {
            Id = entity.Id;
            Type = entity.Type;
            OriginFileName = entity.OriginFileName;
        }

        public Guid Id { get; set; }
        public JobAttachmentType Type { get; set; }

        private string OriginFileName { get; }

        public MvcHtmlString GetHtml(HtmlHelper htmlHelper)
        {
            return new MvcHtmlString(
                $"<a href=\"#\" onclick=\"alert('Нет доступа! Требуется оплата'); return false;\">" +
                $"Скачать файл {htmlHelper.Encode(OriginFileName)}" +
                $"</a>");
        }
    }
}