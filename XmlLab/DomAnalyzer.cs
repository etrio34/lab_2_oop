using System.Xml;

namespace XmlLab;

public class DomAnalyzer : IXmlAnalyzer
{
    public List<Article> Search(string path, Dictionary<string, string> criteria)
    {
        List<Article> result = new List<Article>();

        XmlDocument doc = new XmlDocument();
        doc.Load(path);

        string query = "//article";
        List<string> conditions = new List<string>();
        foreach (KeyValuePair<string, string> pair in criteria)
        {
            conditions.Add("contains(@" + pair.Key + ", " + Quote(pair.Value) + ")");
        }
        if (conditions.Count > 0)
        {
            query += "[" + string.Join(" and ", conditions) + "]";
        }

        XmlNodeList? nodes = doc.SelectNodes(query);
        if (nodes == null)
        {
            return result;
        }

        foreach (XmlElement element in nodes)
        {
            Article article = new Article();
            foreach (XmlAttribute attribute in element.Attributes)
            {
                article.Attributes[attribute.Name] = attribute.Value;
            }
            foreach (XmlElement reader in element.GetElementsByTagName("reader"))
            {
                article.Readers.Add(reader.GetAttribute("Name") + ": " + reader.GetAttribute("Review"));
            }
            result.Add(article);
        }
        return result;
    }

    public List<string> GetValues(string path, string attributeName)
    {
        List<string> values = new List<string>();

        XmlDocument doc = new XmlDocument();
        doc.Load(path);

        XmlNodeList? nodes = doc.SelectNodes("//article/@" + attributeName);
        if (nodes == null)
        {
            return values;
        }

        foreach (XmlNode node in nodes)
        {
            string? value = node.Value;
            if (value != null && !values.Contains(value))
            {
                values.Add(value);
            }
        }
        return values;
    }

    private string Quote(string value)
    {
        if (value.Contains("'"))
        {
            return "\"" + value + "\"";
        }
        return "'" + value + "'";
    }
}
