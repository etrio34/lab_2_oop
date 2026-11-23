using System.Xml.Linq;

namespace XmlLab;

public class LinqAnalyzer : IXmlAnalyzer
{
    public List<Article> Search(string path, Dictionary<string, string> criteria)
    {
        List<Article> result = new List<Article>();

        XDocument doc = XDocument.Load(path);
        IEnumerable<XElement> articles = doc.Descendants("article");

        foreach (KeyValuePair<string, string> pair in criteria)
        {
            string key = pair.Key;
            string value = pair.Value;
            articles = articles.Where(x => ((string?)x.Attribute(key) ?? "").Contains(value));
        }

        foreach (XElement element in articles)
        {
            Article article = new Article();
            foreach (XAttribute attribute in element.Attributes())
            {
                article.Attributes[attribute.Name.LocalName] = attribute.Value;
            }
            foreach (XElement reader in element.Elements("reader"))
            {
                article.Readers.Add(reader.Attribute("Name")?.Value + ": " + reader.Attribute("Review")?.Value);
            }
            result.Add(article);
        }
        return result;
    }

    public List<string> GetValues(string path, string attributeName)
    {
        XDocument doc = XDocument.Load(path);

        return doc.Descendants("article")
                  .Attributes(attributeName)
                  .Select(a => a.Value)
                  .Distinct()
                  .ToList();
    }
}
