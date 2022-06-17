using BigIntegerLibrary;

namespace TestProject.BigIntegerTests.SubtractTests;

[Category("負整數相減")]
public class NegativeSubtractTests
{
    [Test]
    public void _負1_減_負2_等於_1()
    {
        var actual   = BigInteger.Subtract("-1", "-2");
        var expected = "1";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _負1_減_負9_等於_8()
    {
        var actual   = BigInteger.Subtract("-1", "-9");
        var expected = "8";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _負1_減_負99_等於_98()
    {
        var actual   = BigInteger.Subtract("-1", "-99");
        var expected = "98";

        Assert.AreEqual(expected, actual);
    }
}
