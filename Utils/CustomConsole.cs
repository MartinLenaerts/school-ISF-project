using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Microsoft.VisualBasic.FileIO;

namespace Bank.Utils
{
    public class CustomConsole
    {
        
        
        public static void Print(string msg, bool line = true)
        {
            if(line)Console.WriteLine(msg);
            else Console.Write(msg);
        }
        
        public static void PrintSuccess(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        public static void PrintError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }


        public static void PrintChoice(Choice choice)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(choice.Key);
            Console.ResetColor();
            Console.WriteLine(" : " + choice.Message);
        }

        public static void PrintAllChoices(List<Choice> choices)
        {
            foreach (Choice choice in choices)
            {
                PrintChoice(choice);
            }
        }

        public static void PrintInfo(string msg, bool line = true)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            if (line) Console.WriteLine(msg);
            else Console.Write(msg);
            Console.ResetColor();
        }
        
        
        public static string EnterPassword()
        {
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            return pass;
        }
    }
}