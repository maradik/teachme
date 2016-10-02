using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMe.Models
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class HumanAnnotationAttribute : Attribute
    {
        private string text;

        public HumanAnnotationAttribute(string text)
        {
            this.text = text;
        }

        public string Text => text;
    }
}