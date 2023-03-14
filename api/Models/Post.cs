#nullable enable

using System;

namespace DevPortal.Models
{
  public class Post
  {
    /// <summary>
    /// Title of the post
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Category or categories assigned to a post, separated by comma, enclosed in square brackets
    /// </summary>
    public string Categories { get; set; }

    /// <summary>
    /// Author of the post, signed-in user
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// Tag or tags assigned to a post, separated by comma, enclosed in square brackets
    /// </summary>
    public string Tags { get; set; }

    /// <summary>
    /// Content of post (md file) in markdown
    /// </summary>
    public string TotalContent { get; set; }

    /// <summary>
    /// Path of the post (md file) in GitHub repo , for example: _posts/YYYY/YYYY-MM-DD-postname.md
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// Front Matter of the post
    /// </summary>
    public string FrontMatterContent { get; set; }

    /// <summary>
    /// Content of post (md file), excluding front matter
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// Date on which the post was created (or last updated)
    /// </summary>
    private DateTime _date;
    public DateTime Date
    {
      get => (_date == DateTime.MinValue ? DateTime.Now.AddHours(-11) : _date); //to compensate UTC time
      set => _date = value;
    }
  }

  public class PostResponse
  {
    public bool IsSuccess { get; set; }
    public string ResponseMessage { get; set; }
  }

}
