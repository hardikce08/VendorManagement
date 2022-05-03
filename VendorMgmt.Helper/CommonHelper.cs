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
        public static MvcHtmlString SelectList_ApplicationStatus<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return htmlHelper.SelectList_ApplicationStatus(ExpressionHelper.GetExpressionText(expression), metadata.Model == null ? "" : metadata.Model.ToString(), htmlAttributes);
        }
        public static MvcHtmlString SelectList_ApplicationStatus(this HtmlHelper html, string name, string selectedValue, object htmlAttributes = null)
        {
            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(0, "");
            data.Add(1, "Submitted by Vendor");
            data.Add(2, "Submitted to Purch Manager");
            data.Add(3, "Rejected by Purch Manager");
            data.Add(4, "Submitted to Legal");
            data.Add(5, "Rejected by Legal");
            data.Add(6, "Submitted to Treasury");
            data.Add(7, "Rejected by Treasury");
            data.Add(8, "Submitted to 2nd Level Approver");
            data.Add(9, "Validation Pending");
            data.Add(10, "Process Completed");
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(html, name, new SelectList(data, "Value", "Value", selectedValue), htmlAttributes);
        }

        public static MvcHtmlString SelectList_DiversityBusinessCertification<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return htmlHelper.SelectList_DiversityBusinessCertification(ExpressionHelper.GetExpressionText(expression), metadata.Model == null ? "" : metadata.Model.ToString(), htmlAttributes);
        }
        public static MvcHtmlString SelectList_DiversityBusinessCertification(this HtmlHelper html, string name, string selectedValue, object htmlAttributes = null)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("", "Select Certification");
            data.Add("MBE - Minority Business Enterprise", "MBE - Minority Business Enterprise");
            data.Add("WBE - Women Business Enterprise", "WBE - Women Business Enterprise");
            data.Add("VET / VOB - Veteran Owned Enterprise", "VET / VOB - Veteran Owned Enterprise");
            data.Add("Not Applicable", "Not Applicable");
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(html, name, new SelectList(data, "Key", "Value", selectedValue), htmlAttributes);
        }
        public static MvcHtmlString SelectList_PaymentCurrency<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return htmlHelper.SelectList_PaymentCurrency(ExpressionHelper.GetExpressionText(expression), metadata.Model == null ? "" : metadata.Model.ToString(), htmlAttributes);
        }
        public static MvcHtmlString SelectList_PaymentCurrency(this HtmlHelper html, string name, string selectedValue, object htmlAttributes = null)
        {
            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(0, "");
            data.Add(1, "CAD");
            data.Add(2, "USD");
            data.Add(3, "EUR");
            data.Add(4, "GBP");
            data.Add(5, "JPY");
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(html, name, new SelectList(data, "Value", "Value", selectedValue), htmlAttributes);
        }


        public static async Task<MvcHtmlString> SelectList_AzureAdUsers<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return await htmlHelper.SelectList_AzureAdUsers(ExpressionHelper.GetExpressionText(expression), metadata.Model == null ? "True" : metadata.Model.ToString(), htmlAttributes);
        }
        public static async Task<MvcHtmlString> SelectList_AzureAdUsers(this HtmlHelper html, string name, string selectedValue, object htmlAttributes = null)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            var lstusers = await MicrosoftGraphClient.GetAllUsers();
            foreach (var user in lstusers)
            {
                data.Add(user.EmailAddress, user.DisplayName);
            }

            return System.Web.Mvc.Html.SelectExtensions.DropDownList(html, name, new SelectList(data, "Key", "Value", selectedValue), htmlAttributes);
        }
    }
}
