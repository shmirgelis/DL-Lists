

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DL_Lists
{
    class DLLists
    {
        static string filePathReader = FilePath(); //method to request user for correct file location input in .txt format
        static string filePathWriter = OutputPath(filePathReader); //method to format output file and output location

        static void Main(string[] args)
        {
            var dlsOwners = ReadInDLsAndOwners(); //method to read from txt file and create pairs of DLs and DL Owners
            using (StreamWriter writeDLtoCSV = new StreamWriter(filePathWriter))
                foreach (var kvp in dlsOwners)
                {
                    List<string> valuesList = kvp.Value;
                    string joinValues = string.Join(", ", valuesList);
                    writeDLtoCSV.WriteLine($"{kvp.Key}, {joinValues}");
                }
        }

        private static Dictionary<string, List<string>> ReadInDLsAndOwners()
        {
            var dlPairs = new Dictionary<string, List<string>>();
            using (StreamReader readDLfile = new StreamReader(filePathReader))
            {
                readDLfile.ReadLine();
                while (!readDLfile.EndOfStream)
                {
                    string line = readDLfile.ReadLine();
                    string[] items = line.Split('\t', ',');
                    for (int i = 1; i < items.Length; i++)
                    {
                        if (dlPairs.ContainsKey(items[i]))
                        {
                            dlPairs[items[i]].Add(items[0]);
                        }
                        else
                        {
                            var valueList = new List<string>();
                            valueList.Add(items[0]);
                            dlPairs.Add(items[i], valueList);
                        }
                    }
                }
                return dlPairs;
            }

        }

        private static string FilePath()
        {
            Console.WriteLine("Enter path to txt file");
            string filePath = Console.ReadLine();
            string fileExtension = Path.GetExtension(filePath);
            while (!File.Exists(filePath) || (fileExtension != ".txt"))
            {
                Console.WriteLine("Enter correct file path");
                filePath = Console.ReadLine();
                fileExtension = Path.GetExtension(filePath);
            }
            return filePath;

        }
        private static string OutputPath(string filePath)
        {
            string directoryPath = Path.GetDirectoryName(filePath);
            string dateTime = DateTime.Now.ToString("yyyyMMddHHmmssff");
            string reworkedFilePath = Path.Combine(directoryPath, $"DL-Owners-{dateTime}.csv");
            return reworkedFilePath;
        }
    }
}
