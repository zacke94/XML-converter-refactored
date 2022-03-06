using System.Xml.Serialization;

namespace XML_converter_refactored;

public class People
{
    [XmlElement("person", Type = typeof(Person))]
    public List<Person> Person;

    public People()
    {
        Person = new List<Person>();
    }
}