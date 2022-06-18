namespace BigIntegerLibrary;

public class BigDecimal
{
    public BigDecimal()
    {
    }

    private BigDecimal(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
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

    public BigDecimal(BigDecimal target1, BigDecimal target2)
    {
        IsNegative            =   false;
        IsFloat               =   target1.IsFloat || target2.IsFloat;
        PaddedFloatPointIndex ??= target1.PaddedFloatPointIndex ?? target2.PaddedFloatPointIndex;
    }

    /// <summary>
    /// 原始資料
    /// </summary>
    private string Value { get; set; }

    public bool IsNegative { get; internal set; }

    public bool IsFloat { get; internal set; }

    public char[] IntegerPart { get; }

    public char[] FloatPart { get; }

    /// <summary>
    /// Padded 後的小數點 index
    /// </summary>
    public int? PaddedFloatPointIndex { get; private set; }

    /// <summary>
    /// Value Padded 後的資料，主要用於計算
    /// </summary>
    public char[] PaddedValue { get; internal set; } = Array.Empty<char>();

    /// <summary>
    /// 補 0
    /// </summary>
    internal void Padded(int integerDigits, int floatDigits)
    {
        var integerPaddedPart = new string(IntegerPart).PadLeft(integerDigits, '0');
        var floatPaddedPart   = new string(FloatPart).PadRight(floatDigits, '0');

        PaddedFloatPointIndex = integerPaddedPart.Length;

        PaddedValue = (integerPaddedPart + floatPaddedPart).ToCharArray();
    }

    #region Operators

    public static implicit operator BigDecimal(string s) => new(s);
    public static implicit operator string(BigDecimal s) => s.Value;

    public static BigDecimal operator +(BigDecimal obj1, BigDecimal obj2) => Calculator.Plus(obj1, obj2);
    public static BigDecimal operator -(BigDecimal obj1, BigDecimal obj2) => Calculator.Subtract(obj1, obj2);
    public static bool operator ==(BigDecimal      obj1, BigDecimal obj2) => obj1 == obj2;
    public static bool operator !=(BigDecimal      obj1, BigDecimal obj2) => obj1 != obj2;
    public static bool operator >=(BigDecimal      obj1, BigDecimal obj2) => Calculator.IsGreaterThan(obj1, obj2, true);
    public static bool operator <=(BigDecimal      obj1, BigDecimal obj2) => Calculator.IsGreaterThan(obj1, obj2, true) == false;
    public static bool operator >(BigDecimal       obj1, BigDecimal obj2) => Calculator.IsGreaterThan(obj1, obj2);
    public static bool operator <(BigDecimal       obj1, BigDecimal obj2) => Calculator.IsGreaterThan(obj1, obj2) == false;

    #endregion

    public void GenerateValue()
    {
        var integerPart = PaddedValue.Take(PaddedFloatPointIndex.GetValueOrDefault()).ToArray();
        var floatPart   = PaddedValue.Skip(PaddedFloatPointIndex.GetValueOrDefault()).ToArray();

        IsFloat = floatPart.Any(c => c != '0');

        Value = string.Format("{0}{1}{2}",
                              IsNegative ? "-" : string.Empty,
                              new string(integerPart).TrimStart('0'),
                              IsFloat ? "." + new string(floatPart).TrimEnd('0') : string.Empty);

        if (string.IsNullOrWhiteSpace(Value) || Value == "-")
        {
            IsFloat    = false;
            IsNegative = false;
            Value      = "0";
        }
    }
}
