using System;

namespace DevPortal.Models
{
  public class Post
  {
    public string Title { get; set; }
    public string Categories { get; set; }
    public string Author { get; set; }
    public string Layout { get; set; }
    public string Tags { get; set; }
    public string Body { get; set; }

    private string _filePath;
    public string FilePath
    {
      get => _filePath = String.Concat($"_posts/", this.Date.Year, "/", this.Date.ToString("yyyy-MM-dd"), "-", this.Title, ".md");
      set => _filePath = value;
    }
    
    public string FrontMatterContent { get; set; }
    public string MarkdownContent { get; set; }
    public DateTime Date { get; set; }
  }

  public class FrontMatter
  {
    public string Title { get; set; }
    public string Categories { get; set; }
    public string Author { get; set; }
    public string Layout { get; set; }
    public string Tags { get; set; }
  }
  public class PostResponse
  {
    public bool IsSuccess { get; set; }
    public string ResponseMessage { get; set; }
  }

}
