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

    [TestCase((object) new[] {"testcase-correct-input-file.txt"})]
    public void CorrectConverting(string[] args)
    {
        Assert.AreEqual(0, MainClass.Main(args));
    }
    
    [TestCase((object) new[] {"argument 1", "argument 2"})]
    public void ThrowArgumentException(string[] args)
    {
        string expectedMessage = "Error, you entered not correct number of argument. You need to enter 1 argument.";
        var thrownMessage = Assert.Throws<ArgumentException>(
            () => MainClass.Main(args));
        
        Assert.That(thrownMessage?.Message, Is.EqualTo(expectedMessage));
    }
}