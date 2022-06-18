using BigIntegerLibrary;

namespace TestProject.BigIntegerTests.SubtractTests;

[Category("正數減負數")]
public class PositiveSubtractNegativeTests
{
    [Test]
    public void _1_減_負2_等於_負3()
    {
        string actual   = (BigDecimal)"1" - "-2";
        var    expected = "3";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _1_減_負9_等於_10()
    {
        string actual   = (BigDecimal)"1" - "-9";
        var    expected = "10";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _1_減_負99_等於_100()
    {
        string actual   = (BigDecimal)"1" - "-99";
        var    expected = "100";

        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void _1點234_減_99點4567_等於_負98點2227()
    {
        string actual   = (BigDecimal)"1.234" - "99.4567";
        var    expected = "-98.2227";

        Assert.That(actual, Is.EqualTo(expected));
    }
}
