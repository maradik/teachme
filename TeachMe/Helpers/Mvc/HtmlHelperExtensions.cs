using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace TeachMe.Helpers.Mvc
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString DropDownListWithEmptyElementFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                                                                       Expression<Func<TModel, TProperty>> expression,
                                                                       IEnumerable<SelectListItem> selectList,
                                                                       object htmlAttributes = null,
                                                                       SelectListItem emptyElement = null)
        {
            emptyElement = emptyElement ?? new SelectListItem { Value = 0.ToString(), Text = "Выберите значение" };
            return htmlHelper.DropDownListFor(expression, new[] {emptyElement}.Concat(selectList), htmlAttributes);
        }
    }
}