namespace BigIntegerLibrary;

public class BigDecimal
{
    public BigDecimal()
    {
    }

    private BigDecimal(string value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        Value = value;

        if (value[0] == '-')
        {
            IsNegative = true;
            value      = value.Substring(1); // 去掉正負號
        }

        IsFloat = value.Contains('.');

        var integerPart = string.Empty;
        var floatPart   = string.Empty;

        // 拆分 整數 與 小數 部份

        if (IsFloat)
        {
            var split = value.Split('.');
            integerPart = split[0];
            floatPart   = split[1];
        }
        else
        {
            integerPart = value;
            floatPart   = "";
        }

        IntegerPart = integerPart.TrimStart('0').ToCharArray();
        FloatPart   = floatPart.TrimEnd('0').ToCharArray();
    }

    /// <summary>
    /// 原始資料
    /// </summary>
    private string Value { get; set; }

    public bool IsNegative { get; init; }

    public bool IsFloat { get; init; }

    public char[] IntegerPart { get; }

    /// <summary>
    /// 補 0 到指定位數
    /// </summary>
    public char[] IntegerPaddedPart { get; internal set; }

    public char[] FloatPart { get; }

    /// <summary>
    /// 補 0 到指定位數
    /// </summary>
    public char[] FloatPaddedPart { get; internal set; }

    /// <summary>
    /// 補 0 到指定位數 
    /// </summary>
    public void Padded(int integerDigits, int floatDigits)
    {
        IntegerPaddedPart = new string(IntegerPart).PadLeft(integerDigits, '0').ToCharArray();
        FloatPaddedPart   = new string(FloatPart).PadRight(floatDigits, '0').ToCharArray();
    }

    #region Operators

    public static implicit operator BigDecimal(string s) => new(s);
    public static implicit operator string(BigDecimal s) => s.Value;

    public static BigDecimal operator +(BigDecimal obj1, BigDecimal obj2) => Calculator.Plus(obj1, obj2);
    public static BigDecimal operator -(BigDecimal obj1, BigDecimal obj2) => Calculator.Subtract(obj1, obj2);
    public static bool operator ==(BigDecimal      obj1, BigDecimal obj2) => obj1 == obj2;
    public static bool operator !=(BigDecimal      obj1, BigDecimal obj2) => obj1 != obj2;
    public static bool operator >=(BigDecimal      obj1, BigDecimal obj2) => obj1.IsGreaterThan(obj2, true);
    public static bool operator <=(BigDecimal      obj1, BigDecimal obj2) => obj1.IsGreaterThan(obj2, true) == false;
    public static bool operator >(BigDecimal       obj1, BigDecimal obj2) => obj1.IsGreaterThan(obj2);
    public static bool operator <(BigDecimal       obj1, BigDecimal obj2) => obj1.IsGreaterThan(obj2) == false;

    #endregion

    /// <summary>
    /// target1 是否大於 target2
    /// </summary>
    public bool IsGreaterThan(BigDecimal target, bool containsEquals = false)
    {
        // 整數部份
        for (var i = 0; i < this.IntegerPaddedPart.Length; i++)
        {
            if (this.IntegerPaddedPart[i] == target.IntegerPaddedPart[i])
            {
                if (containsEquals)
                {
                    return true;
                }

                continue;
            }

            return this.IntegerPaddedPart[i] > target.IntegerPaddedPart[i];
        }

        // 小數數部份
        for (var i = 0; i < this.FloatPaddedPart.Length; i++)
        {
            if (this.FloatPaddedPart[i] == target.FloatPaddedPart[i])
            {
                if (containsEquals)
                {
                    return true;
                }

                continue;
            }

            return this.FloatPaddedPart[i] > target.FloatPaddedPart[i];
        }

        return false;
    }

    public void GenerateValue()
    {
        Value = string.Format("{0}{1}",
                              IsNegative ? "-" : "",
                              new string(IntegerPaddedPart).TrimStart('0'));
        if (IsFloat)
        {
            Value = string.Format("{0}.{1}",
                                  Value,
                                  new string(FloatPaddedPart).TrimEnd('0'));
        }

        if (Value.EndsWith("."))
        {
            Value = Value.Substring(0, Value.Length - 1);
        }

        if (Value == "-0")
        {
            Value = "0";
        }
    }
}
