// Changelogs Date  | Author                | Description
// 2022-05-16       | Anthony Coudène(ACE)  | Creation

using System.Runtime.Serialization;
using MauiSupervisionApp.Exceptions.Models;

namespace MauiSupervisionApp.Exceptions.Base;

[Serializable]
public abstract class LoggedExceptionBase : Exception, ILoggedException
{
  public LogVO Log { get; }

  protected LoggedExceptionBase()
  {
    Log = new LogVO()
    {
      Exception = this
    };
  }

  protected LoggedExceptionBase(LogVO log)
  {
    Log = log;
  }

  protected LoggedExceptionBase(string message) : base(message)
  {
    Log = new LogVO()
    {
      Message = message,
      Exception = this
    };
  }

  protected LoggedExceptionBase(string message, Exception innerException) : base(message, innerException)
  {
    Log = new LogVO()
    {
      Message = message,
      Exception = this,
      InnerCategoryName = innerException?.GetType().Name
    };
  }

  protected LoggedExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context)     
  {
    Log = new LogVO()
    {
      Exception = this
    };
  }
}
