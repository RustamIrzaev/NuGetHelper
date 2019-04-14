using System;

namespace NuGetHelper.Tool.Helpers
{
    public class ColoredConsole
    {
        public static void Print(string message)
        {
            Console.WriteLine(message);
        }

        public static void PrintEmptyLine()
        {
            Print("");
        }
        
        public static void PrintError(string message)
        {
            PrintInColor($"Error occurred: {message}", ConsoleColor.Red);
        }

        public static void PrintError(Exception exception)
        {
            PrintError(exception.InnerException?.Message ?? exception.Message);
        }
        
        public static void PrintSuccess(string message)
        {
            PrintInColor(message, ConsoleColor.Green);
        }

        public static void PrintGray(string message)
        {
            PrintInColor(message, ConsoleColor.Gray);
        }
        
        private static void PrintInColor(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}