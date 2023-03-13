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

            //Using this for testing purpose 
            //string jsonPayload = "{\"title\":\"Testing markdown 2\",\"categories\":\"test\",\"author\":\"chatGpt\",\"layout\":null,\"tags\":\"test\",\"body\":\"---\\n\\r\\ntitle: Testing markdown 2\\r\\nauthor: chatGpt\\r\\ncategories: [test]\\r\\ntags: [test]\\r\\ndate: 2023-03-13 20:58:27 +10:00\\r\\n---\\n\\n\\r\\n## Test Header 2\",\"filePath\":null,\"frontMatterContent\":null,\"markdownContent\":\"\\n\\n\\r\\n## Test Header 2\\r\\n\",\"date\":\"2023-03-13T20:58:27+10:00\"}";
            //byte[] byteArray = Encoding.UTF8.GetBytes(jsonPayload);
            //MemoryStream stream = new MemoryStream(byteArray);
            //req.Body = stream;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject<Post>(requestBody);

            Post updatedPost = MarkdownPostParser.GenerateMarkdownContent(data);

            log.LogInformation("Incoming Request Body:" + req.Body);

            var postResponse = await UpdatePostInRepository(updatedPost);
            log.LogInformation("Response after calling UpdatePost: ", postResponse.ResponseMessage);

            return new OkObjectResult(postResponse);
        }

        private static async Task<PostResponse> UpdatePostInRepository(Post post)
        {
          var postResponse = new PostResponse();
          var res = new RepositoryContentChangeSet();

          string token = System.Environment.GetEnvironmentVariable("GITHUB_TOKEN", EnvironmentVariableTarget.Process);
          string repoName = System.Environment.GetEnvironmentVariable("GITHUB_REPO", EnvironmentVariableTarget.Process);
          string branch = System.Environment.GetEnvironmentVariable("GITHUB_BRANCH", EnvironmentVariableTarget.Process);
          string owner = System.Environment.GetEnvironmentVariable("GITHUB_OWNER", EnvironmentVariableTarget.Process);

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
                string commitMessage = $"Updated Content for {post.FilePath}";
                res = 
                  await gitHubClient.Repository.Content.UpdateFile(owner, repoName, post.FilePath,
                  new UpdateFileRequest(commitMessage, post.MarkdownContent, existingFile[0].Sha, branch));

                postResponse.IsSuccess = true;
                postResponse.ResponseMessage =
                  "File has been updated in the repository! The commit hash is " + res.Commit.Sha.Substring(0, 7) + ".";

                _logger.LogInformation($"** " + postResponse.ResponseMessage);
              }

            _logger.LogInformation($"** responseBody: {res}");
          }
          catch (Octokit.NotFoundException)
          {
            postResponse.IsSuccess = false;
            postResponse.ResponseMessage = "File not found. Please check if the file exists on the repository.";
            _logger.LogInformation($"** File not found in the repo: {res}");
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
