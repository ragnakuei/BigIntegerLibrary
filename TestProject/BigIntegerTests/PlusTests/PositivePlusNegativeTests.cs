using BigIntegerLibrary;

namespace TestProject.BigIntegerTests.PlusTests;

[Category("正數加負數")]
public class PositivePlusNegativeTests
{
    [Test]
    public void _1_加_負2_等於_負1()
    {
        string actual   = (BigDecimal)"1" + "-2";
        var    expected = "-1";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _2_加_負1_等於_1()
    {
        string actual   = (BigDecimal)"2" + "-1";
        var    expected = "1";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void _100_加_負1_等於_99()
    {
        string actual   = (BigDecimal)"100" + "-1";
        var    expected = "99";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void 小數不借位01()
    {
        string actual   = (BigDecimal)"100.6789" + "-1.56789";
        var    expected = "99.11101";

        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void 小數借位01()
    {
        string actual   = (BigDecimal)"100.56789" + "-1.6789";
        var    expected = "98.88899";

        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void 相加為零01()
    {
        string actual   = (BigDecimal)"12.23" + "-12.23";
        var    expected = "0";

        Assert.That(actual, Is.EqualTo(expected));
    }
}
