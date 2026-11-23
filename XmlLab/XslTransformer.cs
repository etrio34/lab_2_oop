using System.Xml;
using System.Xml.Xsl;

namespace XmlLab;
public class XslTransformer
{
    public async Task TransformAsync(string xmlPath, string htmlPath)
    {

        using (Stream stream = await FileSystem.OpenAppPackageFileAsync("newspaper.xsl"))
        using (XmlReader xslReader = XmlReader.Create(stream))
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(xslReader);
            xslt.Transform(xmlPath, htmlPath);
        }
    }
}
