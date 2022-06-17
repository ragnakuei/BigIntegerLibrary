using BigIntegerLibrary;

namespace TestProject.BigIntegerTests.PlusTests;

[Category("正整數加負整數")]
public class PositivePlusNegativeTests
{
    [Test]
    public void _1_加_負2_等於_負1()
    {
        var actual   = BigInteger.Plus("1", "-2");
        var expected = "-1";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _2_加_負1_等於_1()
    {
        var actual   = BigInteger.Plus("2", "-1");
        var expected = "1";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _100_加_負1_等於_99()
    {
        var actual   = BigInteger.Plus("100", "-1");
        var expected = "99";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _負100_加_1_等於_負99()
    {
        var actual   = BigInteger.Plus("-100", "1");
        var expected = "-99";

        Assert.AreEqual(expected, actual);
    }
}
