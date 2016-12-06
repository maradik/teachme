using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMe.Areas.Student.Models.Home
{
    public class IndexRecallViewModelProvider
    {
        public IndexRecallViewModel[] Get()
        {
            return new[]
            {
                new IndexRecallViewModel
                {
                    AuthorName = "Вика",
                    Grade = 10,
                    Text = "Консультант быстро обнаружил ошибку в моем решении и ответ сошелся! Огромное спасибо!",
                    PhotoUrl = VirtualPathUtility.ToAbsolute("~/Content/Images/vika.jpg")
                },
                new IndexRecallViewModel
                {
                    AuthorName = "Mikle",
                    Grade = 8,
                    Text = "Спасибо за помощь в поиске литературы для реферата по биологии.",
                    PhotoUrl = VirtualPathUtility.ToAbsolute("~/Content/Images/mikle.jpg")
                },
                new IndexRecallViewModel
                {
                    AuthorName = "Танюшка",
                    Grade = 11,
                    Text = "Благодаря этому сайту я всегда могу перепроверить правильность решенных мной задач.",
                    PhotoUrl = VirtualPathUtility.ToAbsolute("~/Content/Images/tanushka.jpg")
                },
                new IndexRecallViewModel
                {
                    AuthorName = "Vano",
                    Grade = 9,
                    Text = "Респект авторам этого сайта! Теперь у меня куча свободного времени на личные дела.",
                    PhotoUrl = VirtualPathUtility.ToAbsolute("~/Content/Images/vano.jpg")
                }
            };
        }
    }
}