using BigIntegerLibrary;

namespace TestProject.BigIntegerTests.PlusTests;

[Category("負整數相加")]
public class NegativePlusTests
{
    [Test]
    public void _負1_加_負2_等於_負3()
    {
        var actual   = (BigInteger)"-1" + "-2";
        var expected = "-3";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _負1_加_負9_等於_負10()
    {
        var actual   = (BigInteger)"-1" + "-9";
        var expected = "-10";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _負1_加_負99_等於_負100()
    {
        var actual   = (BigInteger)"-1" + "-99";
        var expected = "-100";

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void _負1_加_負99個9_等於_負1跟99個0()
    {
        var actual = (BigInteger)"-1" + ("-" + new string(Enumerable.Range(0, 99).Select(i => '9').ToArray()));

        var expected = "-1" + new string(Enumerable.Range(0, 99).Select(i => '0').ToArray());

        Assert.AreEqual(expected, actual);
    }
}
