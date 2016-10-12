using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMe.Areas.Teacher.Models.Home
{
    public class IndexRecallViewModelProvider
    {
        public IndexRecallViewModel[] Get()
        {
            return new[]
            {
                new IndexRecallViewModel
                {
                    AuthorName = "Валентина Петровна",
                    Text = "Помогать ученикам грызть гранит науки и получать достойное вознаграждение, кажется, стало реально.",
                    PhotoUrl = VirtualPathUtility.ToAbsolute("~/Content/Images/valentina-petrovna.jpg"),
                    Subject = "Математика",
                    City = "Москва"
                },
                new IndexRecallViewModel
                {
                    AuthorName = "Наталья",
                    Text = "При помощи этого сайта я могу заниматься репетиторством онлайн. Спасибо авторам!",
                    PhotoUrl = VirtualPathUtility.ToAbsolute("~/Content/Images/natalya.jpg"),
                    Subject = "Физика",
                    City = "Ярославль"
                },
                new IndexRecallViewModel
                {
                    AuthorName = "Ирина",
                    Text = "Удобный сервис для работы на дому, все предельно ясно и прозрачно.",
                    PhotoUrl = VirtualPathUtility.ToAbsolute("~/Content/Images/irina.jpg"),
                    Subject = "Русский язык",
                    City = "Кемерово"
                },
                new IndexRecallViewModel
                {
                    AuthorName = "Мария Андреевна",
                    Text = "Не очень удобно, что деньги выплачиваются раз в неделю. В остальном - все неплохо.",
                    PhotoUrl = VirtualPathUtility.ToAbsolute("~/Content/Images/mariya-andreevna.jpg"),
                    Subject = "Химия",
                    City = "Екатеринбург"
                }
            };
        }
    }
}