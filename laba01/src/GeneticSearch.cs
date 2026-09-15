using System.IO;
using System.Linq;

namespace GeneticSearch;

public class GeneticSearch
{
    private readonly List<Protein> proteins;
    private readonly StreamWriter output;
    private readonly RLE rle = new RLE();

    public GeneticSearch(List<Protein> proteins, StreamWriter output)
    {
        this.proteins = proteins;
        this.output = output;
    }

    public void Search(string text)
    {
        string sequence = rle.Decode(text);

        bool found = false;
        output.WriteLine("organism\t\t\tprotein");

        foreach (var p in proteins)
        {
            if (p.AminoAcids.Contains(sequence))
            {
                output.WriteLine($"{p.Organism}\t{p.Name}");
                found = true;
            }
        }

        if (!found)
            output.WriteLine("NOT FOUND");
    }

    public void Diff(string name1, string name2)
    {
        var p1 = proteins.FirstOrDefault(x => x.Name == name1);
        var p2 = proteins.FirstOrDefault(x => x.Name == name2);

        output.Write("amino-acids difference:  ");

        if (p1 == null || p2 == null)
        {
            output.Write("MISSING: ");
            if (p1 == null) output.Write(name1 + " ");
            if (p2 == null) output.Write(name2);
            output.WriteLine();
            return;
        }

        int diff = 0;
        int length = Math.Min(p1.AminoAcids.Length, p2.AminoAcids.Length);

        for (int i = 0; i < length; i++)
        {
            if (p1.AminoAcids[i] != p2.AminoAcids[i])
                diff++;
        }

        diff += Math.Abs(p1.AminoAcids.Length - p2.AminoAcids.Length);
        output.WriteLine(diff);
    }

    public void Mode(string name)
    {
        var protein = proteins.FirstOrDefault(x => x.Name == name);

        output.Write("amino-acid occurs:  ");

        if (protein == null)
        {
            output.WriteLine("MISSING: " + name);
            return;
        }

        var result = protein.AminoAcids
            .GroupBy(c => c)
            .Select(g => new { Acid = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => x.Acid)
            .First();

        output.WriteLine($"{result.Acid} {result.Count}");
    }
}