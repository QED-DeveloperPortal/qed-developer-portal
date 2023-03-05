using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.Azure.WebJobs.Host;
using System.Linq;
using System.Text;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using DevPortal.Api.Helpers;


namespace DevPortal.Api
{
    public static class GetRoles
    {
        private static GraphServiceClient GraphServiceClient { get; set;}

        [FunctionName("GetRoles")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            ClaimsPrincipal claimsPrincipal)
        {
            log.LogInformation("* Determining role membership for authenticated user...");
            
            Dictionary<string, string> RoleGroupMapping = new Dictionary<string, string>();
            RoleGroupMapping.Add("95cd61fc-4b31-434b-a74c-f161e94c03fd", "internal_dev"); // maps from Azure AD Group "InternalDeveloper"...
            RoleGroupMapping.Add("1bf5763e-cad0-4f96-b335-e1af6b5d9fc4", "vendor"); // maps from Azure AD Group "Vendor"...
            RoleGroupMapping.Add("cfc001d0-557d-488f-b4ba-7c72c2ba751d", "developer_certified"); // maps from Azure AD Group "DeveloperCertified"...
            RoleGroupMapping.Add("a98e5132-90a1-47a5-9ac0-01f0820227c1", "developer_vendor"); // maps from Azure AD Group "DeveloperVendor"...
            RoleGroupMapping.Add("2829f9fa-cad5-49c7-aa66-b066881ffae2", "developer_corporate"); // maps from Azure AD Group "DeveloperCorporate"...
            RoleGroupMapping.Add("3adde8e0-2533-40d8-8aa0-606b0d7409e6", "moderator_general"); // maps from Azure AD Group "ModeratorGeneral"...
            RoleGroupMapping.Add("b4b5d88a-ff99-453c-9384-b5882dea0ba4", "moderator_policy"); // maps from Azure AD Group "ModeratorPolicy"...
            RoleGroupMapping.Add("475c6a62-907d-4cd4-b6e4-5d18da56b633", "moderator_security"); // maps from Azure AD Group "ModeratorSecurity"...

            var response = new RoleResponse();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string userId = data?.userId;

            if (!string.IsNullOrEmpty(userId))
            {
                log.LogInformation($"* userId '{userId}' found in the requestBody");

                GraphServiceClient graphClient = GetAuthenticatedGraphClient();

                var graphResult = await graphClient.Users[userId].MemberOf
                    .Request()
                    .GetAsync();

                if (graphResult == null || graphResult.Count == 0)
                    log.LogInformation($"* graphResult is null or empty - no Group memberships found...");
                else
                {
                    foreach(var roleAssignment in graphResult)
                    {
                        if (RoleGroupMapping.ContainsKey(roleAssignment.Id))
                        {
                            log.LogInformation($"* Adding Group membership for {RoleGroupMapping[roleAssignment.Id]}");
                            response.Roles.Add(RoleGroupMapping[roleAssignment.Id]);
                        }
                    }
                }
            }
            else
                log.LogInformation($"* Unable to derive userId from requestBody...");

            return new OkObjectResult(response);
        }

        private static GraphServiceClient GetAuthenticatedGraphClient()
        {
            var authenticationProvider = CreateAuthorizationProvider();
            GraphServiceClient = new GraphServiceClient(authenticationProvider);

            return GraphServiceClient;
        }

        private static IAuthenticationProvider CreateAuthorizationProvider()
        {
            var clientId = System.Environment.GetEnvironmentVariable("AADB2C_PROVIDER_CLIENT_ID", EnvironmentVariableTarget.Process);
            var clientSecret = System.Environment.GetEnvironmentVariable("AADB2C_PROVIDER_CLIENT_SECRET", EnvironmentVariableTarget.Process);
            var tenantId = System.Environment.GetEnvironmentVariable("AADB2C_PROVIDER_TENANT_ID", EnvironmentVariableTarget.Process);
            var authority = $"https://login.microsoftonline.com/{tenantId}/v2.0";

            //this specific scope means that application will default to what is defined in the application registration rather than using dynamic scopes
            List<string> scopes = new List<string>();
            scopes.Add("https://graph.microsoft.com/.default");

            var cca = ConfidentialClientApplicationBuilder.Create(clientId)
                                    .WithAuthority(authority)
                                    .WithClientSecret(clientSecret)
                                    .Build();
            
            return new MsalAuthenticationProvider(cca, scopes.ToArray());
        }
    }

    public class RoleResponse
    {
        public List<string> Roles { get; set; }

        public RoleResponse()
        {
            Roles = new List<string>();
        }
    }
}