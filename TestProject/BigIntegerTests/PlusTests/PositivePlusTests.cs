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

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _1點2_加_2點3_等於_3點5()
    {
        string actual   = (BigDecimal)"1.2" + "2.3";
        var    expected = "3.5";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _1_加_9_等於_10()
    {
        string actual   = (BigDecimal)"1" + "9";
        var    expected = "10";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _1_加_99_等於_100()
    {
        string actual   = (BigDecimal)"1" + "99";
        var    expected = "100";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _1_加_99個9_等於_1跟99個0()
    {
        string actual = (BigDecimal)"1" + new string(Enumerable.Range(0, 99).Select(i => '9').ToArray());

        var expected = "1" + new string(Enumerable.Range(0, 99).Select(i => '0').ToArray());

        Assert.AreEqual(expected, actual);
    }
}
