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

            string filePath = "";
            var postResponse = new PostResponse();

            try
            {
              if (req != null && !String.IsNullOrEmpty(req.Query["filePath"]))
              {
                log.LogInformation("Incoming Request Body:" + req.Body);
                filePath = req.Query["filePath"];

              }

              //string jsonPayload = "{\"filePath\":\"_posts/2023/2023-03-02-configure-azure-keyvault.md\"}";
              //byte[] byteArray = Encoding.UTF8.GetBytes(jsonPayload);
              //MemoryStream stream = new MemoryStream(byteArray);
              //req.Body = stream;

              //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
              //dynamic data = JsonConvert.DeserializeObject(requestBody);
              //filePath = data.filePath;

              postResponse = await GetPostFromRepository(filePath);
              log.LogInformation("Response after calling GetPost: ", postResponse.ResponseMessage);
            }
            catch (Exception ex)
            {
              Console.WriteLine(ex);
              postResponse.IsSuccess = false;
              postResponse.ResponseMessage = "An error occurred while trying to get a post. Please check the logs for more details. Error: " + ex;
              log.LogInformation(postResponse.ResponseMessage);
            }

            return new OkObjectResult(postResponse.ResponseMessage);
        }


        private static async Task<PostResponse> GetPostFromRepository(string filePath)
        {
          var postResponse = new PostResponse();

          var (owner, repoName, branch) = ("QED-DeveloperPortal", "qed-developer-portal", "master");
          string token = System.Environment.GetEnvironmentVariable("GITHUB_TOKEN", EnvironmentVariableTarget.Process);

          var gitHubClient = new GitHubClient(new Octokit.ProductHeaderValue("DevPortal"));
          gitHubClient.Credentials = new Credentials(token);

          try
          {
            _logger.LogInformation("Checking if the file exists in the repository...");
            var res =
              await gitHubClient.Repository.Content.GetAllContents(owner, repoName, filePath);

            if (res != null)
            {
              _logger.LogInformation("File with same name exists.");

              string content = res[0].Content;
              string sha = res[0].Sha;

              //To update existing post
              /*commitMessage = $"Update commit for {_newPost.FilePath}";
              var updateChangeSet = await gitHubClient.Repository.Content.UpdateFile(owner, repoName, filePath,
               new UpdateFileRequest(commitMessage, fileContent, existingFile[0].Sha, branch));*/

              postResponse.IsSuccess = true;
              postResponse.ResponseMessage = content;
            }
            _logger.LogInformation($"** responseBody: {res}");

          }
          catch (Exception ex)
          {
            postResponse.IsSuccess = false;
            postResponse.ResponseMessage = "The file could not be retrieved. Check for error logs. Exception: " + ex.Message;

            _logger.LogInformation(" **Error occurred while getting a post" + ex );
          }
          return postResponse;
        }
     }
}
