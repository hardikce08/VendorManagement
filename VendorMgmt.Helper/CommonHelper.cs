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
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(html, name, new SelectList(data, "Value", "Value", selectedValue), htmlAttributes);
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
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(html, name, new SelectList(data, "Value", "Value", selectedValue), htmlAttributes);
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
            data.Add(2, "Submitted to Purchasing Manager");
            data.Add(3, "Rejected by Purchasing Manager");
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
        public static MvcHtmlString SelectList_PaymentTerms<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            return htmlHelper.SelectList_PaymentTerms(ExpressionHelper.GetExpressionText(expression), metadata.Model == null ? "" : metadata.Model.ToString(), htmlAttributes);
        }
        public static MvcHtmlString SelectList_PaymentTerms(this HtmlHelper html, string name, string selectedValue, object htmlAttributes = null)
        {
            Dictionary<int, string> data = new Dictionary<int, string>();
            string Terms = ".5%3 N30,.5%5 N30,.5%10 N30,.5%15 N30,.5%15 N45,.5% 1Day N2,.5%ONRECN01,.5%3 N4,1%5 N30,1%7 N30,1%10 N45,1%10 N30,1% 10TH MF,1%15 N30,1%20 N30,1%25 N40,1%30 N31,1% ONRECN01,1.25% ONREC N30,1.5%15 N 30,1.5%10 N 30,15%5 N30,2%10 N15,2%10 N11,2%15 N30,2%15 N60,2% 15th MF,2% 1DAY N2,2%20 N30,2%25 N30,2%25 N45,2%30 N31,2%45 N60,2% ONREC N30,2% ONREC N01,2%10 N30,2.5%30 N31,2.5%10 N30,3&5 N30,3%10 N45,3%15 N30,3%30 N31,3%60 N31,3% ONRECN01,3%10 N30,4%15 N30,4% ONRECN01,4%10 N30,5%10 N30,5%5 N30,5%15 N16,5%15 N30,5%30 N31,5% ONRECN01,5%10 N45,6%15 N30,6%10 N30,6%10 N30,6%5 N30,7%15 N30,MONTH-END,NET 10 DAYS,NET 120 DAYS,NET 14 DAYS,NET 15 DAYS,NET 15TH CM,NET 15TH MF,NET 20 DAYS,NET 20TH MF,NET 25 DAYS,NET 25TH CM,NET 25TH MF,NET 26TH CM,NET 27TH CM,NET 3 DAYS,NET 30 DAYS,NET 30TH MF,NET 35 DAYS,NET 4 DAYS,NET 40 DAYS,NET 45 DAYS,NET 5 DAYS,NET 60 DAYS,NET 7 DAYS,NET 90 DAYS,ON RECEIPT,";
            int cnt = 1;
            foreach (var term in Terms.Split(','))
            {
                data.Add(cnt, term);
                cnt = cnt + 1;
            }
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(html, name, new SelectList(data, "Value", "Value", selectedValue), htmlAttributes);
        }
        //public static async Task<MvcHtmlString> SelectList_Country<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null) where TModel : class
        //{
        //    ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
        //    return await htmlHelper.SelectList_Country(ExpressionHelper.GetExpressionText(expression), metadata.Model == null ? "True" : metadata.Model.ToString(), htmlAttributes);
        //}
        //public static async Task<MvcHtmlString> SelectList_Country(this HtmlHelper html, string name, string selectedValue, object htmlAttributes = null)
        //{
        //    Dictionary<string, string> data = new Dictionary<string, string>();
        //    VendorService
        //    var lstusers = await vs.GetAllUsers();
        //    foreach (var user in lstusers)
        //    {
        //        data.Add(user.EmailAddress, user.DisplayName);
        //    }

        //    return System.Web.Mvc.Html.SelectExtensions.DropDownList(html, name, new SelectList(data, "Key", "Value", selectedValue), htmlAttributes);
        //}
    }
}
