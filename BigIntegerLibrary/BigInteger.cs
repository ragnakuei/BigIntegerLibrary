namespace BigIntegerLibrary;

public class BigInteger
{
    private static Dictionary<string, char[]> _plusMappings = new();

    static BigInteger()
    {
        foreach (var i in Enumerable.Range(0, 10))
        foreach (var j in Enumerable.Range(0, 10))
        {
            _plusMappings[i.ToString() + j] = (i + j).ToString().ToCharArray();
        }
    }

    public static string Plus(string s1, string s2)
    {
        var s1Target = s1.TrimStart('0');
        var s2Target = s2.TrimStart('0');

        // 多二位是進位緩衝
        var maxDigits      = Math.Max(s1Target.Length, s2Target.Length) + 1;
        var s1TargetPadded = s1Target.PadLeft(maxDigits, '0').ToCharArray();
        var s2TargetPadded = s2Target.PadLeft(maxDigits, '0').ToCharArray();

        var result = s1TargetPadded;

        // 以 result + s2Target 來思考即可
        for (var i = maxDigits - 1; i >= 0; i--)
        {
            var s1Digit = result[i];
            var s2Digit = s2TargetPadded[i];

            CarryDigit(result, i, s2Digit);
        }
        
        return new string(result).TrimStart('0');
    }

    /// <summary>
    /// 遞迴進位處理
    /// </summary>
    private static void CarryDigit(char[] result, int index, char plugNumber)
    {
        if (index < 0)
        {
            return;
        }

        var sumDigit = _plusMappings.GetValueOrDefault(result[index] + plugNumber.ToString());
        if (sumDigit.Length == 1)
        {
            result[index] = sumDigit[0];
        }
        else
        {
            result[index] = sumDigit[1];

            CarryDigit(result, index - 1, sumDigit[0]);
        }
    }
}
