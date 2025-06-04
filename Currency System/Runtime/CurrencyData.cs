using System.Collections.Generic;
using UnityEngine;

namespace ergulburak.CurrencySystem
{
    [CreateAssetMenu(fileName = "CurrencyData", menuName = "Data/CurrencyData")]
    public class CurrencyData : ScriptableObject
    {
        public List<CurrencyInformation> currencyList = new List<CurrencyInformation>();
    }
}