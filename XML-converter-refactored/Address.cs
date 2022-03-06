using System.Xml.Serialization;

namespace XML_converter_refactored;

public class Address
{
    [XmlElement(ElementName = "street")]
    public string Street;
    
    [XmlElement(ElementName = "city")]
    public string City;
    
    [XmlElement(ElementName = "zipcode")]
    public string Zipcode;
}