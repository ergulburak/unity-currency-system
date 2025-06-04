using System.Collections.Generic;
using UnityEngine;
using System;

namespace ergulburak.CurrencySystem
{
    [Serializable]
    public class CurrencyInformation
    {
        public string name;
        public string shownName;
        public string description;
        public string symbol;
        public Texture icon;
        public Color color;
        public float defaultAmount;
        public int decimalPlaces;
        public bool useMaximumAmount;
        public float maximumAmount;
        public List<CurrencyExchangeRate> exchangeRates = new();
    }
}