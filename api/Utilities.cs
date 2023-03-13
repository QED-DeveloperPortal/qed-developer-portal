using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace DevPortal.Api
{
    public static class Utilities
    {
      //   [FunctionName("HowLongAgo")]
      //   public static IActionResult Run(
      //       [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "howlongago/{updated}")] HttpRequest req,
      //       DateTime updated,
      //       ILogger log,
      //       ClaimsPrincipal claimsPrincipal)
      //   {
      //       log.LogInformation("* Running...");
      //       log.LogInformation($"* updated: {updated}");

      //       return new OkObjectResult(HowLongAgoAsString(updated));
      //   }

        [FunctionName("Test")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("* Running...");

            return new OkObjectResult("Testing...");
        }

        private static string HowLongAgoAsString(DateTime dateTimeUtc)
      {
         TimeSpan timeSpan = DateTime.UtcNow.Subtract(dateTimeUtc);

         if (timeSpan.TotalSeconds < 30)
            return "A few seconds ago";
         if (timeSpan.TotalMinutes < 2)
            return "A few minutes ago";
         if (timeSpan.TotalMinutes < 60)
            return string.Format("{0} minutes ago", ((int)timeSpan.TotalMinutes).ToString());
         if (timeSpan.TotalMinutes < 120)
            return "About an hour ago";
         if (timeSpan.TotalHours < 22)
            return string.Format("{0} hours ago", ((int)timeSpan.TotalHours).ToString());
         if (timeSpan.TotalDays < 2)
            return "About a day ago";
         if (timeSpan.TotalDays < 7)
            return string.Format("{0} days ago", ((int)timeSpan.TotalDays).ToString());
         if (timeSpan.TotalDays < 12)
            return "About a week ago";
         if (timeSpan.TotalDays < 30)
            return "Several weeks ago";
         if (timeSpan.TotalDays < 50)
            return "About a month ago";
         if (timeSpan.TotalDays < 300)
            return "Several months ago";
         if (timeSpan.TotalDays < 380)
            return "About a year ago";
         if (timeSpan.TotalDays < 730)
            return "More than a year ago";

         var yearsAgo = Math.Floor(timeSpan.TotalDays / 365);

         return string.Format("About {0} years ago", yearsAgo.ToString());
      }
   }
}