namespace XML_converter_refactored;
public static class MainClass
{
    private const int ProgramArgument = 0;
    public static void Main(string[] args)
    {
        if (args.Length is > 1 or < 1) 
        {
            throw new ArgumentException("Error, you entered not correct number of argument. You need to enter 1 argument.");
        }

        XmlConverter xmlConverterObject = new XmlConverter();
        xmlConverterObject.ReadXmlSchemaFile("xml-schema.txt");

        string filePath = args[ProgramArgument];
        List<string[]> textList = xmlConverterObject.ReadInputFile(filePath);
            
        xmlConverterObject.XmlConverterDriver(textList, filePath);
    } 
}