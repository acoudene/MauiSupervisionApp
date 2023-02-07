// Changelogs Date  | Author                | Description
// 2022-05-16       | Anthony Coudène(ACE)  | Creation

using MauiSupervisionApp.Exceptions.Base;
using MauiSupervisionApp.Exceptions.Models;
using System.Runtime.Serialization;

namespace MauiSupervisionApp.Exceptions;

[Serializable]
public class ResponseException : LoggedExceptionBase
{
  public ResponseException()
  {
  }
  public ResponseException(LogVO log) : base(log)
  {
  }

  public ResponseException(string message) : base(message)
  {
  }

  public ResponseException(string message, Exception innerException) : base(message, innerException)
  {
  }

  protected ResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
  {
  }
}
