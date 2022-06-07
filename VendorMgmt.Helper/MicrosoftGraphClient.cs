using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendorMgmt.Helper
{
    public class MicrosoftGraphClient
    {
        private static GraphServiceClient graphClient;
        private static ClientCredentialProvider authProvider;
        static MicrosoftGraphClient()
        {
            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
             .Create(ConfigurationManager.AppSettings["ClientId"])
             .WithTenantId(ConfigurationManager.AppSettings["TenantId"])
             .WithClientSecret(ConfigurationManager.AppSettings["ClientSecret"])
             .Build();
            authProvider = new ClientCredentialProvider(confidentialClientApplication);
        }
        public static GraphServiceClient GetGraphServiceClient()
        {
            graphClient = new GraphServiceClient(authProvider);
            return graphClient;
        }

        public static async Task<List<AzureUserList>> GetAllUsers(bool IncludeDefault=false)
        {
            List<AzureUserList> lstuser = new List<AzureUserList>();
            graphClient = new GraphServiceClient(authProvider);
            var memer = await graphClient.Users.Request().GetAsync();
            foreach (var user in memer.CurrentPage)
            {
                //Console.WriteLine(JsonConvert.SerializeObject(user));
                Microsoft.Graph.User objuser = user;
                lstuser.Add(new AzureUserList { AzureGroupId = "", DisplayName = objuser.DisplayName, AzureObjectId = objuser.Id, FirstName = objuser.GivenName, EmailAddress = objuser.UserPrincipalName });
                // Only output the custom attributes...
                //Console.WriteLine(JsonConvert.SerializeObject(user.AdditionalData));
            }
            if (IncludeDefault)
            {
                lstuser.Insert(0, new AzureUserList { DisplayName = "" });
            }
            return lstuser;
        }
    }

    public class AzureUserList
    {
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string FirstName { get; set; }
        public string AzureObjectId { get; set; }
        public string AzureGroupId { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
    }
}
