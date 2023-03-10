using System;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Graph;
using Post = DevPortal.Models.Post;
using DevPortal.Models;


namespace DevPortal.Api.Helpers
{
  public static class MarkdownPostParser
  {

    /// <summary>
    /// Get markdown content for the post
    /// </summary>
    /// <param name="post"></param>
    /// <returns></returns>
    public static Post GenerateMarkdownContent(Post post)
    {
      StringBuilder sb = new StringBuilder();

      post.FrontMatterContent = GenerateFrontMatter(post.Layout, post.Title, post.Author, post.Categories, post.Tags, post.Date);
      post.MarkdownContent = $"{post.FrontMatterContent}{post.Body}";

      if(String.IsNullOrEmpty(post.FilePath))
        post.FilePath = String.Concat($"_posts/", post.Date.Year, "/", post.Date.ToString("yyyy-MM-dd"), "-", post.Title, ".md");

      return post;
    }

    /// <summary>
    /// Parse markdown content into Post object
    /// </summary>
    /// <param name="markdownContent"></param>
    /// <returns></returns>
    public static Post ParseMarkdownContent(string content)
    {
      //  content =
      //"---\r\ntitle: \"How to add a new post in a jekyll site\"\r\nauthor: jeny\r\ncategories: [technology]\r\ntags: [how-to,getting-started,jekyll-tutorial]\r\ndate: 2023-02-19T13:08:50-04:00\r\n---\r\nJekyll is a powerful static site generator that makes it easy to create a website with a simple and easy-to-use interface. One of the most common tasks when working with Jekyll is adding new posts to your site. In this article, we will take a look at how to add a new post to your Jekyll site.\r\n\r\n### Step 1: Create a new post file\r\nThe first step to adding a new post to your Jekyll site is to create a new post file. You can do this by navigating to the ***`_posts`*** directory in your Jekyll project and creating a new file with the following format:\r\n\r\n```sql\r\nYEAR-MONTH-DAY-title.md\r\n```\r\n\r\nFor example, if you want to create a post with the title \"My First Post\", you would create a file with the following name:\r\n\r\n```\r\n2023-02-20-my-first-post.md\r\n```\r\n\r\n### Step 2: Front Matter\r\nOnce you have created your new post file, you will need to add front matter to it. Front matter is a section of YAML metadata that Jekyll uses to determine how to handle your post.\r\n\r\nAt a minimum, your front matter should include the title of your post, as well as the date and any categories or tags that you want to associate with your post. Here is an example of a basic front matter section:\r\n\r\n```yaml\r\n---\r\nlayout: post\r\ntitle: \"My First Post\"\r\ndate: 2023-02-20 12:00:00 -0500\r\ncategories: jekyll\r\ntags: [jekyll, tutorial]\r\n---\r\n```\r\n\r\nThe ***`layout`*** field specifies which layout file to use for your post, while the ***`title`***, ***`date`***, ***`categories`***, and ***`tags`*** fields provide metadata about your post.\r\n\r\n### Step 3: Add Content\r\nWith your front matter in place, you can now add the content of your post. Jekyll uses Markdown syntax to format your post, so you can include headings, lists, images, and more.\r\n\r\n### Step 4: Save and Build\r\nOnce you have added your post content, save the file and build your Jekyll site. You can do this by running the following command from the terminal:\r\n\r\n```\r\njekyll build\r\n```\r\n\r\nThis command will generate your site and output it to the ***`_site`*** directory.\r\n\r\n### Step 5: Preview your post\r\nTo preview your post, you can run the following command from the terminal:\r\n\r\n```\r\njekyll serve\r\n```\r\n\r\nThis will start a local server that you can use to view your Jekyll site. You can then navigate to your post by entering its URL in your web browser.\r\n\r\nIn conclusion, adding a new post to your Jekyll site is a simple process that involves creating a new post file, adding front matter, adding content, saving and building, and previewing your post. With these steps, you can quickly and easily create new content for your Jekyll site.\r\n\r\n";

      Post post = new Post();

      // ensure markdown contains a header component...
      if (content.IndexOf("---") == -1)
        return post;

      if (content.IndexOf("---") == content.LastIndexOf("---"))
        return post;

      // split header and markdown content - parse the header as a Json object...
      var contentParts = content.Split("---");
      StringBuilder contentBody = new StringBuilder();

      var count = contentParts.Length;
      var header = contentParts[1];

      for (int i = 2; i < count; i++)
      {
        contentBody.AppendLine(contentParts[i]);
        contentBody.AppendLine("---");
      }

      post.MarkdownContent = contentBody.ToString();
      post.Body = content;

      try
      {
        // get the Jekyll header
        header = $"{{{header[3..^3]}}}"
          .TrimStart('{')
          .TrimEnd('}');

        var headerAttributes = header.Split('\n');

        foreach (var line in headerAttributes)
        {
          if (!string.IsNullOrEmpty(line) && line != "\r")
          {
            var key = line.Substring(0, line.IndexOf(":"));
            var value = line.Substring(line.IndexOf(":") + 1).Trim();

            switch (key)
            {
              case "layout":
                post.Layout = value;
                break;

              case "title":
                post.Title = value
                  .Trim('"');
                break;

              case "date":
                post.Date = DateTime.Parse(value);
                break;

              case "categories":
                post.Categories = value
                  .TrimStart('[')
                  .TrimEnd(']');
                break;

              case "tags":
                post.Tags = value
                  .TrimStart('[')
                  .TrimEnd(']');
                break;

              case "author":
                post.Author = value;
                break;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        return post;
      }

      return post;

    }
    
    /// <summary>
    /// Generate the front matter based on the provided information
    /// </summary>
    /// <param name="layout"></param>
    /// <param name="title"></param>
    /// <param name="author"></param>
    /// <param name="categories"></param>
    /// <param name="tags"></param>
    /// <returns></returns>
    private static string GenerateFrontMatter(string layout, string title, string author, string categories, string tags, DateTime date)
    {
      StringBuilder fmBuilder = new StringBuilder();

      //TODO: Need to populate signed-in user
      if (String.IsNullOrEmpty(author))
        author = "chatGpt"; //TODO: To be replaced by logged in user

      if (String.IsNullOrEmpty(layout))
        fmBuilder.AppendLine($"---\n");
      else
        fmBuilder.AppendLine($"---\nlayout: {layout}");
      fmBuilder.AppendLine($"title: {title}");
      fmBuilder.AppendLine($"author: {author}");
      fmBuilder.AppendLine($"categories: {FormatList(categories)}");
      fmBuilder.AppendLine($"tags: {FormatList(tags)}");
      fmBuilder.AppendLine($"date: {date.ToString(date.ToString("yyyy-MM-dd HH:mm:ss zzz")) }");
      fmBuilder.AppendLine($"---\n\n");

      return fmBuilder.ToString();
    }

    private static string FormatList(string items)
    {
      string formattedList = "[" + Regex.Replace(items.TrimEnd(','), @" ", "") + "]";
      return formattedList; 
    }

  }
}
