using BigIntegerLibrary;

namespace TestProject.BigIntegerTests;

public class BigDecimalTests
{
    [Test]
    public void 大於01()
    {
        var actual = (BigDecimal)"2" > "1";

        Assert.True(actual);
    }

    [Test]
    public void 大於02()
    {
        var actual = (BigDecimal)"2.1" > "2.09";

        Assert.True(actual);
    }

    [Test]
    public void 含小數大於01()
    {
        var actual = (BigDecimal)"-2" > "-3";

        Assert.True(actual);
    }

    [Test]
    public void 含小數大於02()
    {
        var actual = (BigDecimal)"-2" > "-2.01";

        Assert.True(actual);
    }
}
