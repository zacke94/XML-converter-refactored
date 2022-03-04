using System.Xml.Serialization;

namespace XML_converter_refactored;

public static class Program
{
    public static int Main(string[] args)
    {
        const int AcceptedArgument = ConstVariables.AcceptedArgument;
        
        if (args.Length is > AcceptedArgument or < AcceptedArgument)
        {
            throw new ArgumentException("Error, you entered not correct number of argument. You need to enter 1 argument.");
        }
        
        string inputTxtFileName = args[ConstVariables.ProgramArgument];
            
        if (File.Exists(inputTxtFileName))
        {
            string[] splittedLine;
            string previousLetter = "";
            string outputXmlFileName = Path.GetFileNameWithoutExtension(inputTxtFileName) + ".xml";

            XmlSerializer peopleSerializer = new XmlSerializer(typeof(People));
            TextWriter xmlFile = new StreamWriter(outputXmlFileName);

            People people = new People();
            Person person = new Person();
            
            bool firstPerson = true;

            string[] lines = File.ReadAllLines(inputTxtFileName);

            foreach (string line in lines)
            {
                splittedLine = line.Split("|");

                try
                {
                    if (string.Equals(splittedLine[ConstVariables.LetterIndex].ToUpper(), "P"))
                    {
                        if (firstPerson == false)
                        {
                            people.person.Add(person);
                            Person newPerson = new Person();
                            person = newPerson;
                        }

                        person.firstName = splittedLine[ConstVariables.FirstElement];
                        person.lastName = splittedLine[ConstVariables.SecondElement];

                        firstPerson = false;
                        previousLetter = splittedLine[ConstVariables.LetterIndex];
                    }
                    else if (string.Equals(splittedLine[ConstVariables.LetterIndex].ToUpper(), "T"))
                    {
                        Telephone newTelephone = new Telephone();
                        newTelephone.mobileNumber = splittedLine[ConstVariables.FirstElement];
                        newTelephone.landlineNumber = splittedLine[ConstVariables.SecondElement];

                        if (string.Equals(previousLetter, "F"))
                        {
                            Family updateFamily = person.family.Last();
                            updateFamily.telephone = newTelephone;
                        }
                        else
                        {
                            person.telephone = newTelephone;
                        }
                    }
                    else if (string.Equals(splittedLine[ConstVariables.LetterIndex].ToUpper(), "A"))
                    {
                        Address newAddress = new Address();
                        newAddress.street = splittedLine[ConstVariables.FirstElement];
                        newAddress.city = splittedLine[ConstVariables.SecondElement];
                        newAddress.zipcode = splittedLine[ConstVariables.ThirdElement];

                        if (string.Equals(previousLetter, "F"))
                        {
                            Family updateFamily = person.family.Last();
                            updateFamily.address = newAddress;
                        }
                        else
                        {
                            person.address = newAddress;  
                        }
                    }
                    else if (string.Equals(splittedLine[ConstVariables.LetterIndex].ToUpper(), "F"))
                    {
                        Family newFamily = new Family();
                        newFamily.name = splittedLine[ConstVariables.FirstElement];
                        newFamily.year = splittedLine[ConstVariables.SecondElement];
                      
                        person.family.Add(newFamily);
                        previousLetter = splittedLine[ConstVariables.LetterIndex];
                    }
                    else
                    {
                        Console.WriteLine("- Wrong input: '{0}'", splittedLine[ConstVariables.LetterIndex]);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("- Error, wrong input!");
                    Console.WriteLine("Exception caught: {0}", e);
                }
            }
            people.person.Add(person);

            peopleSerializer.Serialize(xmlFile, people);
            xmlFile.Close(); 
        }
        else
        {
            Console.WriteLine("- Error, file '{0}' doesn't exist", inputTxtFileName);
            return ConstVariables.ReturnFail;
        }
        return ConstVariables.ReturnSuccess;
    }
}