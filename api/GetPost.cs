using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DevPortal.Api.Helpers;
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
            Post post = new Post();

            try

            {
              if (req != null && !String.IsNullOrEmpty(req.Query["filePath"]))
              {
                log.LogInformation("Incoming Request Body:" + req.Body);
                filePath = req.Query["filePath"];
              }

              //filePath = "_posts/2023/2023-03-14-Hello TUI 1.md";

              postResponse = await GetPostFromRepository(filePath);

              if (postResponse.IsSuccess)
              {
                post = MarkdownPostParser.ParseMarkdownContent(postResponse.ResponseMessage);
              }

              Console.WriteLine(post);
              log.LogInformation("Response after calling GetPost: ", postResponse.ResponseMessage);
            }
            catch (Exception ex)
            {
              Console.WriteLine(ex);
              postResponse.IsSuccess = false;
              postResponse.ResponseMessage = "An error occurred while trying to get a post. Please check the logs for more details. Error: " + ex;
              log.LogInformation(postResponse.ResponseMessage);
            }

            return new OkObjectResult(post);
        }


        private static async Task<PostResponse> GetPostFromRepository(string filePath)
        {
          var postResponse = new PostResponse();

          string token = System.Environment.GetEnvironmentVariable("GITHUB_TOKEN", EnvironmentVariableTarget.Process);
          string repoName = System.Environment.GetEnvironmentVariable("GITHUB_REPO", EnvironmentVariableTarget.Process);
          string owner = System.Environment.GetEnvironmentVariable("GITHUB_OWNER", EnvironmentVariableTarget.Process);

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
