using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevPortal.Models;

namespace DevPortal.Api.Helpers
{
  public static class MarkdownPostParser
  {

    public static Post GenerateMarkdownContent(Post post)
    {
      StringBuilder sb = new StringBuilder();

      post.FrontMatterContent = GenerateFrontMatter(post.Layout, post.Title, post.Author, post.Categories, post.Tags);
      post.MarkdownContent = $"{post.FrontMatterContent}{post.Body}";

      return post;
    }

    private static string GenerateFrontMatter(string layout, string title, string author, string categories, string tags)
    {
      StringBuilder fmBuilder = new StringBuilder();

      //TODO: Need to populate signed-in user
      if (String.IsNullOrEmpty(author))
        author = "chatGpt";

      if (String.IsNullOrEmpty(layout))
        fmBuilder.AppendLine($"---\n");
      else
        fmBuilder.AppendLine($"---\nlayout: {layout}");

      fmBuilder.AppendLine($"title: {title}");
      fmBuilder.AppendLine($"author: {author}");

      //fmBuilder.AppendLine($"categories: {FormatList(categories)}");
      //fmBuilder.AppendLine($"tags: {FormatList(tags)}");
      //fmBuilder.AppendLine($"date: {DateTime.Now.Date}");

      //hard-coding these values for testing purpose
      fmBuilder.AppendLine($"categories: test");
      fmBuilder.AppendLine($"tags: test");
      fmBuilder.AppendLine($"date: 1900-01-01 12:00:00 AM");

      fmBuilder.AppendLine($"---\n\n");

      return fmBuilder.ToString();
    }

    private static string FormatList(string items)
    {
      string[] list = items.Split(',');
      StringBuilder sb = new StringBuilder();
      sb.AppendLine();

      foreach (string item in list)
      {
        sb.AppendLine($"- {item.Trim()}");
      }

      return sb.ToString();
    }

  }
}
