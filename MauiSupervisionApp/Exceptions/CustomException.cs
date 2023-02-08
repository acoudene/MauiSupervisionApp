// Changelogs Date  | Author                | Description
// 2022-05-16       | Anthony Coudène(ACE)  | Creation

using MauiSupervisionApp.Exceptions.Base;
using MauiSupervisionApp.Exceptions.Models;
using System.Runtime.Serialization;

namespace MauiSupervisionApp.Exceptions;

[Serializable]
public class CustomException : LoggedExceptionBase
{
  public CustomException()
  {
  }

  public CustomException(LoggedExceptionBase ex)
    : this(ex.Log)
  {

  }

  public CustomException(LogVO log)
    : base(log)
  {
  }

  public CustomException(string message)
    : base(message)
  {
  }

  public CustomException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  protected CustomException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
