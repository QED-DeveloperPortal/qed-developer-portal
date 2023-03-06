#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevPortal.Models
{
  public class Post
  {
    public string? Title { get; set; }
    public string? Categories { get; set; }
    public string? Author { get; set; }
    public string? Layout { get; set; }
    public string? Tags { get; set; }
    public string? Body { get; set; }

    private string? _filePath;
    public string? FilePath
    {
      get
      {
        _filePath = String.Concat($"_posts/",DateTime.Now.Year,"/", DateTime.Now.ToString("yyyy-MM-dd"), "-", this.Title, ".md");
        //_filePath = String.Concat($"_posts/1900/1900-01-01-", this.Title, ".md");
        return _filePath;
      }
    }
    public string FrontMatterContent { get; set; }
    public string MarkdownContent { get; set; }
    public DateTime Date { get; set; }
    public Post()
    {
    }
  }

  public class FrontMatter
  {
    public string? Title { get; set; }
    public string? Categories { get; set; }
    public string? Author { get; set; }
    public string? Layout { get; set; }
    public string? Tags { get; set; }
  }
  public class PostResponse
  {
    public bool IsSuccess { get; set; }
    public string? ResponseMessage { get; set; }
  }

}
