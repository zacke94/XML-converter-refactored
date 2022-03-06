using System.Xml.Serialization;

namespace XML_converter_refactored;

public class Person
{
    [XmlElement(ElementName = "firstname")]
    public string FirstName;
    
    [XmlElement(ElementName = "lastname")]
    public string LastName;
    
    [XmlElement(ElementName = "telephone")]
    public Telephone Telephone;
    
    [XmlElement(ElementName = "address")]
    public Address Address;

    [XmlElement("family", Type = typeof(Family))]
    public List<Family> Family;
    
    public Person()
    {
        Family = new List<Family>();
    }
}
