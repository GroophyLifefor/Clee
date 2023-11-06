using System.IO;
using System.Xml.Serialization;

namespace Clee.CLI.Resource;

public class ProjectSettingsXML
{
    public static string Get(string name)
    {
        string xmlString;

        using (StringWriter stringWriter = new StringWriter())
        {
            new XmlSerializer(typeof(ProjectSettings)).Serialize(stringWriter, new ProjectSettings()
            {
                ProjectName = name,
                EntryFile = "main.clee"
            });

            xmlString = stringWriter.ToString();
        }

        return xmlString;
    }
}