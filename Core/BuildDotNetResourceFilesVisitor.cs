using System.Linq;
using System.Xml;

namespace Core {
    public class BuildDotNetResourceFilesVisitor : ILocalizationVisitor {
        private readonly string _filePath;

        public BuildDotNetResourceFilesVisitor(string filePath) {
            _filePath = filePath;
        }

        public void Visit(ILocalization localization) {

            foreach (Language language in localization.RetriveLanguages()) {

                using (var writer = XmlWriter.Create(BuildFileName(language))) {
                    writer.WriteStartElement("root");

                    writer.WriteRaw("<xsd:schema id=\"root\" xmlns=\"\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\">" +
                                    "<xsd:import namespace=\"http://www.w3.org/XML/1998/namespace\" />" +
                                    "<xsd:element name=\"root\" msdata:IsDataSet=\"true\">" +
                                    "<xsd:complexType>" +
                                    "<xsd:choice maxOccurs=\"unbounded\">" +
                                    "<xsd:element name=\"metadata\">" +
                                    "<xsd:complexType>" +
                                    "<xsd:sequence>" +
                                    "<xsd:element name=\"value\" type=\"xsd:string\" minOccurs=\"0\" />" +
                                    "</xsd:sequence>" +
                                    "<xsd:attribute name=\"name\" use=\"required\" type=\"xsd:string\" />" +
                                    "<xsd:attribute name=\"type\" type=\"xsd:string\" />" +
                                    "<xsd:attribute name=\"mimetype\" type=\"xsd:string\" />" +
                                    "<xsd:attribute ref=\"xml:space\" />" +
                                    "</xsd:complexType>" +
                                    "</xsd:element>" +
                                    "<xsd:element name=\"assembly\">" +
                                    "<xsd:complexType>" +
                                    "<xsd:attribute name=\"alias\" type=\"xsd:string\" />" +
                                    "<xsd:attribute name=\"name\" type=\"xsd:string\" />" +
                                    "</xsd:complexType>" +
                                    "</xsd:element>" +
                                    "<xsd:element name=\"data\">" +
                                    "<xsd:complexType>" +
                                    "<xsd:sequence>" +
                                    "<xsd:element name=\"value\" type=\"xsd:string\" minOccurs=\"0\" msdata:Ordinal=\"1\" />" +
                                    "<xsd:element name=\"comment\" type=\"xsd:string\" minOccurs=\"0\" msdata:Ordinal=\"2\" />" +
                                    "</xsd:sequence>" +
                                    "<xsd:attribute name=\"name\" type=\"xsd:string\" use=\"required\" msdata:Ordinal=\"1\" />" +
                                    "<xsd:attribute name=\"type\" type=\"xsd:string\" msdata:Ordinal=\"3\" />" +
                                    "<xsd:attribute name=\"mimetype\" type=\"xsd:string\" msdata:Ordinal=\"4\" />" +
                                    "<xsd:attribute ref=\"xml:space\" />" +
                                    "</xsd:complexType>" +
                                    "</xsd:element>" +
                                    "<xsd:element name=\"resheader\">" +
                                    "<xsd:complexType>" +
                                    "<xsd:sequence>" +
                                    "<xsd:element name=\"value\" type=\"xsd:string\" minOccurs=\"0\" msdata:Ordinal=\"1\" />" +
                                    "</xsd:sequence>" +
                                    "<xsd:attribute name=\"name\" type=\"xsd:string\" use=\"required\" />" +
                                    "</xsd:complexType>" +
                                    "</xsd:element>" +
                                    "</xsd:choice>" +
                                    "</xsd:complexType>" +
                                    "</xsd:element>" +
                                    "</xsd:schema>");
                    writer.WriteRaw("<resheader name=\"resmimetype\">" +
                                    "<value>text/microsoft-resx</value>" +
                                    "</resheader>" +
                                    "<resheader name=\"version\">" +
                                    "<value>2.0</value>" +
                                    "</resheader>" +
                                    "<resheader name=\"reader\">" +
                                    "<value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>" +
                                    "</resheader>" +
                                    "<resheader name=\"writer\">" +
                                    "<value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>" +
                                    "</resheader>");

                    foreach (LocalizedText localizedText in localization.RetriveTexts()
                                                                        .Where(c => c.LanguageId == language.Id)
                                                                        .ToList()) {
                        var key = localization.RetriveKey(localizedText.KeyId);
                        var area = localization.RetriveArea(key.AreaId);

                        writer.WriteRaw(string.Format("<data name=\"{0}_{1}\" xml:space=\"preserve\">",
                                                      area.Name,
                                                      key.Key));
                        
                        writer.WriteElementString("Value",
                                                  localizedText.Text);
                        writer.WriteRaw("</data>");
                    }


                    writer.WriteEndElement();
                }
            }
        }

        private string BuildFileName(Language language) {
            if (language.IsDefault)
                return _filePath;


            string[] fileName = _filePath.Split(".".ToCharArray(),
                                                2);
            return string.Format("{0}.{1}.{2}",
                                 fileName[0],
                                 language.LanguageRegion,
                                 fileName[1]);
        }
    }
}