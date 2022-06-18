using BigIntegerLibrary;

namespace TestProject.BigIntegerTests.PlusTests;

[Category("負數加正數")]
public class NegativePlusPositiveTests
{
    [Test]
    public void _負100_加_1_等於_負99()
    {
        string actual   = (BigDecimal)"-100" + "1";
        var    expected = "-99";

        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void _負100點1234_加_1點23456_等於_負98點88884()
    {
        string actual   = (BigDecimal)"-100.1234" + "1.23456";
        var    expected = "-98.88884";

        Assert.That(actual, Is.EqualTo(expected));
    }
}
