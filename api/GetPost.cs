using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DevPortal.Models;
using Microsoft.Graph;
using Newtonsoft.Json.Linq;
using Octokit;
using Post = DevPortal.Models.Post;


namespace DevPortal.Api
{
    public static class GetPost
    {
      private static ILogger _logger;

      [FunctionName("GetPost")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            _logger = log;
            log.LogInformation("C# HTTP trigger function processed a GetPost request.");

            string filePath;

            if (req != null && !String.IsNullOrEmpty(req.Query["filePath"]))
            {
              filePath = req.Query["filePath"];
}
            else
            {
              filePath = "_posts/2023/2023-03-02-configure-azure-keyvault.md";
            }
            

            log.LogInformation("Incoming Request Body:" + req.Body);

            /*string jsonPayload = "{\"filePath\":\"_posts/2023/2023-03-02-configure-azure-keyvault.md\"}";
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonPayload);
            MemoryStream stream = new MemoryStream(byteArray);
            req.Body = stream;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;
            filePath = data.filePath;// "_posts/2023/2023-03-02-configure-azure-keyvault.md";*/

            var postResponse = await GetPostFromRepository(filePath);
            log.LogInformation("Response after trying to get a post: ", postResponse.ResponseMessage);

            string responseMessage = string.IsNullOrEmpty(filePath)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {filePath}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(postResponse.ResponseMessage);
        }

        

        private static async Task<PostResponse> GetPostFromRepository(string filePath)
        {
          var postResponse = new PostResponse();
          var res = new RepositoryContentChangeSet();

          var (owner, repoName, branch) = ("QED-DeveloperPortal", "qed-developerportal.github.io", "master");

          string token = System.Environment.GetEnvironmentVariable("GITHUB_TOKEN", EnvironmentVariableTarget.Process);

          var gitHubClient = new GitHubClient(new Octokit.ProductHeaderValue("DevPortal"));
          gitHubClient.Credentials = new Credentials(token);

          string commitMessage = $"Retrieved Content for { filePath }";

          try
          {
            _logger.LogInformation("Checking if file exists in the repository...");
            var existingFile =
              await gitHubClient.Repository.Content.GetAllContents(owner, repoName, filePath);

            if (existingFile != null)
            {
              _logger.LogInformation("File with same name exists.");

              string content = existingFile[0].Content;
              string sha = existingFile[0].Sha;


              //To update existing post
              /*commitMessage = $"Update commit for {_newPost.FilePath}";
              var updateChangeSet = await gitHubClient.Repository.Content.UpdateFile(owner, repoName, filePath,
               new UpdateFileRequest(commitMessage, fileContent, existingFile[0].Sha, branch));*/

              postResponse.IsSuccess = true;
              postResponse.ResponseMessage =
                "An existing file ( " + sha + ") found in the repository and can be edited! \n\n " + content;
            }
            _logger.LogInformation($"** responseBody: {res}");

            
          }
          catch (Exception ex)
          {
            postResponse.IsSuccess = false;
            postResponse.ResponseMessage = "The file could not be retrieved. Check for error logs. Exception: " + ex.Message;
            _logger.LogInformation("The file could not be retrieved.Check for error logs. Exception: " + ex.Message);

          }
          return postResponse;
}
     }
}
