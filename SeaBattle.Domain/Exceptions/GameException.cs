﻿namespace SeaBattle.Domain.Exceptions
{
    public class GameException : Exception
    {
        public GameException(string? message) : base(message)
        {
        }
    }
}
