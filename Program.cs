using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;

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
                        Console.WriteLine("Help is currently in development.\n");
                        break;
                    case "/quit":
                        Console.WriteLine("Goodbye!");
                        isQuitting = true;
                        break;
                    case "/day1":
                        Console.WriteLine("Please give the input file name: ");
                        string fileName = Console.ReadLine();
                        Console.WriteLine(Day1(fileName).ToString());
                        break;  
                    default:
                        Console.WriteLine($"Unknown command: '{command}'\n");
                        break;
                }
            }
        }



        static int Day1(string fileName)
        {
            Console.WriteLine("\nCalibrating...");
            int checkSum = 0;
            var wordToDigit = new Dictionary<string, char>
            {
                {"zero", '0'},
                {"one", '1'},
                {"two", '2'},
                {"three", '3'},
                {"four", '4'},
                {"five", '5'},
                {"six", '6'},
                {"seven", '7'},
                {"eight", '8'},
                {"nine", '9'}
            };
            //Step 1: Open Filestream
            using (var fs = File.OpenRead(fileName))
            {
                using (var reader = new StreamReader(fs))
                {
                    //Step 2: Parse lines for integers (possibly separate by char);
                    //        first and last ints form a 2-digit int, if there's only one it forms a 2-digit int on its own, ignore lines with no ints;
                    //        each 2-digit int is added to a running sum called checkSum
                    while (!reader.EndOfStream)
                    {
                        string currentLine = reader.ReadLine();
                        string word = "";
                        List<char> digits = new List<char>();
                        digits.Clear();
                        for (int i = 0; i < currentLine.Length; i++)
                        {
                            if (char.IsDigit(currentLine[i]))
                            {
                                word = "";
                                digits.Add(currentLine[i]);
                            }
                            else
                            {
                                word += currentLine[i].ToString();
                                foreach (string key in wordToDigit.Keys)
                                {
                                    if (word.Length < 3)
                                        break;
                                    if (!word.Contains(key) || !foundKeys.Contains(key))
                                        continue;
                                    word = "";
                                    char value = wordToDigit[key];
                                    digits.Add(value);
                                    break;
                                }
                            }
                                
                        }

                        if (digits.Count == 0)
                            continue;
                        string numberString = digits[0].ToString() + digits[digits.Count - 1].ToString();
                        int currentNumber = int.Parse(numberString);
                        checkSum += currentNumber;
                    }
                }

            }
            //Step 3: Return the CheckSum
            return checkSum;
        }
    }
}
