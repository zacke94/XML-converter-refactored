using System;
using NUnit.Framework;
using XML_converter_refactored;

namespace XML_converter_refactoredTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCase((object) new[] {"people.txt"})]
    public void CorrectConversion(string[] args)
    {
        Assert.AreEqual(0, XmlConverter.Main(args));
    }
    
    [Test]
    [TestCase((object) new[] {"argument 1", "argument 2"})]
    //[TestCase((object) new[] {""})]
    public void ThrowArgumentException(string[] args)
    {
        string message = "Error, you entered not correct number of argument. You need to enter 1 argument.";
        var thrownMessage = Assert.Throws<ArgumentException>(
            () => XmlConverter.Main(args));
        
        Assert.That(thrownMessage.Message, Is.EqualTo(message));
    }
}