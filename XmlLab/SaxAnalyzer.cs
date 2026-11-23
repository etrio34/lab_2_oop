using System.Xml;

namespace XmlLab;

public class SaxAnalyzer : IXmlAnalyzer
{
    public List<Article> Search(string path, Dictionary<string, string> criteria)
    {
        List<Article> result = new List<Article>();
        Article? current = null;

        using (XmlReader reader = XmlReader.Create(path))
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "article")
                {
                    bool isEmpty = reader.IsEmptyElement;

                    current = new Article();
                    while (reader.MoveToNextAttribute())
                    {
                        current.Attributes[reader.Name] = reader.Value;
                    }

                    if (IsMatch(current, criteria))
                    {
                        result.Add(current);
                    }
                    if (isEmpty)
                    {
                        current = null;
                    }
                }
                else if (reader.NodeType == XmlNodeType.Element && reader.Name == "reader" && current != null)
                {
                    current.Readers.Add(reader.GetAttribute("Name") + ": " + reader.GetAttribute("Review"));
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "article")
                {
                    current = null;
                }
            }
        }
        return result;
    }

    public List<string> GetValues(string path, string attributeName)
    {
        List<string> values = new List<string>();

        using (XmlReader reader = XmlReader.Create(path))
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "article")
                {
                    string? value = reader.GetAttribute(attributeName);
                    if (value != null && !values.Contains(value))
                    {
                        values.Add(value);
                    }
                }
            }
        }
        return values;
    }

    private bool IsMatch(Article article, Dictionary<string, string> criteria)
    {
        foreach (KeyValuePair<string, string> pair in criteria)
        {
            if (!article.Attributes.ContainsKey(pair.Key))
            {
                return false;
            }
            if (!article.Attributes[pair.Key].Contains(pair.Value))
            {
                return false;
            }
        }
        return true;
    }
}
