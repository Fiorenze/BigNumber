using System;
using UnityEngine;
[Serializable]
public struct BigNumber
{
    public double Amount;
    public int Exp;
    const int TEN_CUBED = 1000;

    // Find the index for the maximum suffix "ga". Change if needed
    // Make sure the suffix is also included in NumberTypes.types
    private static readonly int MaxExponent = NumberTypes.stringToIndex("ga");
    // Define the maximum amount allowed at the maximum exponent, e.g. 999ga
    private static readonly double MaxAllowedAmountAtMaxExponent = 999.0;


    public BigNumber(double amount, int exp)
    {
        Amount = amount;
        Exp = exp;
        normalize();
    }

    // Clamps the current BigNumber instance to the defined maximum value
    private void ClampToMax()
    {
        // Only clamp if MaxExponent was successfully found
        if (MaxExponent < 0) return;

        // If exponent exceeds the max, clamp both exponent and amount
        if (this.Exp > MaxExponent)
        {
            this.Exp = MaxExponent;
            this.Amount = MaxAllowedAmountAtMaxExponent;
        }
        // If exponent is at the max, clamp amount if it exceeds the allowed limit
        else if (this.Exp == MaxExponent && this.Amount > MaxAllowedAmountAtMaxExponent)
        {
            this.Amount = MaxAllowedAmountAtMaxExponent;
        }
    }

    void normalize()
    {
        if (this.Amount < 0)
        {
            this.Exp = 0;
            this.Amount = 0;
            return;
        }
        if (this.Amount < 1 && this.Exp > 0)
        {
            // e.g. 0.1E6 is converted to 100E3 ([0.1, 6] = [100, 3])
            while (this.Amount < 1 && this.Exp > 0)
            {
                this.Amount *= TEN_CUBED;
                this.Exp -= 1;
            }
            // If normalization resulted in zero (e.g., extremely small initial value), reset.
            if (this.Amount <= 0) { this.Exp = 0; this.Amount = 0; return; }
        }
        else if (this.Amount >= TEN_CUBED)
        {
            // e.g. 10000E3 is converted to 10E6 ([10000, 3] = [10, 6])
            while (this.Amount >= TEN_CUBED)
            {
                this.Amount *= 1d / TEN_CUBED;
                this.Exp += 1;
            }
        }

        ClampToMax();

        if (this.Amount <= 0 && this.Exp != 0)
        {
            this.Exp = 0;
            this.Amount = 0;
        }
    }
    void align(int exp)
    {
        int diff = exp - Exp;
        if (diff > 0)
        {
            Amount /= Math.Pow(TEN_CUBED, diff);
            Exp = exp;
        }
    }

    #region Math Operations
    public void add(BigNumber bigNumber)
    {
        if (bigNumber.Exp < this.Exp)
        {
            bigNumber.align(Exp);
        }
        else
        {
            align(bigNumber.Exp);
        }
        this.Amount += bigNumber.Amount;
        normalize();
    }
    public void add(float number)
    {
        BigNumber numberToAdd = BigNumber.ToBigNumber(number);
        add(numberToAdd);
    }
    public void subtract(BigNumber bigNumber)
    {
        if (bigNumber.Exp < this.Exp)
        {
            bigNumber.align(this.Exp);
        }
        else
        {
            align(bigNumber.Exp);
        }
        this.Amount -= bigNumber.Amount;
        normalize();
    }
    public void subtract(float number)
    {
        BigNumber numberToSubtract = BigNumber.ToBigNumber(number);
        subtract(numberToSubtract);
    }
    public void multiply(float factor)
    {
        if (factor >= 0)
        {
            this.Amount *= factor;
            normalize();
        }
    }
    public void multiply(BigNumber bigNumber)
    {
        this.Exp += bigNumber.Exp;
        this.Amount *= bigNumber.Amount;
        normalize();
    }
    public void divide(BigNumber bigNumber)
    {
        this.Exp -= bigNumber.Exp;
        this.Amount /= bigNumber.Amount;
        normalize();
    }
    public void divide(float factor)
    {
        if (factor > 0)
        {
            this.Amount /= factor;
            normalize();
        }
    }
    #endregion

    #region Operator Overloading
    public static BigNumber operator +(BigNumber number1, BigNumber number2)
    {
        number1.add(number2);
        return number1;
    }
    public static BigNumber operator +(BigNumber number1, float number2)
    {
        number1.add(number2);
        return number1;
    }
    public static BigNumber operator -(BigNumber number1, BigNumber number2)
    {
        number1.subtract(number2);
        return number1;
    }
    public static BigNumber operator -(BigNumber number1, float number2)
    {
        number1.subtract(number2);
        return number1;
    }
    public static BigNumber operator *(BigNumber number1, float factor)
    {
        number1.multiply(factor);
        return number1;
    }
    public static BigNumber operator *(BigNumber number1, BigNumber number2)
    {
        number1.multiply(number2);
        return number1;
    }
    public static BigNumber operator /(BigNumber number1, BigNumber number2)
    {
        number1.divide(number2);
        return number1;
    }
    public static BigNumber operator /(BigNumber number1, float factor)
    {
        number1.divide(factor);
        return number1;
    }

    public static bool operator <(BigNumber number1, BigNumber number2)
    {
        if (number1.Exp < number2.Exp) return true;
        if (number1.Exp == number2.Exp && number1.Amount < number2.Amount) return true;
        return false;
    }
    public static bool operator >(BigNumber number1, BigNumber number2)
    {
        if (number1.Exp > number2.Exp) return true;
        if (number1.Exp == number2.Exp && number1.Amount > number2.Amount) return true;
        return false;
    }
    public static bool operator <=(BigNumber number1, BigNumber number2)
    {
        return !(number1 > number2);
    }
    public static bool operator >=(BigNumber number1, BigNumber number2)
    {
        return !(number1 < number2);
    }
    public static bool operator ==(BigNumber number1, BigNumber number2)
    {
        // Normalize ensures representation is consistent, so direct comparison might be okay.
        // But we consider a small tolerance (epsilon)
        return number1.Exp == number2.Exp && Math.Abs(number1.Amount - number2.Amount) < double.Epsilon; // Basic equality
    }
    public static bool operator !=(BigNumber number1, BigNumber number2)
    {
        return !(number1 == number2);
    }
    #endregion

    public override bool Equals(object obj)
    {
        return obj is BigNumber other && this == other;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Exp);
    }
    public override string ToString()
    {
        string numberString = string.Empty;
        numberString += Amount.ToString("F2");
        numberString += NumberTypes.indexToString(Exp);
        return numberString;
    }
    public static BigNumber ToBigNumber(double number)
    {
        BigNumber newNumber = new BigNumber(number, 0);
        return newNumber;
    }
    public static BigNumber zero()
    {
        return new BigNumber(0, 0);
    }

    public static bool TryParse(string bigNumberString, out BigNumber result)
    {
        result = default;
        if (string.IsNullOrEmpty(bigNumberString))
        {
            Debug.LogError("Cannot parse null or empty string.");
            return false;
        }
        int suffixStartIndex = -1;

        for (int i = 0; i < bigNumberString.Length; i++)
        {
            // Iterates until it finds a char that is not a part of the number and keep the index
            if (!char.IsDigit(bigNumberString[i]) && bigNumberString[i] != '.' && bigNumberString[i] != ',')
            {
                if (i == 0)
                {
                    Debug.LogError("Invalid BigNumber");
                    return false;
                }
                suffixStartIndex = i;
                break;
            }
        }
        // Suffix is the rest of the string after the index we found
        string suffixString = bigNumberString[suffixStartIndex..];
        int exp = NumberTypes.stringToIndex(suffixString);
        // Number is the part of the string before the index we found
        if (!double.TryParse(bigNumberString[..suffixStartIndex], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double amount))
        {
            Debug.LogError($"Invalid BigNumber amount format");
            return false;
        }
        result = new BigNumber(amount, exp);
        return true;
    }
}
public static class NumberTypes
{
    public static string[] types =
    {
        "", "K", "M", "B", "T", "aa", "ab", "ac", "ad","ae", "af", "ag","ah", "ai", "aj","ak", "al", "am","an", "ao",
        "ap","aq", "ar", "as","at", "au", "av", "aw", "ax", "ay","az", "ba", "bb", "bc", "bd", "be","bf", "bg","bh", "bi","bj", "bk",
        "bl", "bm","bn", "bo","bp", "bq","br", "bs","bt", "bu","bw", "bx","by", "bz", "ca", "cb", "cc","cd", "ce", "cf","cg", "ch", "ci","cj", "ck", "cl",
        "cm", "cn", "co","cp", "cq", "cr","cs", "ct", "cu","cv", "cw", "cx","cy", "cz", "da", "db", "dc","dd", "de", "df","dg", "dh", "di","dj", "dk", "dl",
        "dm", "dn", "do","dp", "dq", "dr","ds", "dt", "du","dv", "dw", "dx","dy", "dz", "ea", "eb", "ec","ed", "ee", "ef","eg", "eh", "ei","ej", "ek", "el",
        "em", "en", "eo","ep", "eq", "er","es", "et", "eu","ev", "ew", "ex","ey", "ez", "fa", "fb", "fc","fd", "fe", "ff","fg", "fh", "fi","fj", "fk", "fl",
        "fm", "fn", "fo","fp", "fq", "fr","fs", "ft", "fu","fv", "fw", "fx","fy", "fz", "ga"
        // Add more if needed
    };

    public static string indexToString(int index)
    {
        if (index < 0 || index >= types.Length)
        {
            return string.Empty;
        }
        return types[index];
    }
    public static int stringToIndex(string str)
    {
        return Array.IndexOf(types, str);
    }
}

