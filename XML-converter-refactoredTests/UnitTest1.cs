using System;
using NUnit.Framework;
using XML_converter_refactored;

namespace XML_converter_refactoredTests;
public class Tests
{
    [TestCase((object) new[] {"testcase-correct-input-file.txt"})]
    public void CorrectConverting(string[] args)
    {
        Assert.AreEqual(ConstVariables.ReturnSuccess, Program.Main(args));
    }

    [TestCase((object) new[] {"non-existing-file.txt"})]
    public void NonExistingFile(string[] args)
    {
        Assert.AreEqual(ConstVariables.ReturnFail, Program.Main(args));
    }
    
    [TestCase((object) new[] {"argument 1", "argument 2"})]
    public void ThrowArgumentExceptionTwoArguments(string[] args)
    {
        string expectedMessage = "Error, you entered not correct number of argument. You need to enter 1 argument.";
        var thrownMessage = Assert.Throws<ArgumentException>(
            () => Program.Main(args));
        
        Assert.AreEqual(thrownMessage?.Message, expectedMessage);
    }
    
    [TestCase((object) new [] {""})]
    public void ReturnFailNonArgument(string[] args)
    {
        Assert.AreEqual(ConstVariables.ReturnFail,Program.Main(args));
    }
}