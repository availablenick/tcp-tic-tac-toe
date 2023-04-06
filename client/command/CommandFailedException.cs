using System;

public class CommandFailedException : Exception
{
  public CommandFailedException() {}
  public CommandFailedException(string message) : base(message) {}
  public CommandFailedException(string message, Exception inner)
    : base(message, inner) {}
}
