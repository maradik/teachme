﻿using System;
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
                    Text = "Это самый удобный онлайн-решебник из всех, которые я когда-либо встречала.",
                    PhotoUrl = VirtualPathUtility.ToAbsolute("~/Content/Images/vika.jpg")
                },
                new IndexRecallViewModel
                {
                    AuthorName = "Mikle",
                    Grade = 8,
                    Text = "Теперь можно не тратить время на скучную физику, она все равно мне не пригодится.",
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