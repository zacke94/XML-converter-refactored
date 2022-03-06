using System.Xml.Serialization;

namespace XML_converter_refactored;

public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length is > ConstVariables.AcceptedArgument or < ConstVariables.AcceptedArgument)
        {
            throw new ArgumentException("Error, you entered not correct number of argument. You need to enter 1 argument.");
        }
        string inputTxtFileName = args[ConstVariables.ProgramArgument];
            
        if (File.Exists(inputTxtFileName))
        {
            string[] splittedLine;
            string[] lines = File.ReadAllLines(inputTxtFileName);
            string previousLetter = "";
            string outputXmlFileName = Path.GetFileNameWithoutExtension(inputTxtFileName) + ".xml";
            bool firstPerson = true;
            
            XmlSerializer peopleSerializer = new XmlSerializer(typeof(People));
            TextWriter xmlFileWriter = new StreamWriter(outputXmlFileName);

            People peopleObject = new People();
            Person personObject = new Person();
            
            Console.WriteLine("Converting to XML....");
            
            foreach (string line in lines)
            {
                splittedLine = line.Split("|");
                
                try
                {
                    if (string.Equals(splittedLine[ConstVariables.LetterIndex].ToUpper(), "P"))
                    {
                        if (firstPerson == false)
                        {
                            peopleObject.Person.Add(personObject);
                            Person newPersonObject = new Person();
                            personObject = newPersonObject;
                        }
                        personObject.FirstName = splittedLine[ConstVariables.FirstNameIndex];
                        personObject.LastName = splittedLine[ConstVariables.LastNameIndex];

                        firstPerson = false;
                        previousLetter = splittedLine[ConstVariables.LetterIndex];
                    }
                    else if (string.Equals(splittedLine[ConstVariables.LetterIndex].ToUpper(), "T"))
                    {
                        Telephone newTelephoneObject = new Telephone();
                        newTelephoneObject.MobileNumber = splittedLine[ConstVariables.MobileNumberIndex];
                        newTelephoneObject.LandlineNumber = splittedLine[ConstVariables.LandlineNumberIndex];

                        if (string.Equals(previousLetter, "F"))
                        {
                            Family updateFamilyObject = personObject.Family.Last();
                            updateFamilyObject.Telephone = newTelephoneObject;
                        }
                        else
                        {
                            personObject.Telephone = newTelephoneObject;
                        }
                    }
                    else if (string.Equals(splittedLine[ConstVariables.LetterIndex].ToUpper(), "A"))
                    {
                        Address newAddressObject = new Address();
                        newAddressObject.Street = splittedLine[ConstVariables.StreetIndex];
                        newAddressObject.City = splittedLine[ConstVariables.CityIndex];
                        newAddressObject.Zipcode = splittedLine[ConstVariables.ZipcodeIndex];

                        if (string.Equals(previousLetter, "F"))
                        {
                            Family updateFamilyObject = personObject.Family.Last();
                            updateFamilyObject.Address = newAddressObject;
                        }
                        else
                        {
                            personObject.Address = newAddressObject;  
                        }
                    }
                    else if (string.Equals(splittedLine[ConstVariables.LetterIndex].ToUpper(), "F"))
                    {
                        Family newFamilyObject = new Family();
                        newFamilyObject.Name = splittedLine[ConstVariables.FamilyMemberNameIndex];
                        newFamilyObject.Year = splittedLine[ConstVariables.FamilyMemberYearIndex];
                      
                        personObject.Family.Add(newFamilyObject);
                        previousLetter = splittedLine[ConstVariables.LetterIndex];
                    }
                    else
                    {
                        Console.WriteLine("- Error, unknown input: '{0}'", splittedLine[ConstVariables.LetterIndex]);
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine("- Error, missing input field!");
                    Console.WriteLine("Exception caught: {0}", e);
                }
            }
            peopleObject.Person.Add(personObject);

            peopleSerializer.Serialize(xmlFileWriter, peopleObject);
            xmlFileWriter.Close();
            Console.WriteLine("XML file created: {0}", outputXmlFileName);
        }
        else
        {
            Console.WriteLine("- Error, file '{0}' doesn't exist", inputTxtFileName);
            return ConstVariables.ReturnFail;
        }
        return ConstVariables.ReturnSuccess;
    }
}