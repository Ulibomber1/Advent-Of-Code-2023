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
            List<string> commandArgs = new();
            Console.WriteLine("Welcome to the Advent of Code 2023 CLI!\n\n");
            bool isQuitting = false;
            while (!isQuitting)
            {
                command = Console.ReadLine();
                commandArgs = SplitString(command);
                command = commandArgs[0];
                commandArgs.RemoveAt(0);

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
                        Console.WriteLine($"\nCalibrating with file {commandArgs[0]}...");
                        Console.WriteLine($"\nCalibration value is: {Day1(commandArgs[0])}\n");
                        break;
                    case "/day2":
                        Console.WriteLine($"\nFinding possible games in {commandArgs[0]}...");
                        Console.WriteLine($"\nSum of possible game IDs: {Day2(commandArgs[0])}\n");
                        break;
                    default:
                        Console.WriteLine($"Unknown command: '{command}'\n");
                        break;
                }

                commandArgs.Clear();
            }
        }



        static int Day1(string fileName)
        {
            int checkSum = 0;

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
                        List<char> digits = FindAllDigits(currentLine);
                        if (digits.Count == 0)
                            continue;

                        string numberString = digits[0].ToString() + digits[digits.Count - 1].ToString();
                        int currentNumber = int.Parse(numberString);
                        Console.WriteLine($"Line: {currentLine}\nNumber String: {numberString}\nInteger: {currentNumber}\n");
                        checkSum += currentNumber;
                        digits.Clear();
                    }
                }
            }
            //Step 3: Return the CheckSum
            return checkSum;
        }

        static int Day2(string filename)
        {
            int checkSum = 0;
            // open file stream and start reader, start a while loop.
            using (var fs = File.OpenRead(filename))
            using (var reader = new StreamReader(fs))
                while (!reader.EndOfStream)
                {
                    string currentLine = reader.ReadLine();
                    List<string> idDataSplit = SplitString(currentLine, ':');
                    int gameID = int.Parse(SplitString(idDataSplit[0])[1]);
                    List<string> dataSplit = SplitString(idDataSplit[1], ';');

                    Console.WriteLine($"\ncurrentLine: {currentLine}\nid: {gameID}\ndataSplit: {dataSplit}");

                    List<Dictionary<string, int>> listOfSets;
                    for (int i = 0; i < dataSplit.Count - 1; i++)
                    {
                        List<string> currentSubset = SplitString(dataSplit[i]);
                        //string key 
                    }
                    
                }
                // split line into game ID and data, then split the ID from the word 'game' as {ID : list of subsets}
                // Split the data into subsets, and each subset is a dictionary with {color : # of color}
                // Test if any subset contains more than the known maximum # of each color,
                // If any subset has more of any color, continue while loop (impossible game)
                // If the subsets pass: add associated game ID to the check sum, print out that Game {ID} is possible 
            return checkSum;
        }

        static private List<char> FindAllDigits(string currentLine)
        {
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
            string word = "";
            List<char> digits = new List<char>();

            //Check for first occurence of a digit or digit word forwards and backwards
            for (int i = 0; i < currentLine.Length; i++)
            {
                if (char.IsDigit(currentLine[i]))
                {
                    word = "";
                    digits.Add(currentLine[i]);
                    break;
                }
                else
                {
                    bool wordFound = false;
                    word += currentLine[i].ToString();
                    foreach (string key in wordToDigit.Keys)
                    {
                        if (!word.Contains(key))
                            continue;
                        wordFound = true;
                        char value = wordToDigit[key];
                        digits.Add(value);
                        break;
                    }
                    if (wordFound)
                        break;
                }
            }
            word = "";
            for (int i = currentLine.Length - 1; i > 0; i--)
            {
                if (char.IsDigit(currentLine[i]))
                {
                    word = "";
                    digits.Add(currentLine[i]);
                    break;
                }
                else
                {
                    bool wordFound = false;
                    word = currentLine[i].ToString() + word;
                    foreach (string key in wordToDigit.Keys)
                    {
                        if (!word.Contains(key))
                            continue;
                        wordFound = true;
                        char value = wordToDigit[key];
                        digits.Add(value);
                        break;
                    }
                    if (wordFound)
                        break;
                }
            }

            return digits;
        }

        static private List<string> SplitString(string line, char separator = ' ')
        {
            List<string> strings = new();
            string currentString = "";

            foreach (char character in line)
            {
                if (character != separator)
                    currentString += character.ToString();
                else
                {
                    strings.Add(currentString);
                    currentString = "";
                } 
            }

            return strings;
        }
    }
}
