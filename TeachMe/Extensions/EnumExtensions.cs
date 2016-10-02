using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachMe.Models;

namespace TeachMe.Extensions
{
    public static class EnumExtensions
    {
        public static string GetHumanAnnotation<T>(this T enumValue)
            where T : struct
        {
            var humanAnnotation = typeof(T).GetField(enumValue.ToString())
                .GetCustomAttributes(false)
                .Where(x => x is HumanAnnotationAttribute)
                .Select(x => x as HumanAnnotationAttribute)
                .DefaultIfEmpty(new HumanAnnotationAttribute(enumValue.ToString()))
                .Single();

            return humanAnnotation.Text;
        }
    }
}