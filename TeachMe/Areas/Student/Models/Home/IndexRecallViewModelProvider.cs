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
                    AuthorName = "Татьяна",
                    Status = "Студент 2 курса",
                    Text = "Благодаря этому сайту я успешно сдала рефераты по физике, термеху, БЖД.",
                    PhotoUrl = VirtualPathUtility.ToAbsolute("~/Content/Images/tanushka.jpg")
                },
                new IndexRecallViewModel
                {
                    AuthorName = "Mikle",
                    Status = "Студент 1 курса",
                    Text = "Сдал курсач по нормам права на хор. Сделали за день, спасибо!",
                    PhotoUrl = VirtualPathUtility.ToAbsolute("~/Content/Images/mikle.jpg")
                },
                new IndexRecallViewModel
                {
                    AuthorName = "Вика",
                    Status = "10 класс",
                    Text = "Здорово, что при заказе я сама определяю цену работы.",
                    PhotoUrl = VirtualPathUtility.ToAbsolute("~/Content/Images/vika.jpg")
                },
                new IndexRecallViewModel
                {
                    AuthorName = "Vano",
                    Status = "11 класс",
                    Text = "Респект авторам этого сайта! Теперь у меня куча свободного времени на личные дела.",
                    PhotoUrl = VirtualPathUtility.ToAbsolute("~/Content/Images/vano.jpg")
                }
            };
        }
    }
}