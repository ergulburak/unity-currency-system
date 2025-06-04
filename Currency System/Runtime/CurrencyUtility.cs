using System;

namespace ergulburak.CurrencySystem
{
    public static class CurrencyUtility
    {
        public static string ToStringNumber(this float number, int decimalPlaces = 0,
            MidpointRounding roundingMode = MidpointRounding.AwayFromZero)
        {
            float roundedNumber = (float)Math.Round(number, decimalPlaces, roundingMode);
            return roundedNumber.ToString("F" + decimalPlaces);
        }
    }
}