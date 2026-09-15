using System;
using System.Collections.Generic;
using System.IO;

namespace GeneticSearch
{
    class Program
    {
        static List<Protein> ReadData(string filename)
        {
            List<Protein> data = new List<Protein>();
            RLE rle = new RLE();

            using (StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split('\t');
                    if (parts.Length < 3) continue;

                    string name = parts[0];
                    string organism = parts[1];
                    string aminoAcids = rle.Decode(parts[2]);

                    data.Add(new Protein(name, organism, aminoAcids));
                }
            }

            return data;
        }

        static void ProcessCommands(
            List<Protein> proteins,
            string filename,
            StreamWriter output,
            GeneticSearch search)
        {
            RLE rle = new RLE();

            using (StreamReader reader = new StreamReader(filename))
            {
                int number = 1;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split('\t');

                    string operationLine = line;
                    if (parts[0] == "search")
                    {
                        string decoded = rle.Decode(parts[1]);
                        operationLine = $"{parts[0]}\t{decoded}";
                    }

                    output.WriteLine($"{number:000}   {operationLine}");

                    if (parts[0] == "search")
                    {
                        search.Search(parts[1]);
                    }
                    else if (parts[0] == "diff")
                    {
                        search.Diff(parts[1], parts[2]);
                    }
                    else if (parts[0] == "mode")
                    {
                        search.Mode(parts[1]);
                    }

                    output.WriteLine(new string('-', 74));
                    number++;
                }
            }
        }

        static void Main(string[] args)
        {
            Console.Write("Выберите набор файлов (0, 1 или 2): ");
            string choice = Console.ReadLine()?.Trim();

            if (choice != "0" && choice != "1" && choice != "2")
            {
                Console.WriteLine("Неверный выбор. Допустимые значения: 0, 1, 2.");
                return;
            }

            string sequencesFile = $"sequences.{choice}.txt";
            string commandsFile = $"commands.{choice}.txt";
            string outputFile = $"genedata.{choice}.txt";

            if (!File.Exists(sequencesFile))
            {
                Console.WriteLine($"Файл {sequencesFile} не найден.");
                return;
            }
            if (!File.Exists(commandsFile))
            {
                Console.WriteLine($"Файл {commandsFile} не найден.");
                return;
            }

            List<Protein> proteins = ReadData(sequencesFile);

            using (StreamWriter output = new StreamWriter(outputFile))
            {
                output.WriteLine("Alexandra Matchenya");
                output.WriteLine("Genetic Searching");
                output.WriteLine(new string('-', 74));

                var search = new GeneticSearch(proteins, output);
                ProcessCommands(proteins, commandsFile, output, search);
            }

            Console.WriteLine($"Результат записан в файл: {outputFile}");
        }
    }
}