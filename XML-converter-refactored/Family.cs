using System.Xml.Serialization;

namespace XML_converter_refactored;

public class Family
{
    [XmlElement(ElementName = "name")]
    public string Name;
    
    [XmlElement(ElementName = "year")]
    public string Year;
    
    [XmlElement(ElementName = "telephone")]
    public Telephone Telephone;
    
    [XmlElement(ElementName = "address")]
    public Address Address;
}