using System.Xml.Serialization;

namespace XML_converter_refactored;

public class Telephone
{
    [XmlElement(ElementName = "mobile")]
    public string MobileNumber;
    
    [XmlElement(ElementName = "landline")]
    public string LandlineNumber;
}