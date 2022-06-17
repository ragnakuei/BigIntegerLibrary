namespace BigIntegerLibrary;

public class BigInteger
{
    private static Dictionary<string, char[]> _plusMappings = new();

    static BigInteger()
    {
        foreach (var i in Enumerable.Range(0, 10))
        foreach (var j in Enumerable.Range(0, 10))
        {
            _plusMappings[$"{i}+{j}"] = (i + j).ToString().ToCharArray();

            if (i < j)
            {
                _plusMappings[$"{i}-{j}"] = new char[] { '-', (10 + i - j).ToString()[0] };
            }
            else
            {
                _plusMappings[$"{i}-{j}"] = (i - j).ToString().ToCharArray();
            }
        }
    }

    public static string Plus(string s1, string s2)
    {
        var resultPrefix = string.Empty;

        var s1Negative = s1.StartsWith("-");
        var s2Negative = s2.StartsWith("-");

        var s1Target = s1.TrimStart('-', '0');
        var s2Target = s2.TrimStart('-', '0');

        // 多一位是進位緩衝
        var maxDigits      = Math.Max(s1Target.Length, s2Target.Length) + 1;
        var s1TargetPadded = s1Target.PadLeft(maxDigits, '0').ToCharArray();
        var s2TargetPadded = s2Target.PadLeft(maxDigits, '0').ToCharArray();


        if (!(s1Negative ^ s2Negative))
        {
            // 正負同號

            if (s1Negative)
            {
                resultPrefix = "-";
            }

            return resultPrefix + PositivesPlus(s1TargetPadded, s2TargetPadded);
        }
        else
        {
            // 正負不同號

            if (OneIsBigger(s1TargetPadded, s2TargetPadded))
            {
                if (s1Negative)
                {
                    resultPrefix = "-";
                }
            }
            else
            {
                if (s2Negative)
                {
                    resultPrefix = "-";
                }

                (s1TargetPadded, s2TargetPadded) = (s2TargetPadded, s1TargetPadded);
            }

            return resultPrefix + BiggerSubtractSmaller(s1TargetPadded, s2TargetPadded);
        }
    }

    public static string? Subtract(string s1, string s2)
    {
        var resultPrefix = string.Empty;

        var s1Negative = s1.StartsWith("-");
        var s2Negative = s2.StartsWith("-");

        var s1Target = s1.TrimStart('-', '0');
        var s2Target = s2.TrimStart('-', '0');

        // 多一位是進位緩衝
        var maxDigits      = Math.Max(s1Target.Length, s2Target.Length) + 1;
        var s1TargetPadded = s1Target.PadLeft(maxDigits, '0').ToCharArray();
        var s2TargetPadded = s2Target.PadLeft(maxDigits, '0').ToCharArray();

        if (s1Negative == false && s2Negative == false)
        {
            // 正減正

            if (OneIsBigger(s1TargetPadded, s2TargetPadded))
            {
            }
            else
            {
                resultPrefix = "-";

                (s1TargetPadded, s2TargetPadded) = (s2TargetPadded, s1TargetPadded);
            }

            return resultPrefix + BiggerSubtractSmaller(s1TargetPadded, s2TargetPadded);
        }
        else if (s1Negative && s2Negative)
        {
            // 負減負

            if (OneIsBigger(s1TargetPadded, s2TargetPadded))
            {
                resultPrefix = "-";
            }
            else
            {
                (s1TargetPadded, s2TargetPadded) = (s2TargetPadded, s1TargetPadded);
            }

            return resultPrefix + BiggerSubtractSmaller(s1TargetPadded, s2TargetPadded);
        }
        else
        {
            if (s1Negative && s2Negative == false)
            {
                // 負減正
                resultPrefix = "-";    
            }

            if (s1Negative == false && s2Negative)
            {
                // 正減負    
            }
            
            return resultPrefix + PositivesPlus(s1TargetPadded, s2TargetPadded);
        }
    }

    /// <summary>
    /// 第一個數字大於第二個數字
    /// <remarks>二個陣列同長度</remarks>
    /// </summary>
    private static bool OneIsBigger(char[] s1TargetPadded, char[] s2TargetPadded)
    {
        for (var i = 0; i < s1TargetPadded.Length; i++)
        {
            if (s1TargetPadded[i] == s2TargetPadded[i])
            {
                continue;
            }

            return s1TargetPadded[i] > s2TargetPadded[i];
        }

        return false;
    }

    /// <summary>
    /// 正整數相加
    /// <remarks>二個陣列同長度</remarks>
    /// </summary>
    private static string PositivesPlus(char[] s1TargetPadded, char[] s2TargetPadded)
    {
        // 以 s1 + s2 => s1 為思考方式，而不是 s1 + s2 => result
        var result    = s1TargetPadded;
        var maxDigits = s1TargetPadded.Length;

        // 以 result + s2Target 來思考即可
        for (var i = maxDigits - 1; i >= 0; i--)
        {
            // var s1Digit = result[i];
            var s2Digit = s2TargetPadded[i];

            CarryDigit(result, i, s2Digit);
        }

        return new string(result).TrimStart('0');
    }

    /// <summary>
    /// 大數減小數
    /// <remarks>二個陣列同長度</remarks>
    /// </summary>
    private static string BiggerSubtractSmaller(char[] bigger, char[] smaller)
    {
        // 以 bigger - smaller => bigger 為思考方式，而不是 bigger - smaller => result
        var result    = bigger;
        var maxDigits = bigger.Length;

        for (var i = 0; i < maxDigits; i++)
        {
            var substractResults = _plusMappings[$"{bigger[i]}-{smaller[i]}"];
            if (substractResults.Length == 1)
            {
                result[i] = substractResults[0];
            }
            else
            {
                result[i] = substractResults[1];
                AbdicateDigit(result, i - 1);
            }
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

        var sumResults = _plusMappings.GetValueOrDefault($"{result[index]}+{plugNumber}");
        if (sumResults.Length == 1)
        {
            result[index] = sumResults[0];
        }
        else
        {
            result[index] = sumResults[1];

            CarryDigit(result, index - 1, sumResults[0]);
        }
    }

    /// <summary>
    /// 遞迴退位處理
    /// </summary>
    private static void AbdicateDigit(char[] result, int index)
    {
        var substractResults = _plusMappings[$"{result[index]}-1"];
        if (substractResults.Length == 1)
        {
            result[index] = substractResults[0];
        }
        else
        {
            result[index] = substractResults[1];
            AbdicateDigit(result, index - 1);
        }
    }

    private enum OperatorType
    {
        Positives,
        Negatives,
    }
}
