using BigIntegerLibrary;

namespace TestProject.BigIntegerTests.PlusTests;

[Category("正數相加")]
public class PositivePlusTests
{
    [Test]
    public void _1_加_2_等於_3()
    {
        string actual   = (BigDecimal)"1" + "2";
        var    expected = "3";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _1點2_加_2點3_等於_3點5()
    {
        string actual   = (BigDecimal)"1.2" + "2.3";
        var    expected = "3.5";

        Assert.That(actual, Is.EqualTo(expected));
    }


    [Test]
    public void _1_加_2點6_等於_3點6()
    {
        string actual   = (BigDecimal)"1" + "2.6";
        var    expected = "3.6";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _2點6_加_1_等於_3點6()
    {
        string actual   = (BigDecimal)"2.6" + "1";
        var    expected = "3.6";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _2點0_加_1_等於_3()
    {
        string actual   = (BigDecimal)"2.0" + "1";
        var    expected = "3";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _1_加_9_等於_10()
    {
        string actual   = (BigDecimal)"1" + "9";
        var    expected = "10";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _1_加_99_等於_100()
    {
        string actual   = (BigDecimal)"1" + "99";
        var    expected = "100";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _1_加_99個9_等於_1跟99個0()
    {
        string actual = (BigDecimal)"1" + new string(Enumerable.Range(0, 99).Select(i => '9').ToArray());

        var expected = "1" + new string(Enumerable.Range(0, 99).Select(i => '0').ToArray());

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void 小數進位01()
    {
        string actual   = (BigDecimal)"1.56789" + "99.67890";
        var    expected = "101.24679";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void 小數不進位01()
    {
        string actual   = (BigDecimal)"12.34" + "23.45";
        var    expected = "35.79";

        Assert.That(actual, Is.EqualTo(expected));
    }
}

