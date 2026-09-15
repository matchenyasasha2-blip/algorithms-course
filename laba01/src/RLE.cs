using System.Text;

namespace GeneticSearch;

public class RLE
{
    public string Decode(string text)
    {
        StringBuilder result = new();

        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsDigit(text[i]))
            {
                int count = text[i] - '0';
                char letter = text[i + 1];

                for (int j = 0; j < count; j++)
                    result.Append(letter);

                i++;
            }
            else
            {
                result.Append(text[i]);
            }
        }

        return result.ToString();
    }

    public string Encode(string text)
    {
        StringBuilder result = new();
        int count = 1;

        for (int i = 0; i < text.Length; i++)
        {
            if (i + 1 < text.Length && text[i] == text[i + 1])
            {
                count++;
            }
            else
            {
                if (count > 2)
                    result.Append(count);

                result.Append(text[i]);
                count = 1;
            }
        }

        return result.ToString();
    }
}