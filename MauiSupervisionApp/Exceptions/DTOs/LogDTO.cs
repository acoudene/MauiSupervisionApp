// Changelogs Date  | Author                | Description
// 2022-05-16       | Anthony Coudène(ACE)  | Creation

using System.Text.Json.Serialization;

namespace MauiSupervisionApp.Exceptions.DTOs;

public sealed record LogDTO
{
  /// <summary>
  /// Constructor
  /// </summary>
  public LogDTO()
  {
    Id = Guid.NewGuid();
    CreationDate = DateTime.Now;
    ExceptionMessages = Enumerable
        .Empty<string>()
        .ToList();
  }

  public string? CategoryName { get; set; }

  /// <summary>
  /// catégorie de l'exception avant qu'elle soit transformée en "ServerException"
  /// </summary>
  public string? InnerCategoryName { get; set; }

  public Guid Id { get; set; }
  public string? StackTrace { get; set; }
  public string? Message { get; set; }

  public static List<string> SplitExceptionMessages(Exception? ex)
  {
    var innerExceptionMessages = new List<string>();
    var currentException = ex;
    while (currentException != null)
    {
      innerExceptionMessages.Add(currentException.GetType().Name + " : " + currentException.Message);
      currentException = currentException.InnerException;
    }
    return innerExceptionMessages;
  }

  private Exception? _exception = default;

  [JsonIgnore]
  public Exception? Exception
  {
    get
    {
      return _exception;
    }
    set
    {
      _exception = value;
      ExceptionMessages = SplitExceptionMessages(_exception);
      StackTrace = _exception?.StackTrace ?? _exception?.InnerException?.StackTrace;
      CategoryName = CategoryName ?? _exception?.GetType().Name;
    }
  }

  public List<string> ExceptionMessages { get; set; }

  public DateTime CreationDate { get; set; }
}
