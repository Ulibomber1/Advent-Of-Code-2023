using System;

namespace Advent_Of_Code_2023
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            string command;
            Console.WriteLine("Welcome to the Advent of Code 2023 CLI!\n\n");
            bool isQuitting = false;
            while (!isQuitting)
            {
                command = Console.ReadLine();
                switch (command)
                {
                    case "/help":
                        Console.WriteLine("Help is currently in development.");
                        break;
                    case "/quit":
                        Console.WriteLine("Goodbye!");
                        isQuitting = true;
                        break;
                    default:
                        Console.WriteLine("Unknown command " + command);
                        break;
                }
            }
        }
    }
}
