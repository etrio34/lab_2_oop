namespace XmlLab;

public class Article
{
    public Dictionary<string, string> Attributes = new Dictionary<string, string>();

    public List<string> Readers = new List<string>();

    public override string ToString()
    {
        string text = "";
        foreach (KeyValuePair<string, string> pair in Attributes)
        {
            text += pair.Key + ": " + pair.Value + "\n";
        }
        foreach (string reader in Readers)
        {
            text += "Читач - " + reader + "\n";
        }
        return text;
    }
}
