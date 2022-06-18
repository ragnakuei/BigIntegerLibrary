using BigIntegerLibrary;

namespace TestProject.BigIntegerTests.PlusTests;

[Category("負數相加")]
public class NegativePlusTests
{
    [Test]
    public void _負1_加_負2_等於_負3()
    {
        string actual   = (BigDecimal)"-1" + "-2";
        var    expected = "-3";

        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void _負1點0_加_負2_等於_負3()
    {
        string actual   = (BigDecimal)"-1.0" + "-2";
        var    expected = "-3";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _負1_加_負9_等於_負10()
    {
        string actual   = (BigDecimal)"-1" + "-9";
        var    expected = "-10";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _負1_加_負99_等於_負100()
    {
        string actual   = (BigDecimal)"-1" + "-99";
        var    expected = "-100";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _負1_加_負99個9_等於_負1跟99個0()
    {
        string actual = (BigDecimal)"-1" + ("-" + new string(Enumerable.Range(0, 99).Select(i => '9').ToArray()));

        var expected = "-1" + new string(Enumerable.Range(0, 99).Select(i => '0').ToArray());

        Assert.That(actual, Is.EqualTo(expected));
    }
}
