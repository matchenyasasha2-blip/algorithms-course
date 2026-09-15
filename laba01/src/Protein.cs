namespace GeneticSearch;

public class Protein
{
    public string Name { get; }
    public string Organism { get; }
    public string AminoAcids { get; }

    public Protein(string name, string organism, string aminoAcids)
    {
        Name = name;
        Organism = organism;
        AminoAcids = aminoAcids;
    }
}