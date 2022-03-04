using System.Xml.Serialization;

namespace XML_converter_refactored;

public class Telephone
{
    [XmlElement(ElementName = "mobile")]
    public string mobileNumber;
    [XmlElement(ElementName = "landline")]
    public string landlineNumber;
}