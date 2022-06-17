namespace BigIntegerLibrary;

public class BigInteger
{
    private readonly char[] _value;

    private BigInteger(string value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        _value = value.ToCharArray();
    }

    private string Value => new string(_value);

    #region Operators

    public static implicit operator BigInteger(string s) => new(s);

    public static implicit operator string(BigInteger d) => d.Value;

    public static string operator +(BigInteger obj1, BigInteger obj2) => Calculator.Plus(obj1.Value, obj2.Value);
    public static string operator -(BigInteger obj1, BigInteger obj2) => Calculator.Subtract(obj1.Value, obj2.Value);
    public static bool operator ==(BigInteger  obj1, BigInteger obj2) => obj1.Value == obj2.Value;
    public static bool operator !=(BigInteger  obj1, BigInteger obj2) => obj1.Value != obj2.Value;
    public static bool operator >=(BigInteger  obj1, BigInteger obj2) => Calculator.OneIsBigger(obj1._value, obj2._value, true);
    public static bool operator <=(BigInteger  obj1, BigInteger obj2) => Calculator.OneIsBigger(obj1._value, obj2._value, true) == false;
    public static bool operator >(BigInteger   obj1, BigInteger obj2) => Calculator.OneIsBigger(obj1._value, obj2._value);
    public static bool operator <(BigInteger   obj1, BigInteger obj2) => Calculator.OneIsBigger(obj1._value, obj2._value) == false;

    #endregion

    private static class Calculator
    {
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

        private static bool   _s1Negative     = false;
        private static bool   _s2Negative     = false;
        private static int    _maxDigits      = 0;
        private static char[] _s1TargetPadded = Array.Empty<char>();
        private static char[] _s2TargetPadded = Array.Empty<char>();

        private static void Initialize(string s1, string s2)
        {
            _s1Negative = s1.StartsWith("-");
            _s2Negative = s2.StartsWith("-");

            var s1Target = s1.TrimStart('-', '0');
            var s2Target = s2.TrimStart('-', '0');

            // 多一位是進位緩衝
            var maxDigits = Math.Max(s1Target.Length, s2Target.Length) + 1;
            _s1TargetPadded = s1Target.PadLeft(maxDigits, '0').ToCharArray();
            _s2TargetPadded = s2Target.PadLeft(maxDigits, '0').ToCharArray();
        }


        public static string Plus(string s1, string s2)
        {
            Initialize(s1, s2);

            var resultPrefix = string.Empty;

            if (!(_s1Negative ^ _s2Negative))
            {
                // 正負同號

                if (_s1Negative)
                {
                    resultPrefix = "-";
                }

                return resultPrefix + PositivesPlus(_s1TargetPadded, _s2TargetPadded);
            }
            else
            {
                // 正負不同號

                if (OneIsBigger(_s1TargetPadded, _s2TargetPadded))
                {
                    if (_s1Negative)
                    {
                        resultPrefix = "-";
                    }
                }
                else
                {
                    if (_s2Negative)
                    {
                        resultPrefix = "-";
                    }

                    (_s1TargetPadded, _s2TargetPadded) = (_s2TargetPadded, _s1TargetPadded);
                }

                return resultPrefix + BiggerSubtractSmaller(_s1TargetPadded, _s2TargetPadded);
            }
        }

        public static string Subtract(string s1, string s2)
        {
            Initialize(s1, s2);

            var resultPrefix = string.Empty;

            if (_s1Negative == false && _s2Negative == false)
            {
                // 正減正

                if (OneIsBigger(_s1TargetPadded, _s2TargetPadded))
                {
                }
                else
                {
                    resultPrefix = "-";

                    (_s1TargetPadded, _s2TargetPadded) = (_s2TargetPadded, _s1TargetPadded);
                }

                return resultPrefix + BiggerSubtractSmaller(_s1TargetPadded, _s2TargetPadded);
            }
            else if (_s1Negative && _s2Negative)
            {
                // 負減負

                if (OneIsBigger(_s1TargetPadded, _s2TargetPadded))
                {
                    resultPrefix = "-";
                }
                else
                {
                    (_s1TargetPadded, _s2TargetPadded) = (_s2TargetPadded, _s1TargetPadded);
                }

                return resultPrefix + BiggerSubtractSmaller(_s1TargetPadded, _s2TargetPadded);
            }
            else
            {
                if (_s1Negative && _s2Negative == false)
                {
                    // 負減正
                    resultPrefix = "-";
                }

                if (_s1Negative == false && _s2Negative)
                {
                    // 正減負    
                }

                return resultPrefix + PositivesPlus(_s1TargetPadded, _s2TargetPadded);
            }
        }

        /// <summary>
        /// 第一個數字大於第二個數字
        /// <remarks>二個陣列同長度</remarks>
        /// </summary>
        public static bool OneIsBigger(char[] s1TargetPadded, char[] s2TargetPadded, bool containsEquals = false)
        {
            for (var i = 0; i < s1TargetPadded.Length; i++)
            {
                if (s1TargetPadded[i] == s2TargetPadded[i])
                {
                    if (containsEquals)
                    {
                        return true;
                    }

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
                var substractResults = _opeartorMappings[$"{bigger[i]}-{smaller[i]}"];
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

            var sumResults = _opeartorMappings.GetValueOrDefault($"{result[index]}+{plugNumber}");
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
            var substractResults = _opeartorMappings[$"{result[index]}-1"];
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
    }
}
