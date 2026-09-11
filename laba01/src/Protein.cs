namespace GeneticSearch;

public class Protein
{
    public string Name;
    public string Organism;
    public string AminoAcids;


    public Protein(string name, string organism, string aminoAcids)
    {
        Name = name;
        Organism = organism;
        AminoAcids = aminoAcids;
    }
}