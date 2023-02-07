// Changelogs Date  | Author                | Description
// 2022-05-16       | Anthony Coudène(ACE)  | Creation

using MauiSupervisionApp.Exceptions.Models;

namespace MauiSupervisionApp.Exceptions.Base;

public interface ILoggedException
{
  LogVO Log { get; }
}