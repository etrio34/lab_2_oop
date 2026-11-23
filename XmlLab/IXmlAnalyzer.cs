namespace XmlLab;

public interface IXmlAnalyzer
{
    List<Article> Search(string path, Dictionary<string, string> criteria);

    List<string> GetValues(string path, string attributeName);
}
