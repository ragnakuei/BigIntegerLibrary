using BigIntegerLibrary;

namespace TestProject;

public class PositivePlusTests
{
    [Test]
    [Category("正整數相加")]
    public void _1_加_2_等於_3()
    {
        var actual = BigInteger.Plus("1", "2");
        var expected = "3";
        
        Assert.AreEqual(expected, actual);
    }
    
    [Test]
    [Category("正整數相加")]
    public void _1_加_9_等於_10()
    {
        var actual = BigInteger.Plus("1", "9");
        var expected = "10";
        
        Assert.AreEqual(expected, actual);
    }
    
    [Test]
    [Category("正整數相加")]
    public void _1_加_99_等於_100()
    {
        var actual = BigInteger.Plus("1", "99");
        var expected = "100";
        
        Assert.AreEqual(expected, actual);
    }
    
    [Test]
    [Category("正整數相加")]
    public void _1_加_99個9_等於_1跟99個0()
    {
        var actual   = BigInteger.Plus("1", new string( Enumerable.Range(0, 99).Select(i => '9').ToArray()));
        var expected = "1" + new string( Enumerable.Range(0, 99).Select(i => '0').ToArray());
        
        Assert.AreEqual(expected, actual);
    }
}
