using System.Xml;
using System;
using System.Collections.Generic;

namespace XML_converter_refactored {
    public class XmlConverter {
        private const int ERROR_BAD_ARGUMENTS = 0xA0;
        private const int PROGRAM_ARGUMENT = 0;
        private const int LETTER_INDEX = 0;
        private const int PERSON_MAX_SLOTS = 2;
        private const int PHONE_MAX_SLOTS = 2;
        private const int ADDRESS_MAX_SLOTS = 3;
        private const int FAMILY_MAX_SLOTS = 2;
        private const int EXCLUDED_LETTER_INDEX = 1;
        
        private const string TEXT_FORMAT_FILE_NAME = "textFormat.txt";
        

        //private List<string[]> _textFormat = new List<string[]>();
        private static Dictionary<string, string[]> _textFormatDictionary = new Dictionary<string, string[]>();
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
                Console.WriteLine("{0} Exception caught.", e);
            }
            return textList;
        }

        private static string[] getTextFormats(string letterKey)
        {
            string[] textFormatArray;
            bool hasValue = _textFormatDictionary.TryGetValue(letterKey, out textFormatArray);

            if (!hasValue)
            {
                Console.WriteLine("Values could not be found with the key '{0}'.", letterKey);
            }
            return textFormatArray;
        }

        private static void WriteToXml(string elementName, string elementValue, XmlWriter writer)
        {
            writer.WriteStartElement(elementName);
            writer.WriteString(elementValue);
            writer.WriteEndElement();
        }
        private static void ConvertToXml(List<string[]> textList, string filePath)
        {
            Console.WriteLine("Converting to XML format...");

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
                if(string.Equals(textArray[LETTER_INDEX], "P"))
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
                    string[] elementNames = getTextFormats("P");
                    
                    for (int i = EXCLUDED_LETTER_INDEX; i < PERSON_MAX_SLOTS; i++)
                    {
                        WriteToXml(elementNames[i - 1], textArray[i], writer);
                    }
                    
                    previousLetter = textArray[LETTER_INDEX]; 
                    firstPerson = false; // Will be used later if another Person will be added
                }
                else if(string.Equals(textArray[LETTER_INDEX], "T"))
                {
                    writer.WriteStartElement("phone");
                    string[] elementNames = getTextFormats("T");
                    
                    for (int i = EXCLUDED_LETTER_INDEX; i < PHONE_MAX_SLOTS; i++)
                    {
                        WriteToXml(elementNames[i - 1], textArray[i], writer);
                    }

                    writer.WriteEndElement();
                }
                else if(string.Equals(textArray[LETTER_INDEX], "A"))
                {
                    writer.WriteStartElement("address");
                    string[] elementNames = getTextFormats("A");
                    
                    for (int i = EXCLUDED_LETTER_INDEX; i < ADDRESS_MAX_SLOTS; i++)
                    {
                        WriteToXml(elementNames[i - 1], textArray[i], writer);
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
                    string[] elementNames = getTextFormats("F");
                    
                    for (int i = EXCLUDED_LETTER_INDEX; i < FAMILY_MAX_SLOTS; i++)
                    {
                        WriteToXml(elementNames[i - 1], textArray[i], writer);
                    }
                    
                    previousLetter = textArray[LETTER_INDEX];
                }  
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();

            Console.WriteLine("Successfully converted and generated a XML file, 'people.xml'!");
        }

        private static void GetTextFormat()
        {
            try
            {
                string[] textFormatArray = File.ReadAllLines(Path.GetFileName(TEXT_FORMAT_FILE_NAME));

                foreach (string textRow in textFormatArray)
                {
                    string[] textRowArray = textRow.Split('|');

                    if (_textFormatDictionary.ContainsKey(textFormatArray[LETTER_INDEX]))
                    {
                        Console.WriteLine("Letter '{0}' already exists!", textRowArray[LETTER_INDEX]);
                    }
                    else
                    {
                        string[] tagsArray = new string[textRowArray.Length];
                 
                        Array.Copy(textRowArray, 1, tagsArray, 0, textRowArray.Length - 1);
                        
                        _textFormatDictionary.Add(textRowArray[LETTER_INDEX], tagsArray);
                    }
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }

        public static void Main(string[] args) {
            if (args.Length is > 1 or < 1) {
                Console.WriteLine("Error, you entered {0} argument(s). You can only enter 1.", args.Length);
                Environment.ExitCode = ERROR_BAD_ARGUMENTS;
            }
            
            GetTextFormat();

            string filePath = args[PROGRAM_ARGUMENT];
            List<string[]> textList = ReadInputFile(filePath);
            
            ConvertToXml(textList, filePath);
        }
    }
}