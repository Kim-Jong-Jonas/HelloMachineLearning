﻿using System;
using Microsoft.ML;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace HelloMachineLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            string lengthInput = "";
            string accuracyInput;

            while (lengthInput != "q" && lengthInput != "Q")
            {
                Console.WriteLine("Enter Q to exit application");
                Console.Write("Enter desired length of generated name: ");
                lengthInput = Console.ReadLine();
                if (Int32.TryParse(lengthInput, out int num))
                {
                    Console.Write("Enter desired accuracy (recommended value is 500): ");
                    accuracyInput = Console.ReadLine();
                    Console.WriteLine();


                    if (Int32.TryParse(accuracyInput, out int acc))
                        Console.WriteLine(GenerateNewName(num, acc));
                    else
                        Console.WriteLine("No valid input");

                    Console.ReadLine();
                }
            }
        }

        private static string GenerateNewName(int lengthOfName, int accuracy)
        {

            string alphabet = "abcdefghijklmnopqrstuvwxyzåäö";
            int[,] combinationScore = new int[alphabet.Length, alphabet.Length];
            string path = AppDomain.CurrentDomain.BaseDirectory + "/SwedishFirstNames";
            var random = new Random();
            string newName = "";

            string[] names = File.ReadAllLines(path);


            foreach (string name in names)
            {
                for (int i = 1; i < name.Length; i++)
                {
                    if (alphabet.Contains(name[i - 1]) && alphabet.Contains(name[i]))
                        combinationScore[alphabet.IndexOf(name[i - 1]), alphabet.IndexOf(name[i])]++;
                }
            }

            newName += alphabet[random.Next(0, alphabet.Length)];

            int start = Environment.TickCount;

            for (int i = 0; i < lengthOfName - 1; i++)
            {

                char temp = alphabet[random.Next(0, alphabet.Length)];

                if (combinationScore[alphabet.IndexOf(newName[i]), alphabet.IndexOf(temp)] > accuracy)
                    newName += temp;

                else
                    i--;

                if (Environment.TickCount - start > 1000)
                    break;
            }

            if (Environment.TickCount - start > 1000)
                return GenerateNewName(lengthOfName, accuracy);

            for (int i = 0; i < newName.Length - 2; i++)
            {
                if (newName[i] == newName[i + 1] && newName[i] == newName[i + 2])
                {
                    return GenerateNewName(lengthOfName, accuracy);
                }
            }

            return newName;     //Capitalize before return?
        }
    }
}
