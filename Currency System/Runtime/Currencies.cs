using UnityEngine;

namespace ergulburak.CurrencySystem
{
    public static class Currencies
    {
        public static string NewCurrency1 => "NewCurrency1";
        public static string NewCurrency => "NewCurrency";

        public static CurrencyData GetCurrencyData()
        {
            return Resources.Load<CurrencyData>("CurrencySystem/CurrencyData");
        }

        public static CurrencyInformation GetCurrencyInformation(string currencyName)
        {
            var data = GetCurrencyData();
            if (data == null) return null;
            return data.currencyList.Find(c => c.name == currencyName);
        }

        public static string GetName(string currencyName)
        {
            var info = GetCurrencyInformation(currencyName);
            return info != null ? info.name : null;
        }

        public static string GetShownName(string currencyName)
        {
            var info = GetCurrencyInformation(currencyName);
            return info != null ? info.shownName : null;
        }

        public static string GetDescription(string currencyName)
        {
            var info = GetCurrencyInformation(currencyName);
            return info != null ? info.description : null;
        }

        public static string GetSymbol(string currencyName)
        {
            var info = GetCurrencyInformation(currencyName);
            return info != null ? info.symbol : null;
        }

        public static Texture GetIcon(string currencyName)
        {
            var info = GetCurrencyInformation(currencyName);
            return info != null ? info.icon : null;
        }

        public static Color GetColor(string currencyName)
        {
            var info = GetCurrencyInformation(currencyName);
            return info != null ? info.color : Color.white;
        }

        public static int GetDecimalPlaces(string currencyName)
        {
            var info = GetCurrencyInformation(currencyName);
            return info != null ? info.decimalPlaces : 0;
        }

        public static bool GetUseMaximumAmount(string currencyName)
        {
            var info = GetCurrencyInformation(currencyName);
            return info != null ? info.useMaximumAmount : false;
        }

        public static float GetMaximumAmount(string currencyName)
        {
            var info = GetCurrencyInformation(currencyName);
            return info != null ? info.maximumAmount : 0f;
        }

        public static float GetDefaultAmount(string currencyName)
        {
            var info = GetCurrencyInformation(currencyName);
            return info != null ? info.defaultAmount : 0f;
        }
    }
}
