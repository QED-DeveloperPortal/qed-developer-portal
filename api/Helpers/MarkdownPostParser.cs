using System;
using System.Text;
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
      Post post = new Post();

      // ensure markdown contains a header component...
      if (content.IndexOf("---") == -1)

      if (content.IndexOf("---") == content.LastIndexOf("---"))
        return post;

      // split header and markdown content - parse the header as a Json object...
      var header = content.Substring(0, content.LastIndexOf("---") + 3);
      post.MarkdownContent = content.Substring(content.LastIndexOf("---") + 3).TrimEnd('-');
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
    {/*
      string[] list = items.Split(',');
      StringBuilder sb = new StringBuilder();
      sb.AppendLine();

      foreach (string item in list)
      {
        sb.AppendLine($"- {item.Trim()}");
      }
      sb.ToString();*/

      string formattedList = String.Concat("[", items.TrimEnd(',').Trim(), "]");

      return formattedList; 
    }

  }
}
