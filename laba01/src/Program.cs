using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeneticSearch
{
    class Program
    {
        struct Protein
        {
            public string name;
            public string organism;
            public string amino_acids;
        }


        static List<Protein> ReadData(string filename)
        {
            List<Protein> data = new List<Protein>();

            StreamReader reader = new StreamReader(filename);

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                string[] parts = line.Split('\t');

                Protein p;

                p.name = parts[0];
                p.organism = parts[1];
                p.amino_acids = RLDecoding(parts[2]);

                data.Add(p);
            }

            reader.Close();

            return data;
        }


        static string RLDecoding(string sequence)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < sequence.Length; i++)
            {
                if (char.IsDigit(sequence[i]))
                {
                    int count = sequence[i] - '0';
                    char letter = sequence[i + 1];

                    for (int j = 0; j < count; j++)
                        result.Append(letter);

                    i++;
                }
                else
                {
                    result.Append(sequence[i]);
                }
            }

            return result.ToString();
        }


        static string RLEncoding(string sequence)
        {
            StringBuilder result = new StringBuilder();

            int count = 1;

            for (int i = 0; i < sequence.Length; i++)
            {
                if (i + 1 < sequence.Length &&
                    sequence[i] == sequence[i + 1])
                {
                    count++;
                }
                else
                {
                    if (count > 2)
                        result.Append(count);

                    result.Append(sequence[i]);

                    count = 1;
                }
            }

            return result.ToString();
        }


        static void Search(List<Protein> proteins, string seq, StreamWriter output)
        {
            seq = RLDecoding(seq);

            bool found = false;

            output.WriteLine("organism\t\t\tprotein");

            foreach (Protein p in proteins)
            {
                if (p.amino_acids.Contains(seq))
                {
                    found = true;

                    output.WriteLine(
                        p.organism + "\t\t" + p.name);
                }
            }


            if (!found)
                output.WriteLine("NOT FOUND");
        }


        static Protein? FindProtein(List<Protein> proteins, string name)
        {
            foreach (Protein p in proteins)
            {
                if (p.name == name)
                    return p;
            }

            return null;
        }


        static void Diff(List<Protein> proteins,
                         string name1,
                         string name2,
                         StreamWriter output)
        {
            Protein? p1 = FindProtein(proteins, name1);
            Protein? p2 = FindProtein(proteins, name2);


            if (p1 == null || p2 == null)
            {
                output.Write("MISSING: ");

                if (p1 == null)
                    output.Write(name1 + " ");

                if (p2 == null)
                    output.Write(name2);

                output.WriteLine();

                return;
            }


            int diff = 0;

            int length = Math.Min(
                p1.Value.amino_acids.Length,
                p2.Value.amino_acids.Length);


            for (int i = 0; i < length; i++)
            {
                if (p1.Value.amino_acids[i] !=
                    p2.Value.amino_acids[i])
                {
                    diff++;
                }
            }


            diff += Math.Abs(
                p1.Value.amino_acids.Length -
                p2.Value.amino_acids.Length);


            output.WriteLine(diff);
        }



        static void Mode(List<Protein> proteins,
                         string name,
                         StreamWriter output)
        {
            Protein? protein = FindProtein(proteins, name);


            if (protein == null)
            {
                output.WriteLine("MISSING: " + name);
                return;
            }


            int[] count = new int[26];


            foreach (char c in protein.Value.amino_acids)
            {
                count[c - 'A']++;
            }


            int max = 0;
            char answer = 'Z';


            for (char c = 'A'; c <= 'Z'; c++)
            {
                int value = count[c - 'A'];

                if (value > max)
                {
                    max = value;
                    answer = c;
                }
            }


            output.WriteLine(answer + "\t" + max);
        }



        static void ProcessCommands(
            List<Protein> proteins,
            string filename,
            StreamWriter output)
        {
            StreamReader reader =
                new StreamReader(filename);


            int number = 1;


            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                string[] parts =
                    line.Split('\t');


                output.WriteLine(
                    number.ToString("000")
                    + "   " + line);


                if (parts[0] == "search")
                {
                    Search(
                        proteins,
                        parts[1],
                        output);
                }


                if (parts[0] == "diff")
                {
                    Diff(
                        proteins,
                        parts[1],
                        parts[2],
                        output);
                }


                if (parts[0] == "mode")
                {
                    Mode(
                        proteins,
                        parts[1],
                        output);
                }


                output.WriteLine(
                    "--------------------------------------------------------------------------");


                number++;
            }


            reader.Close();
        }



        static void Main(string[] args)
        {
            List<Protein> proteins =
                ReadData("sequences.txt");


            StreamWriter output =
                new StreamWriter("genedata.txt");


            output.WriteLine("Alexandra");
            output.WriteLine("Genetic Searching");
            output.WriteLine(
            "--------------------------------------------------------------------------");


            ProcessCommands(
                proteins,
                "commands.txt",
                output);


            output.Close();
        }
    }
}
