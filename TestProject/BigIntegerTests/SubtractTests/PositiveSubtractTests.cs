using BigIntegerLibrary;

namespace TestProject.BigIntegerTests.SubtractTests;

[Category("正數相減")]
public class PositiveSubtractTests
{
    [Test]
    public void _2_減_1_等於_1()
    {
        string actual   = (BigDecimal)"2" - "1";
        var    expected = "1";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _1_減_2_等於_負1()
    {
        string actual   = (BigDecimal)"1" - "2";
        var    expected = "-1";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _1_減_9_等於_負8()
    {
        string actual   = (BigDecimal)"1" - "9";
        var    expected = "-8";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _1_減_99_等於_負98()
    {
        string actual   = (BigDecimal)"1" - "99";
        var    expected = "-98";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _99點4567_減_1點234_等於_98點2227()
    {
        string actual   = (BigDecimal)"99.4567" - "1.234";
        var    expected = "98.2227";

        Assert.That(actual, Is.EqualTo(expected));
    }
}
