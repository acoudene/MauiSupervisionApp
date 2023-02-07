// Changelogs Date  | Author                | Description
// 2022-05-16       | Anthony Coudène(ACE)  | Creation

using MauiSupervisionApp.Exceptions.Base;
using MauiSupervisionApp.Exceptions.Models;
using System.Runtime.Serialization;

namespace MauiSupervisionApp.Exceptions;

[Serializable]
public class PageInitializationException : LoggedExceptionBase
{
  public PageInitializationException()
  {
  }

  public PageInitializationException(LoggedExceptionBase ex)
    : this(ex.Log)
  {

  }

  public PageInitializationException(LogVO log)
    : base(log)
  {
  }

  public PageInitializationException(string message)
    : base(message)
  {
  }

  public PageInitializationException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  protected PageInitializationException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
  }
}
