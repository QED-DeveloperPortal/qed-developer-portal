using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DevPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Octokit;
using Post = DevPortal.Models.Post;
using DevPortal.Api.Helpers;

namespace DevPortal.Api
{
    public static class UpdatePost
    {
    private static ILogger _logger;

    [FunctionName("UpdatePost")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", Route = null)] HttpRequest req,
            ILogger log)
        {
           _logger = log;
            log.LogInformation("C# HTTP trigger function processed an UpdatePost request.");

            //string name = req.Query["name"];
            //Using this for testing purpose 
            //string jsonPayload = "{\"title\":\"Testing post with non-duplicate name2\",\"categories\":\"test\",\"tags\":\"test\",\"body\":\"<p><h1>Hello Test Post!</h1> </p><p><h2>Hello Test Post Again!</h2></p>\"}";
            //byte[] byteArray = Encoding.UTF8.GetBytes(jsonPayload);
            //MemoryStream stream = new MemoryStream(byteArray);
            //req.Body = stream;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<Post>(requestBody);
            //name = name ?? data?.name;

            Post updatedPost = MarkdownPostParser.GenerateMarkdownContent(data);

             log.LogInformation("Incoming Request Body:" + req.Body);

            var postResponse = await UpdatePostInRepository(updatedPost);
              //log.LogInformation("Response after trying to get a post: ", postResponse.ResponseMessage);

            /*string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";*/

            return new OkObjectResult(postResponse.ResponseMessage);
        }

        private static async Task<PostResponse> UpdatePostInRepository(Post post)
        {
          var postResponse = new PostResponse();
          var res = new RepositoryContentChangeSet();

          var (owner, repoName, branch) = ("QED-DeveloperPortal", "qed-developerportal.github.io", "master");

          string token = System.Environment.GetEnvironmentVariable("GITHUB_TOKEN", EnvironmentVariableTarget.Process);

          var gitHubClient = new GitHubClient(new Octokit.ProductHeaderValue("DevPortal"));
          gitHubClient.Credentials = new Credentials(token);

          try
          {
            //Check if a file with same name exists
            _logger.LogInformation("Checking if file with same name exists in the repository...");
            var existingFile =
              await gitHubClient.Repository.Content.GetAllContentsByRef(owner, repoName, post.FilePath, branch);

            if (existingFile != null)
            {
              _logger.LogInformation("File with same name exists.");


              //To update existing post
              //commitMessage = $"Update commit for {post.FilePath}";
              string commitMessage = $"Updated Content for {post.FilePath}";
              var updateChangeSet = await gitHubClient.Repository.Content.UpdateFile(owner, repoName, post.FilePath,
                new UpdateFileRequest(commitMessage, post.MarkdownContent, existingFile[0].Sha, branch));

              postResponse.IsSuccess = true;
              postResponse.ResponseMessage =
                "File has been updated in the repository! The commit has is " + updateChangeSet.Commit.Sha.Substring(0, 7) + ".";
            }

            _logger.LogInformation($"** responseBody: {res}");
          }
          catch (Octokit.NotFoundException)
          {
            postResponse.IsSuccess = false;
            postResponse.ResponseMessage = "File not found. Please check if the file exists on the repository.";
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex);

            postResponse.IsSuccess = false;
            postResponse.ResponseMessage = "Error occurred while updating the file. Check logs for more details. Error: " + ex;
            _logger.LogError($"** Error occurred while updating a post: {ex}");
          }

          return postResponse;

        }
    }
}
