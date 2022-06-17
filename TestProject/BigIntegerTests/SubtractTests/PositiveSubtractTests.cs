using BigIntegerLibrary;

namespace TestProject.BigIntegerTests.SubtractTests;

[Category("正整數相減")]
public class PositiveSubtractTests
{
    [Test]
    public void _2_減_1_等於_1()
    {
        var actual   = (BigInteger)"2" - "1";
        var expected = "1";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _1_減_2_等於_負1()
    {
        var actual   = (BigInteger)"1" - "2";
        var expected = "-1";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _1_減_9_等於_負8()
    {
        var actual   = (BigInteger)"1" - "9";
        var expected = "-8";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _1_減_99_等於_負98()
    {
        var actual   = (BigInteger)"1" - "99";
        var expected = "-98";

        Assert.AreEqual(expected, actual);
    }
}
