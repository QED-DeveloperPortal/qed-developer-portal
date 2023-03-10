using System;
using System.IO;
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
    public static class AddNewPost
    {
        private static Post _newPost = new Post();
        private static ILogger _logger;

        [FunctionName("AddNewPost")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "put", Route = null)] HttpRequest req, ILogger log)
        {
           _logger = log;
            log.LogInformation("C# HTTP trigger function to add new post invoked.");

            //Using this for testing purpose
            //string jsonPayload = "{\"title\":\"Hello TUI 1\",\"categories\":\"test\",\"tags\":\"toast\",\"body\":\"## Test Header 2 \"}";
            //byte[] byteArray = Encoding.UTF8.GetBytes(jsonPayload);
            //MemoryStream stream = new MemoryStream(byteArray);
            //req.Body = stream;

            log.LogInformation("Incoming Request Body:" + req.Body);
            dynamic data = JsonConvert.DeserializeObject<Post>(await new StreamReader(req.Body).ReadToEndAsync());

            //TODO: To be removed  when the user can choose date on UI
            data.Date = DateTime.Now; 

            //Generate content for markdown file for the new post
            _newPost = MarkdownPostParser.GenerateMarkdownContent(data);

            log.LogInformation("Total content of new post:" + _newPost.TotalContent);

            //Call GitHub API function to upload/add this file to the github repo branch
            var postResponse = await AddNewPostToRepository();
            log.LogInformation("Response after trying to add new post: ", postResponse.ResponseMessage);

            //Todo: Call GitHub API function to create a new pull request            
            
            return new OkObjectResult(postResponse);
        }

    
        private static async Task<PostResponse> AddNewPostToRepository()
        {
          var postResponse = new PostResponse();
          var res = new RepositoryContentChangeSet();

          string token = System.Environment.GetEnvironmentVariable("GITHUB_TOKEN", EnvironmentVariableTarget.Process);
          string repoName = System.Environment.GetEnvironmentVariable("GITHUB_REPO", EnvironmentVariableTarget.Process);
          string branch = System.Environment.GetEnvironmentVariable("GITHUB_BRANCH", EnvironmentVariableTarget.Process);
          string owner = System.Environment.GetEnvironmentVariable("GITHUB_OWNER", EnvironmentVariableTarget.Process);

          var gitHubClient = new GitHubClient(new Octokit.ProductHeaderValue("DevPortal"));
          gitHubClient.Credentials = new Credentials(token);

          string fileContent = _newPost.TotalContent;
          string filePath = _newPost.FilePath;
          string commitMessage = $"First commit for { filePath }";

          try
          {
            //Check if a file with same name exists
              _logger.LogInformation("Checking if file with same name exists in the repository...");
              var existingFile =
                await gitHubClient.Repository.Content.GetAllContentsByRef(owner, repoName, filePath, branch);

              if (existingFile != null)
              {
                _logger.LogInformation("File with same name exists.");

                postResponse.IsSuccess = false;
                postResponse.ResponseMessage =
                  "A new post could not be added. A post with same name already exists!";
              }
              _logger.LogInformation($"** responseBody: {existingFile}");

          }
          catch (Octokit.NotFoundException)
          {
            _logger.LogInformation("Can't find a file with same name in the repository.");
            _logger.LogInformation("Adding a new file...");

            res = await gitHubClient.Repository.Content.CreateFile(owner, repoName, filePath,
              new CreateFileRequest(commitMessage, fileContent, branch));

            if (res != null && res.Commit != null)
            {
              postResponse.IsSuccess = true;
              postResponse.ResponseMessage =
                "A new post has been created successfully! The commit hash is " + res.Commit.Sha.Substring(0, 7) + ".";
              _logger.LogInformation(postResponse.ResponseMessage);
            }
          }
          catch (Exception ex)
          {
            postResponse.IsSuccess = false;
            postResponse.ResponseMessage =
              "An error occurred while adding the new post. Please check the logs for more details." + ex;

            _logger.LogInformation($"** Error occurred while adding a new post: {ex}");
          }

          return postResponse;
        }
    }
}
