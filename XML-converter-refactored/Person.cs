using System.Xml.Serialization;

namespace XML_converter_refactored;
public class Person
{
    [XmlElement(ElementName = "firstname")]
    public string firstName;
    [XmlElement(ElementName = "lastname")]
    public string lastName;
    public Telephone telephone;
    public Address address;

    [XmlElement("family", Type = typeof(Family))]
    public List<Family> family;

    public Person()
    {
        family = new List<Family>();
    }
}
