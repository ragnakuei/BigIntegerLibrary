namespace BigIntegerLibrary;

internal static class Calculator
{
    /*
     * 加減法的設計概念為：以二數的正負號來判斷，是要 二數相加或大數減小數 !
     * 二數相加：如果二數的正負號相同，則相加
     * 大數減小數：以大數減小數，省去多餘的退位判斷
     */

    /// <summary>
    /// 加減用查找表
    /// </summary>
    private static readonly Dictionary<string, char[]> _opeartorMappings = new();

    static Calculator()
    {
        foreach (var i in Enumerable.Range(0, 10))
        foreach (var j in Enumerable.Range(0, 10))
        {
            _opeartorMappings[$"{i}+{j}"] = (i + j).ToString().ToCharArray();

            if (i < j)
            {
                _opeartorMappings[$"{i}-{j}"] = new[] { '-', (10 + i - j).ToString()[0] };
            }
            else
            {
                _opeartorMappings[$"{i}-{j}"] = (i - j).ToString().ToCharArray();
            }
        }
    }


    public static BigDecimal Plus(BigDecimal target1, BigDecimal target2)
    {
        // 確認已 Sync Padded
        SyncPadded(target1, target2);
        var result = new BigDecimal(target1, target2);

        if (!(target1.IsNegative ^ target2.IsNegative))
        {
            // 正負同號

            if (target1.IsNegative)
            {
                result.IsNegative = true;
            }

            return Plus(target1, target2, result);
        }
        else
        {
            // 正負不同號

            if (IsGreaterThan(target1, target2, ignoreSign: true))
            {
                if (target1.IsNegative)
                {
                    result.IsNegative = true;
                }
            }
            else
            {
                if (target2.IsNegative)
                {
                    result.IsNegative = true;
                }

                (target1, target2) = (target2, target1);
            }

            return BiggerSubtractSmaller(target1, target2, result);
        }
    }

    public static BigDecimal Subtract(BigDecimal target1, BigDecimal target2)
    {
        // 確認已 Sync Padded
        SyncPadded(target1, target2);
        var result = new BigDecimal(target1, target2);

        if (target1.IsNegative == false && target2.IsNegative == false)
        {
            // 正減正

            if (IsGreaterThan(target1, target2, ignoreSign: true))
            {
            }
            else
            {
                result.IsNegative = true;

                (target1, target2) = (target2, target1);
            }

            return BiggerSubtractSmaller(target1, target2, result);
        }
        else if (target1.IsNegative && target2.IsNegative)
        {
            // 負減負

            if (IsGreaterThan(target1, target2, ignoreSign: true))
            {
                result.IsNegative = true;
            }
            else
            {
                (target1, target2) = (target2, target1);
            }

            return BiggerSubtractSmaller(target1, target2, result);
        }
        else
        {
            if (target1.IsNegative && target2.IsNegative == false)
            {
                // 負減正
                result.IsNegative = true;
            }

            if (target1.IsNegative == false && target2.IsNegative)
            {
                // 正減負    
            }

            return Plus(target1, target2, result);
        }
    }

    /// <summary>
    /// 正數相加
    /// </summary>
    private static BigDecimal Plus(BigDecimal target1, BigDecimal target2, BigDecimal result)
    {
        result.PaddedValue = (char[])target1.PaddedValue.Clone();

        for (var i = result.PaddedValue.Length - 1; i >= 0; i--)
        {
            var s1Digit = result.PaddedValue[i];
            var s2Digit = target2.PaddedValue[i];

            CarryDigitPlus(result.PaddedValue, i, s1Digit, s2Digit);
        }


        result.GenerateValue();
        return result;
    }

    /// <summary>
    /// 遞迴進位處理
    /// </summary>
    private static void CarryDigitPlus(char[] result,
                                       int    index,
                                       char   s1Digit,
                                       char   s2Digit)
    {
        if (index < 0)
        {
            return;
        }

        var sumResults = _opeartorMappings.GetValueOrDefault($"{s1Digit}+{s2Digit}");
        if (sumResults == null)
        {
            throw new Exception($"{s1Digit}+{s2Digit} is not supported");
        }

        if (sumResults.Length == 1)
        {
            result[index] = sumResults[0];
        }
        else
        {
            result[index] = sumResults[1];
            CarryDigitPlus(result, index - 1, result[index - 1], '1');
        }
    }


    /// <summary>
    /// 大數減小數
    /// </summary>
    private static BigDecimal BiggerSubtractSmaller(BigDecimal target1, BigDecimal target2, BigDecimal result)
    {
        result.PaddedValue = (char[])target1.PaddedValue.Clone();

        for (var i = result.PaddedValue.Length - 1; i >= 0; i--)
        {
            var s1Digit = result.PaddedValue[i];
            var s2Digit = target2.PaddedValue[i];

            AbdicateDigitSubtract(result.PaddedValue, i, s1Digit, s2Digit);
        }

        result.GenerateValue();
        return result;
    }

    /// <summary>
    /// 遞迴借位處理
    /// </summary>
    private static bool AbdicateDigitSubtract(char[] result,
                                              int    index,
                                              char   s1Digit,
                                              char   s2Digit)
    {
        var substractResults = _opeartorMappings[$"{s1Digit}-{s2Digit}"];
        if (substractResults.Length == 1)
        {
            result[index] = substractResults[0];
            return false;
        }
        else
        {
            result[index] = substractResults[1];
            return AbdicateDigitSubtract(result, index - 1, result[index - 1], '1');
        }
    }

    private static bool IsSyncPadded(BigDecimal target1, BigDecimal target2)
    {
        return target1.PaddedValue.Length > 0
            && target2.PaddedValue.Length > 0
            && target1.PaddedValue.SequenceEqual(target2.PaddedValue)
            && target1.PaddedFloatPointIndex == target2.PaddedFloatPointIndex;
    }

    private static void SyncPadded(BigDecimal target1, BigDecimal target2)
    {
        if (IsSyncPadded(target1, target2))
        {
            return;
        }

        // 整數多一位是進位緩衝
        var integerDigits = Math.Max(target1.IntegerPart.Length, target2.IntegerPart.Length) + 1;
        var floatDigits   = Math.Max(target1.FloatPart.Length, target2.FloatPart.Length);

        // 補 0 是為了在做運算時，不需要額外判斷 index 是否有資料
        target1.Padded(integerDigits, floatDigits);
        target2.Padded(integerDigits, floatDigits);
    }

    /// <summary>
    /// target1 是否大於 target2
    /// </summary>
    public static bool IsGreaterThan(BigDecimal target1,
                                     BigDecimal target2,
                                     bool       includeEqual = false,
                                     bool       ignoreSign   = false)
    {
        // 確認已 Sync Padded
        SyncPadded(target1, target2);

        if (ignoreSign         == false
         && target1.IsNegative == false
         && target2.IsNegative)
        {
            return false;
        }

        var adjust = ignoreSign == false && target1.IsNegative && target2.IsNegative;
        var result = false;

        for (var i = 0; i < target1.PaddedValue.Length; i++)
        {
            if (target1.PaddedValue[i] == target2.PaddedValue[i])
            {
                if (includeEqual)
                {
                    result = true;
                    break;
                }

                continue;
            }

            result = (target1.PaddedValue[i] > target2.PaddedValue[i]);
            break;
        }

        return result ^ adjust;

        // 上面結果下面語法同意
        // 如果二數同為負數，則將結果反向
        // if (adjust)
        // {
        //     return !result;
        // }
        //
        // return result;
    }
}
