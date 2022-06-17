using BigIntegerLibrary;

namespace TestProject.BigIntegerTests.SubtractTests;

[Category("正整數減負整數")]
public class PositiveSubtractNegativeTests
{
    [Test]
    public void _1_減_負2_等於_負3()
    {
        var actual   = (BigInteger)"1" - "-2";
        var expected = "3";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _負1_減_9_等於_負10()
    {
        var actual   = (BigInteger)"1" - "-9";
        var expected = "10";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _負1_減_99_等於_負100()
    {
        var actual   = (BigInteger)"1" - "-99";
        var expected = "100";

        Assert.AreEqual(expected, actual);
    }
}
