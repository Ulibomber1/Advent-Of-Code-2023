using System;
using System.Collections.Generic;
using System.Drawing;
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
            List<string> commandArgs;
            Console.WriteLine("Welcome to the Advent of Code 2023 CLI!\n\n");
            bool isQuitting = false;
            while (!isQuitting)
            {
                command = Console.ReadLine();
                commandArgs = SplitString(command);
                command = commandArgs[0];
                commandArgs.RemoveAt(0);

                Console.WriteLine($"commandArgs: {commandArgs.Count}");
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
                        int[] resultD2 = Day2(commandArgs[0]);
                        Console.WriteLine($"\nSum of possible game IDs: {resultD2[0]}\nSum of games' cube powers: {resultD2[1]}\n");
                        break;
                    case "/day3":
                        Console.WriteLine($"\nFinding part numbers in {commandArgs[0]}...");
                        int resultD3 = Day3(commandArgs[0]);
                        Console.WriteLine($"\nSum of part numbers: {resultD3}");
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

        static int[] Day2(string filename)
        {
            int checkSum = 0;
            int powerSum = 0;
            Dictionary<string, int> possibleColorsMax = new Dictionary<string, int>{ { "red", 12 }, { "green", 13 }, { "blue", 14 } };

            // open file stream and start reader, start a while loop.
            using (var fs = File.OpenRead(filename))
            using (var reader = new StreamReader(fs))
                while (!reader.EndOfStream)
                {
                    // Split line into game ID and data, then split the ID from the word 'game' as {ID : list of subsets}
                    string currentLine = reader.ReadLine();
                    List<string> idDataSplit = SplitString(currentLine, ':');
                    int gameID = int.Parse(SplitString(idDataSplit[0])[1]);
                    int minGreen, minRed, minBlue;
                    minGreen = minRed = minBlue = int.MinValue;
                    List<string> dataSplit = SplitString(idDataSplit[1], ';');

                    // Split the data into subsets, and each subset is a dictionary with {color : # of color}
                    List<Dictionary<string, int>> listOfSets = new();
                    for (int i = 0; i < dataSplit.Count; i++)
                    {
                        List<string> currentSubset = SplitString(dataSplit[i].Trim(), ',');
                        Dictionary<string, int> currentSubsetDict = new();
                        for (int j  = 0; j < currentSubset.Count; j++)
                        {
                            List<string> splitSubset = SplitString(currentSubset[j].Trim());
                            string key = splitSubset[1];
                            int value = int.Parse(splitSubset[0]);
                            currentSubsetDict.Add(key, value);
                        }
                        listOfSets.Add(currentSubsetDict);
                    }

                    // Test if any subset contains more than the known maximum # of each color
                    bool possibleGame = true;
                    foreach (var dict in listOfSets)
                    {
                        foreach (var color in possibleColorsMax.Keys)
                        {
                            if (!dict.ContainsKey(color))
                                continue;
                            switch (color)
                            {
                                case "red":
                                    if (dict[color] > minRed)
                                        minRed = dict[color];
                                    break;
                                case "green":
                                    if (dict[color] > minGreen)
                                        minGreen = dict[color];
                                    break;
                                case "blue":
                                    if (dict[color] > minBlue)
                                        minBlue = dict[color];
                                    break;
                            }
                            if (dict[color] > possibleColorsMax[color])
                                possibleGame = false;
                        }
                    }

                    // If the subsets pass: add associated game ID to the check sum, print out that Game {ID} is possible 
                    if (possibleGame)
                    {
                        Console.WriteLine($"\nCURRENT LINE: {currentLine}\nGame {gameID} is possible!");
                        checkSum += gameID;
                    }

                    // Add the power of the current game's cubes to the power sum
                    if (minBlue == int.MinValue) minBlue = 1;
                    if (minGreen == int.MinValue) minGreen = 1;
                    if (minRed == int.MinValue) minRed = 1;
                    int cubePower = minRed * minGreen * minBlue;
                    powerSum += cubePower;
                }

            int[] sumList = { checkSum, powerSum };
            return sumList;
        }

        static int Day3(string filename)
        {
            int checkSum = 0;

            int rowLength = 0;
            char sep = '.';
            List<string> rows = new();
            List<char[]> charMatrix = new();
            List<int> partNums = new();

            using (var fs = File.OpenRead(filename))
            using (var reader = new StreamReader(fs))
                while (!reader.EndOfStream)
                {
                    rows.Add(reader.ReadLine());
                }

            rowLength = rows[0].Length;
            foreach (string row in rows)
            {
                char[] currentRow = row.ToCharArray();
                charMatrix.Add(currentRow);
            }
            
            for (int i = 0; i < charMatrix.Count - 1; i++)
            {
                string numString = "";
                for (int j = 0; j < rowLength - 1; j++)
                {
                    
                    if (char.IsDigit(charMatrix[i][j]))
                    {
                        numString += charMatrix[i][j];
                        continue;
                    }
                    else if (charMatrix[i][j] == sep)
                    {

                        numString = "";
                        continue;
                    }
                    else
                    {

                    }

                }
            }

            return checkSum;
        }

        static private bool IsPartNumber(List<char[]> matrix, int i, int j)
        {
            bool isTrue = false;
            List<Tuple<int, int>> directions = new({0,1});

            return isTrue;
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

            for (int index = 0; index < line.Length; index++)
            {
                if (line[index] != separator)
                    currentString += line[index].ToString();
                if (line[index] == separator || index == line.Length - 1)
                {
                    strings.Add(currentString);
                    currentString = "";
                }    
            }

            return strings;
        }
    }
}
