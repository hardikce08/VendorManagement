using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
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
             .Create("d72e3a81-37ce-4b5c-b08e-5e28cc1d66c8")
             .WithTenantId("d121b713-6356-41c5-b535-57ca3629c005")
             .WithClientSecret("dfR7Q~hn0Ddv~RN5qeCYypW1hhn5tAvv9pXGS")
             .Build();
            authProvider = new ClientCredentialProvider(confidentialClientApplication);
        }
        public static GraphServiceClient GetGraphServiceClient()
        {
            graphClient = new GraphServiceClient(authProvider);
            return graphClient;
        }

        public static async Task<List<AzureUserList>> GetAllUsers()
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
