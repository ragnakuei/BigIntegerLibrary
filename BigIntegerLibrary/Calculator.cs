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
    private static Dictionary<string, char[]> _opeartorMappings = new();

    static Calculator()
    {
        foreach (var i in Enumerable.Range(0, 10))
        foreach (var j in Enumerable.Range(0, 10))
        {
            _opeartorMappings[$"{i}+{j}"] = (i + j).ToString().ToCharArray();

            if (i < j)
            {
                _opeartorMappings[$"{i}-{j}"] = new char[] { '-', (10 + i - j).ToString()[0] };
            }
            else
            {
                _opeartorMappings[$"{i}-{j}"] = (i - j).ToString().ToCharArray();
            }
        }
    }

    private static void Initialize(BigDecimal target1, BigDecimal target2)
    {
        // 整數多一位是進位緩衝
        var integerDigits = Math.Max(target1.IntegerPart.Length, target2.IntegerPart.Length) + 1;
        var floatDigits   = Math.Max(target1.FloatPart.Length, target2.FloatPart.Length);

        // 補 0 是為了在做運算時，不需要額外判斷 index 是否有資料
        target1.Padded(integerDigits, floatDigits);
        target2.Padded(integerDigits, floatDigits);
    }


    public static BigDecimal Plus(BigDecimal target1, BigDecimal target2)
    {
        Initialize(target1, target2);

        var resultIsNegative = false;

        if (!(target1.IsNegative ^ target2.IsNegative))
        {
            // 正負同號

            if (target1.IsNegative)
            {
                resultIsNegative = true;
            }

            return PositivesPlus(target1, target2, resultIsNegative);
        }
        else
        {
            // 正負不同號

            if (target1.IsGreaterThan(target2))
            {
                if (target1.IsNegative)
                {
                    resultIsNegative = true;
                }
            }
            else
            {
                if (target2.IsNegative)
                {
                    resultIsNegative = true;
                }

                (target1, target2) = (target2, target1);
            }

            return BiggerSubtractSmaller(target1, target2, resultIsNegative);
        }
    }

    public static BigDecimal Subtract(BigDecimal target1, BigDecimal target2)
    {
        Initialize(target1, target2);

        var resultIsNegative = false;

        if (target1.IsNegative == false && target2.IsNegative == false)
        {
            // 正減正

            if (target1.IsGreaterThan(target2))
            {
            }
            else
            {
                resultIsNegative = true;

                (target1, target2) = (target2, target1);
            }

            return BiggerSubtractSmaller(target1, target2, resultIsNegative);
        }
        else if (target1.IsNegative && target2.IsNegative)
        {
            // 負減負

            if (target1.IsGreaterThan(target2))
            {
                resultIsNegative = true;
            }
            else
            {
                (target1, target2) = (target2, target1);
            }

            return BiggerSubtractSmaller(target1, target2, resultIsNegative);
        }
        else
        {
            if (target1.IsNegative && target2.IsNegative == false)
            {
                // 負減正
                resultIsNegative = true;
            }

            if (target1.IsNegative == false && target2.IsNegative)
            {
                // 正減負    
            }

            return PositivesPlus(target1, target2, resultIsNegative);
        }
    }

    /// <summary>
    /// 正整數相加
    /// </summary>
    /// <param name="isNegative">預期結果是否為負數</param>
    private static BigDecimal PositivesPlus(BigDecimal target1, BigDecimal target2, bool isNegative)
    {
        var result = new BigDecimal
                     {
                         IsFloat           = target1.IsFloat || target2.IsFloat,
                         IsNegative        = isNegative,
                         IntegerPaddedPart = new char[target1.IntegerPaddedPart.Length],
                         FloatPaddedPart   = new char[target1.FloatPaddedPart.Length],
                     };

        // 加浮點數
        result.FloatPaddedPart = PositivesPlus(target1.FloatPaddedPart, target2.FloatPaddedPart, out var floatIsCarry);

        // 加整數
        result.IntegerPaddedPart = PositivesPlus(target1.IntegerPaddedPart, target2.IntegerPaddedPart, out _);

        // 浮點數進位至整數的處理
        if (floatIsCarry)
        {
            var integerCarryPaddedPart = new char[result.IntegerPaddedPart.Length];
            integerCarryPaddedPart[result.IntegerPaddedPart.Length - 1] = '1';

            result.IntegerPaddedPart = PositivesPlus(result.IntegerPaddedPart, integerCarryPaddedPart, out _);
        }

        result.GenerateValue();
        return result;
    }

    /// <summary>
    /// char[] 相加，從最低位數 (右邊) 開始加
    /// </summary>
    /// <param name="isCarry">最高位數是否需要進位</param>
    /// <returns></returns>
    private static char[] PositivesPlus(char[] target1, char[] target2, out bool isCarry)
    {
        var result = target1.Clone() as char[];
        isCarry = false;

        for (var i = target1.Length - 1; i >= 0; i--)
        {
            var s1Digit = result[i];
            var s2Digit = target2[i];

            isCarry = CarryDigit(result, i, s1Digit, s2Digit);
        }

        return result;
    }


    /// <summary>
    /// 大數減小數
    /// </summary>
    /// <param name="isNegative">預期結果是否為負數</param>
    private static BigDecimal BiggerSubtractSmaller(BigDecimal bigger, BigDecimal smaller, bool isNegative)
    {
        var result = new BigDecimal
                     {
                         IsFloat           = bigger.IsFloat || smaller.IsFloat,
                         IsNegative        = isNegative,
                         IntegerPaddedPart = new char[bigger.IntegerPaddedPart.Length],
                         FloatPaddedPart   = new char[bigger.FloatPaddedPart.Length],
                     };

        // 減浮點數 - 忽略浮點數要大數減小數的處理
        result.FloatPaddedPart = Subtract(bigger.FloatPaddedPart, smaller.FloatPaddedPart, out var floatIsAbdicate);

        // 減整數 - 要大數減小數
        result.IntegerPaddedPart = Subtract(bigger.IntegerPaddedPart, smaller.IntegerPaddedPart, out _);

        // 浮點數相減後，跟整數借位的處理
        if (floatIsAbdicate)
        {
            var integerCarryPaddedPart = new char[result.IntegerPaddedPart.Length];
            integerCarryPaddedPart[result.IntegerPaddedPart.Length - 1] = '1';

            result.IntegerPaddedPart = Subtract(result.IntegerPaddedPart, integerCarryPaddedPart, out _);
        }

        result.GenerateValue();
        return result;
    }

    private static char[] Subtract(char[] target1, char[] target2, out bool isAbdicate)
    {
        var result = target1.Clone() as char[];
        isAbdicate = false;

        for (var i = 0; i < target1.Length; i++)
        {
            var substractResults = _opeartorMappings[$"{result[i]}-{target2[i]}"];
            if (substractResults.Length == 1)
            {
                result[i] = substractResults[0];
            }
            else
            {
                result[i]  = substractResults[1];
                isAbdicate = AbdicateDigit(result, i - 1);
            }
        }

        return result;
    }

    /// <summary>
    /// 遞迴進位處理
    /// <returns>最高位數是否有進位</returns>
    /// </summary>
    private static bool CarryDigit(char[] result,
                                   int    index,
                                   char   s1Digit,
                                   char   s2Digit)
    {
        if (index < 0)
        {
            return false;
        }

        var sumResults = _opeartorMappings.GetValueOrDefault($"{s1Digit}+{s2Digit}");
        if (sumResults.Length == 1)
        {
            result[index] = sumResults[0];
            return false;
        }
        else
        {
            result[index] = sumResults[1];

            return CarryDigit(result, index - 1, result[index - 1], '1');
        }
    }

    /// <summary>
    /// 遞迴退位處理
    /// </summary>
    private static bool AbdicateDigit(char[] result, int index)
    {
        var substractResults = _opeartorMappings[$"{result[index]}-1"];
        if (substractResults.Length == 1)
        {
            result[index] = substractResults[0];
            return false;
        }
        else
        {
            result[index] = substractResults[1];
            return AbdicateDigit(result, index - 1);
        }
    }
}
