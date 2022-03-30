using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VendorMgmt.Helper
{
    public static class CommonHelper
    {
        public static string GetRegistrationCode()
        {
            Random _random = new Random();  
            return _random.Next(10000, 99999).ToString();
        }
        public static MvcHtmlString SelectList_YesNoFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return htmlHelper.SelectList_YesNo(ExpressionHelper.GetExpressionText(expression), metadata.Model == null ? "True" : metadata.Model.ToString(), htmlAttributes);
        }
        public static MvcHtmlString SelectList_YesNo(this HtmlHelper html, string name, string selectedValue, object htmlAttributes = null)
        {
            Dictionary<bool, string> data = new Dictionary<bool, string>();
            data.Add(true, "Yes");
            data.Add(false, "No");

            return System.Web.Mvc.Html.SelectExtensions.DropDownList(html, name, new SelectList(data, "Key", "Value", selectedValue), htmlAttributes);
        }

        public static MvcHtmlString SelectList_AccountGroupName<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return htmlHelper.SelectList_AccountGroupName(ExpressionHelper.GetExpressionText(expression), metadata.Model == null ? "" : metadata.Model.ToString(), htmlAttributes);
        }
        public static MvcHtmlString SelectList_AccountGroupName(this HtmlHelper html, string name, string selectedValue, object htmlAttributes = null)
        {
            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(1, "Dofasco-Purchasing");
            data.Add(2, "Dofasco-OP");
            data.Add(3, "Dofasco-Payroll");
            data.Add(4, "Dofasco-Pension and Benefits");
            data.Add(5, "Dofasco-Other Non-Purchasing");
            data.Add(6, "Dofasco-Freight");
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(html, name, new SelectList(data, "Key", "Value", selectedValue), htmlAttributes);
        }
        public static MvcHtmlString SelectList_TypeofVendorRequest<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return htmlHelper.SelectList_TypeofVendorRequest(ExpressionHelper.GetExpressionText(expression), metadata.Model == null ? "" : metadata.Model.ToString(), htmlAttributes);
        }
        public static MvcHtmlString SelectList_TypeofVendorRequest(this HtmlHelper html, string name, string selectedValue, object htmlAttributes = null)
        {
            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(1, "New Vendor Activation");
            data.Add(2, "New Vendor Activation - GST# Update");
            data.Add(3, "Banking Change");
            data.Add(4, "Address Change");
            data.Add(5, "Vendor Name Change");
            data.Add(6, "Remittance Email Change");
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(html, name, new SelectList(data, "Key", "Value", selectedValue), htmlAttributes);
        }

        
    }
}
