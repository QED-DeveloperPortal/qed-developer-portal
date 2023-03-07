#nullable enable

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

    public string FilePath { get; set; }
    
    public string FrontMatterContent { get; set; }
    public string MarkdownContent { get; set; }

    private DateTime _date;
    public DateTime Date
    {
      get => (_date == DateTime.MinValue ? DateTime.Now : _date);
      set => _date = value;
    }
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
