using System.Xml;

namespace XML_converter_refactored {
    public class XmlConverter {
        private const int ProgramArgument = 0;
        private const int LetterIndex = 0;
        private const int PersonMaxSlots = 2;
        private const int PhoneMaxSlots = 2;
        private const int AddressMaxSlots = 3;
        private const int FamilyMaxSlots = 2;
        private const int ExcludedLetterIndex = 1;
        
        private const string TextFormatFileName = "textFormat.txt";
        
        private static readonly Dictionary<string, string[]> textFormatDictionary = new Dictionary<string, string[]>();
        private static List<string[]> ReadInputFile(string filePath)
        {
            List<string[]> textList = new List<string[]>();
            try
            {
                string[] textArray = File.ReadAllLines(Path.GetFileName(filePath));
                
                foreach (string line in textArray)
                {
                    textList.Add(line.Split('|'));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught: {0}", e);
            }
            return textList;
        }

        private static string[]? GetTextFormats(string letterKey)
        {
            bool hasValue = textFormatDictionary.TryGetValue(letterKey, out var textFormatArray);

            if (!hasValue)
            {
                Console.WriteLine("Values could not be found with the key '{0}'.", letterKey);
            }
            return textFormatArray;
        }

        private static void WriteToXmlFile(string elementName, string elementValue, XmlWriter writer)
        {
            writer.WriteStartElement(elementName);
            if (elementValue.Length == 0)
            {
                Console.WriteLine("- Warning, empty element value for element name '{0}'", elementName);
            }
            writer.WriteString(elementValue);
            writer.WriteEndElement();
        }

        private static void PrintWrongDataFormat(string[] textArray, string[] elementTagNames, IndexOutOfRangeException e)
        {
            Console.Write("- Warning, wrong data format for '{0}'! ", textArray[LetterIndex]);
            Console.WriteLine("Expected {0} tag elements, got {1}", elementTagNames.Length - 1, textArray.Length - 1);
        
        }
        private static void XmlConverterDriver(List<string[]> textList, string filePath)
        {
            Console.WriteLine("Converting '{0}' to XML format...", Path.GetFileName(filePath));

            string previousLetter = ""; //To save information about previous letter, to which level the new tag should be. (E.g. deeper level)
            bool firstPerson = true;    //To know if to close the previous 'Person' tag before adding a new.
            string xmlFilePath = Path.GetFileNameWithoutExtension(filePath) + ".xml";

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter writer = XmlWriter.Create(xmlFilePath, settings);
            
            writer.WriteStartDocument();
            writer.WriteStartElement("people");

            foreach(string[] textArray in textList)
            {
                if(string.Equals(textArray[LetterIndex], "P"))
                {   
                    if(firstPerson == false) //Close the previous 'Person' tag before adding a new.
                    {
                        writer.WriteEndElement();

                        if (string.Equals(previousLetter,"F")) //If the previus letter is 'Family', it should close that 'Family' tag before adding a new 'Person' 
                        {
                            writer.WriteEndElement();
                        }
                    }
                    
                    writer.WriteStartElement("person");

                    string[] elementTagNames = GetTextFormats("P");
              
                    for (int i = ExcludedLetterIndex; i < PersonMaxSlots + 1; i++)
                    {
                        try
                        {
                            WriteToXmlFile(elementTagNames[i - 1], textArray[i], writer);
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            PrintWrongDataFormat(textArray, elementTagNames, e);
                        }
                    }
                  
                    previousLetter = textArray[LetterIndex]; 
                    firstPerson = false; // Will be used later if another Person will be added
                }
                else if(string.Equals(textArray[LetterIndex], "T"))
                {
                    writer.WriteStartElement("phone");
                    string[] elementTagNames = GetTextFormats("T");

                    for (int i = ExcludedLetterIndex; i < PhoneMaxSlots + 1; i++)
                    {
                        try
                        {
                            WriteToXmlFile(elementTagNames[i - 1], textArray[i], writer);
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            PrintWrongDataFormat(textArray, elementTagNames, e);
                        }
                    }

                    writer.WriteEndElement();
                }
                else if(string.Equals(textArray[LetterIndex], "A"))
                {
                    writer.WriteStartElement("address");
                    string[] elementTagNames = GetTextFormats("A");
                    
                    for (int i = ExcludedLetterIndex; i < AddressMaxSlots + 1; i++)
                    {
                        try
                        {
                            WriteToXmlFile(elementTagNames[i - 1], textArray[i], writer);
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            PrintWrongDataFormat(textArray, elementTagNames, e);
                        }
                    }

                    writer.WriteEndElement();
                }
                else if(string.Equals(textArray[0], "F"))
                {
                    if (string.Equals(previousLetter,"F")) //If the previus letter is 'Family', it should close that 'Family' tag before adding a new 'Family'. 
                    {
                        writer.WriteEndElement();
                    }
                    
                    writer.WriteStartElement("family");
                    string[] elementTagNames = GetTextFormats("F");
                    
                    for (int i = ExcludedLetterIndex; i < FamilyMaxSlots + 1; i++)
                    {
                        try
                        {
                            WriteToXmlFile(elementTagNames[i - 1], textArray[i], writer);
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            PrintWrongDataFormat(textArray, elementTagNames, e);
                        }
                    }
                    
                    previousLetter = textArray[LetterIndex];
                }
                else
                {
                    Console.WriteLine("Unknown letter '{0}'.", textArray[LetterIndex]);
                }
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
            
            Console.WriteLine("Created XML file '{0}'", xmlFilePath);
        }

        private static void ReadTextFormat()
        {
            try
            {
                string[] textFormatArray = File.ReadAllLines(Path.GetFileName(TextFormatFileName));

                foreach (string textRow in textFormatArray)
                {
                    string[] textRowArray = textRow.Split('|');

                    if (textFormatDictionary.ContainsKey(textFormatArray[LetterIndex]))
                    {
                        Console.WriteLine("Letter '{0}' already exists!", textRowArray[LetterIndex]);
                    }
                    else
                    {
                        string[] tagsArray = new string[textRowArray.Length];
                 
                        Array.Copy(textRowArray, 1, tagsArray, 0, textRowArray.Length - 1);
                        
                        textFormatDictionary.Add(textRowArray[LetterIndex], tagsArray);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }

        public static int Main(string[] args) {
            if (args.Length is > 1 or < 1) 
            {
                throw new ArgumentException("Error, you entered not correct number of argument. You need to enter 1 argument.");
            }
            
            ReadTextFormat();

            string filePath = args[ProgramArgument];
            List<string[]> textList = ReadInputFile(filePath);
            
            XmlConverterDriver(textList, filePath);
            
            return 0;
        }
    }
}