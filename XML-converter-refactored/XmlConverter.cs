using System.Xml;
using System.Xml.Serialization;

namespace XML_converter_refactored;
public class XmlConverter
{
    private const int LetterIndex = 0;
    private const int PersonMaxSlots = 2;
    private const int PhoneMaxSlots = 2;
    private const int AddressMaxSlots = 3;
    private const int FamilyMaxSlots = 2;
    private const int ExcludedLetterIndex = 1;

    private const int FirstElement = 1;
    private const int SecondElement = 2;
    private const int ThirdElement = 3;
    
    private readonly Dictionary<string, string[]> _xmlSchemaDictionary = new();
    private string[]? GetXmlSchema(string elementKey)
    {
        bool hasValue = _xmlSchemaDictionary.TryGetValue(elementKey, out var xmlFormatArray);

        if (!hasValue)
        {
            Console.WriteLine("Values could not be found with the key '{0}'.", elementKey);
        }
        return xmlFormatArray;
    }
    private void WriteToXmlFile(string elementName, string elementValue, XmlWriter writer)
    {
        writer.WriteStartElement(elementName);
        if (elementValue == string.Empty)
        {
            Console.WriteLine("- Warning, empty element value for element name '{0}'", elementName);
        }
        writer.WriteString(elementValue);
        writer.WriteEndElement();
    }
    private void PrintWrongXmlSchema(string[]? xmlSchemaArray, string[]? elementNames)
    {
        if (elementNames != null && xmlSchemaArray != null)
        {
            Console.Write("- Warning, wrong XML schema for '{0}'! ", xmlSchemaArray[LetterIndex]);
            Console.WriteLine("Expected {0} elements, got {1}", elementNames.Length - 1, xmlSchemaArray.Length - 1);
        }
    }
    
    public void XmlConverterDriver(List<string[]> inputTextList, string filePath)
    {
        Console.WriteLine("Converting '{0}' to XML format...", Path.GetFileName(filePath));

        string previousLetter = "";
        string xmlFilePath = Path.GetFileNameWithoutExtension(filePath) + ".xml";
        bool firstPerson = true;

        /*XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        XmlWriter writer = XmlWriter.Create(xmlFilePath, settings);
        
        writer.WriteStartDocument();
        writer.WriteStartElement("people");*/
        People people = new People();
        XmlSerializer peopleSerializer = new XmlSerializer(people.GetType());
        //xmlSerializer.Serialize(Console.Out, people);
        
        //var path = Environment.GetFolderPath(Environment.) + "//SerializationOverview.xml";  
    
        FileStream file = File.Create(xmlFilePath);
        peopleSerializer.Serialize(file, people);

        foreach (string[] inputTextArray in inputTextList)
        {
            if (string.Equals(inputTextArray[LetterIndex], "P"))
            {
                Person person = new Person();
                person.firstName = inputTextArray[FirstElement];
                person.lastName = inputTextArray[SecondElement];
            }
        }
        file.Close();
        /*foreach (string[] inputTextArray in inputTextList)
        {
            if (string.Equals(inputTextArray[LetterIndex], "P"))
            {   
                if (firstPerson == false)
                {
                    writer.WriteEndElement();

                    if (string.Equals(previousLetter,"F")) 
                    {
                        writer.WriteEndElement();
                    }
                }
                writer.WriteStartElement("person");

                string[]? elementNames = GetXmlSchema("P");
          
                for (int i = ExcludedLetterIndex; i < PersonMaxSlots + 1; i++)
                {
                    try
                    {
                        WriteToXmlFile(elementNames[i - 1], inputTextArray[i], writer);
                    }
                    catch
                    {
                        PrintWrongXmlSchema(inputTextArray, elementNames);
                    }
                }
              
                previousLetter = inputTextArray[LetterIndex]; 
                firstPerson = false; 
            }
            else if (string.Equals(inputTextArray[LetterIndex], "T"))
            {
                writer.WriteStartElement("phone");
                string[]? elementNames = GetXmlSchema("T");

                for (int i = ExcludedLetterIndex; i < PhoneMaxSlots + 1; i++)
                {
                    try
                    {
                        WriteToXmlFile(elementNames[i - 1], inputTextArray[i], writer);
                    }
                    catch
                    {
                        PrintWrongXmlSchema(inputTextArray, elementNames);
                    }
                }
                writer.WriteEndElement();
            }
            else if (string.Equals(inputTextArray[LetterIndex], "A"))
            {
                writer.WriteStartElement("address");
                string[]? elementNames = GetXmlSchema("A");
                
                for (int i = ExcludedLetterIndex; i < AddressMaxSlots + 1; i++)
                {
                    try
                    {
                        WriteToXmlFile(elementNames[i - 1], inputTextArray[i], writer);
                    }
                    catch
                    {
                        PrintWrongXmlSchema(inputTextArray, elementNames);
                    }
                }
                writer.WriteEndElement();
            }
            else if (string.Equals(inputTextArray[0], "F"))
            {
                if (string.Equals(previousLetter,"F"))
                {
                    writer.WriteEndElement();
                }
                
                writer.WriteStartElement("family");
                string[]? elementNames = GetXmlSchema("F");
                
                for (int i = ExcludedLetterIndex; i < FamilyMaxSlots + 1; i++)
                {
                    try
                    {
                        WriteToXmlFile(elementNames[i - 1], inputTextArray[i], writer);
                    }
                    catch
                    {
                        PrintWrongXmlSchema(inputTextArray, elementNames);
                    }
                }
                
                previousLetter = inputTextArray[LetterIndex];
            }
            else
            {
                Console.WriteLine("- Warning, unknown letter '{0}'.", inputTextArray[LetterIndex]);
            }
        }
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();
        */
        Console.WriteLine("Created XML file '{0}'", xmlFilePath);
    }
    /*public void XmlConverterDriver(List<string[]> inputTextList, string filePath)
    {
        Console.WriteLine("Converting '{0}' to XML format...", Path.GetFileName(filePath));

        string previousLetter = "";
        string xmlFilePath = Path.GetFileNameWithoutExtension(filePath) + ".xml";
        bool firstPerson = true;

        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        XmlWriter writer = XmlWriter.Create(xmlFilePath, settings);
        
        writer.WriteStartDocument();
        writer.WriteStartElement("people");

        foreach (string[] inputTextArray in inputTextList)
        {
            if (string.Equals(inputTextArray[LetterIndex], "P"))
            {   
                if (firstPerson == false)
                {
                    writer.WriteEndElement();

                    if (string.Equals(previousLetter,"F")) 
                    {
                        writer.WriteEndElement();
                    }
                }
                writer.WriteStartElement("person");

                string[]? elementNames = GetXmlSchema("P");
          
                for (int i = ExcludedLetterIndex; i < PersonMaxSlots + 1; i++)
                {
                    try
                    {
                        WriteToXmlFile(elementNames[i - 1], inputTextArray[i], writer);
                    }
                    catch
                    {
                        PrintWrongXmlSchema(inputTextArray, elementNames);
                    }
                }
              
                previousLetter = inputTextArray[LetterIndex]; 
                firstPerson = false; 
            }
            else if (string.Equals(inputTextArray[LetterIndex], "T"))
            {
                writer.WriteStartElement("phone");
                string[]? elementNames = GetXmlSchema("T");

                for (int i = ExcludedLetterIndex; i < PhoneMaxSlots + 1; i++)
                {
                    try
                    {
                        WriteToXmlFile(elementNames[i - 1], inputTextArray[i], writer);
                    }
                    catch
                    {
                        PrintWrongXmlSchema(inputTextArray, elementNames);
                    }
                }
                writer.WriteEndElement();
            }
            else if (string.Equals(inputTextArray[LetterIndex], "A"))
            {
                writer.WriteStartElement("address");
                string[]? elementNames = GetXmlSchema("A");
                
                for (int i = ExcludedLetterIndex; i < AddressMaxSlots + 1; i++)
                {
                    try
                    {
                        WriteToXmlFile(elementNames[i - 1], inputTextArray[i], writer);
                    }
                    catch
                    {
                        PrintWrongXmlSchema(inputTextArray, elementNames);
                    }
                }
                writer.WriteEndElement();
            }
            else if (string.Equals(inputTextArray[0], "F"))
            {
                if (string.Equals(previousLetter,"F"))
                {
                    writer.WriteEndElement();
                }
                
                writer.WriteStartElement("family");
                string[]? elementNames = GetXmlSchema("F");
                
                for (int i = ExcludedLetterIndex; i < FamilyMaxSlots + 1; i++)
                {
                    try
                    {
                        WriteToXmlFile(elementNames[i - 1], inputTextArray[i], writer);
                    }
                    catch
                    {
                        PrintWrongXmlSchema(inputTextArray, elementNames);
                    }
                }
                
                previousLetter = inputTextArray[LetterIndex];
            }
            else
            {
                Console.WriteLine("- Warning, unknown letter '{0}'.", inputTextArray[LetterIndex]);
            }
        }
        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();
        
        Console.WriteLine("Created XML file '{0}'", xmlFilePath);
    }
    private void ReadXmlSchemaFile(string xmlSchemaFilePath)
    {
        try
        {
            string[] xmlSchemaArray = File.ReadAllLines(Path.GetFileName(xmlSchemaFilePath));

            foreach (string line in xmlSchemaArray)
            {
                string[] eachLineArray = line.Split('|');

                if (_xmlSchemaDictionary.ContainsKey(xmlSchemaArray[LetterIndex]))
                {
                    Console.WriteLine("Letter '{0}' already exists!", eachLineArray[LetterIndex]);
                }
                else
                {
                    string[] elementNames = new string[eachLineArray.Length];
                    Array.Copy(eachLineArray, ExcludedLetterIndex, elementNames, 0, eachLineArray.Length - 1);
                    
                    _xmlSchemaDictionary.Add(eachLineArray[LetterIndex], elementNames);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception caught: {0}.", e);
        }
    }*/
    public List<string[]> ReadInputFile(string filePath)
    {
        List<string[]> inputTextList = new List<string[]>();
        try
        {
            string[] inputTextArray = File.ReadAllLines(Path.GetFileName(filePath));
            
            foreach (string line in inputTextArray)
            {
                inputTextList.Add(line.Split('|'));
            }
        }
        catch (Exception e)
        {
            throw new Exception($"Exception caught: {e}");
        }
        return inputTextList;
    }
}