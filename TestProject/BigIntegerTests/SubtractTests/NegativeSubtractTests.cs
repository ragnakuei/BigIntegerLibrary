using BigIntegerLibrary;

namespace TestProject.BigIntegerTests.SubtractTests;

[Category("負數相減")]
public class NegativeSubtractTests
{
    [Test]
    public void _負1_減_負2_等於_1()
    {
        string actual   = (BigDecimal)"-1" - "-2";
        var    expected = "1";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _負1_減_負9_等於_8()
    {
        string actual   = (BigDecimal)"-1" - "-9";
        var    expected = "8";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _負1_減_負99_等於_98()
    {
        string actual   = (BigDecimal)"-1" - "-99";
        var    expected = "98";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _負100_減_1_等於_負101()
    {
        string actual   = (BigDecimal)"-100" - "-1";
        var    expected = "-99";

        Assert.That(actual, Is.EqualTo(expected));
    }
}
