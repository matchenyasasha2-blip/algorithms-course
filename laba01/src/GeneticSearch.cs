namespace GeneticSearch;

public class GeneticSearch
{

    private List<Protein> proteins;


    public GeneticSearch(List<Protein> proteins)
    {
        this.proteins = proteins;
    }


    public void Search(string text)
    {
        foreach (var p in proteins)
        {
            if (p.AminoAcids.Contains(text))
            {
                Console.WriteLine(
                    p.Organism + " " + p.Name);
            }
        }
    }


    public void Mode(string name)
    {
        var protein =
            proteins.First(x => x.Name == name);


        var result =
            protein.AminoAcids
            .GroupBy(x => x)
            .OrderByDescending(x => x.Count())
            .First();


        Console.WriteLine(
            result.Key + " " + result.Count());
    }


    public void Diff(string first, string second)
    {
        var p1 =
            proteins.First(x => x.Name == first);

        var p2 =
            proteins.First(x => x.Name == second);


        int count = 0;


        int length =
            Math.Min(
            p1.AminoAcids.Length,
            p2.AminoAcids.Length);


        for (int i = 0; i < length; i++)
        {
            if (p1.AminoAcids[i] != p2.AminoAcids[i])
                count++;
        }


        count += Math.Abs(
            p1.AminoAcids.Length -
            p2.AminoAcids.Length);


        Console.WriteLine(count);
    }
}