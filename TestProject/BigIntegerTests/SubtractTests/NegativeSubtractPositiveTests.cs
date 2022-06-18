using BigIntegerLibrary;

namespace TestProject.BigIntegerTests.SubtractTests;

[Category("負數減正數")]
public class NegativeSubtractPositiveTests
{
    [Test]
    public void _負1_減_2_等於_負3()
    {
        string actual   = (BigDecimal)"-1" - "2";
        var    expected = "-3";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _負3點2_減_2_等於_負5點2()
    {
        string actual   = (BigDecimal)"-3.2" - "2";
        var    expected = "-5.2";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _負2_減_3點2_等於_負5點2()
    {
        string actual   = (BigDecimal)"-2" - "3.2";
        var    expected = "-5.2";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _負1_減_9_等於_負10()
    {
        string actual   = (BigDecimal)"-1" - "9";
        var    expected = "-10";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _負1_減_99_等於_負100()
    {
        string actual   = (BigDecimal)"-1" - "99";
        var    expected = "-100";

        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void _負99_減_1_等於_負100()
    {
        string actual   = (BigDecimal)"-99" - "1";
        var    expected = "-100";

        Assert.That(actual, Is.EqualTo(expected));
    }
}
