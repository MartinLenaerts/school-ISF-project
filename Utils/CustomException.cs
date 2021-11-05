using System;
using System.Reflection.Emit;

namespace Bank.Utils
{
    public class CustomException : Exception
    {
        public Label Label { get; set; }

        public CustomException(string? message) : base(message)
        {
        }
    }
}