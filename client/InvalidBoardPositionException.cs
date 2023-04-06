using System;

public class InvalidBoardPositionException : Exception
{
  public InvalidBoardPositionException() {}
  public InvalidBoardPositionException(string message) : base(message) {}
  public InvalidBoardPositionException(string message, Exception inner)
    : base(message, inner) {}
}
