namespace XmlLab;

public class AnalysisContext
{
    private IXmlAnalyzer analyzer;

    public AnalysisContext(IXmlAnalyzer analyzer)
    {
        this.analyzer = analyzer;
    }

    public void SetAnalyzer(IXmlAnalyzer analyzer)
    {
        this.analyzer = analyzer;
    }

    public List<Article> Search(string path, Dictionary<string, string> criteria)
    {
        return analyzer.Search(path, criteria);
    }

    public List<string> GetValues(string path, string attributeName)
    {
        return analyzer.GetValues(path, attributeName);
    }
}
