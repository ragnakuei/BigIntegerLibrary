using BigIntegerLibrary;

namespace TestProject.BigIntegerTests.MultiplicationTests;

[Category("異號相乘")]
public class OppositeSignMultiplicationTests
{
    [Test]
    public void Case01()
    {
        string actual   = (BigDecimal)"-1.1" * "2.2";
        var    expected = "-2.42";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Case02()
    {
        string actual   = (BigDecimal)"9.9" * "-9.9";
        var    expected = "-98.01";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Case03()
    {
        string actual   = (BigDecimal)"-99.99" * "99.99";
        var    expected = "-9998.0001";

        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void Case04()
    {
        string actual   = (BigDecimal)"999.99" * "-999.99";
        var    expected = "-999980.0001";

        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void Case05()
    {
        string actual   = (BigDecimal)"-2469135780246913578.002469" * "5";
        var    expected = "-12345678901234567890.012345";

        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    public void Case06()
    {
        string actual   = (BigDecimal)"2469135780246913578.002469" * "-5";
        var    expected = "-12345678901234567890.012345";

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void Case09()
    {
        string actual   = (BigDecimal)"-9999999999.999999" * "9999999999.999999";
        var    expected = "-99999999999999980000.000000000001";

        Assert.That(actual, Is.EqualTo(expected));
    }
}
