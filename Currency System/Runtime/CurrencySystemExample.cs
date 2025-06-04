using ergulburak.SaveSystem;
using UnityEngine;

namespace ergulburak.CurrencySystem
{
    public class CurrencySystemExample : MonoBehaviour
    {
        [Header("Exchange Amount")] public int exchangeAmount = 10;

        private void Awake()
        {
            SaveHelper.OnInitializeComplete += CurrencyExample;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) CurrencyExample(0);
        }

        private void CurrencyExample(int _)
        {
            Debug.Log("<color=orange>--- Currency System Example Start ---</color>");

            var amountA = CurrencyHelper.GetAmount(Currencies.NewCurrency);
            var amountB = CurrencyHelper.GetAmount(Currencies.NewCurrency1);

            Debug.Log($"Initial {Currencies.NewCurrency}: {amountA}");
            Debug.Log($"Initial {Currencies.NewCurrency1}: {amountB}");

            CurrencyHelper.Add(Currencies.NewCurrency, 50);
            Debug.Log($"Added 50 to {Currencies.NewCurrency}: {CurrencyHelper.GetAmount(Currencies.NewCurrency)}");

            bool subtracted = CurrencyHelper.Subtract(Currencies.NewCurrency, 20);
            Debug.Log(
                $"Subtracted 20 from {Currencies.NewCurrency}: {subtracted}, New Amount: {CurrencyHelper.GetAmount(Currencies.NewCurrency)}");

            subtracted = CurrencyHelper.Subtract(Currencies.NewCurrency, 9999);
            Debug.Log(
                $"Tried to subtract 9999 from {Currencies.NewCurrency}: {subtracted}, Amount stays: {CurrencyHelper.GetAmount(Currencies.NewCurrency)}");

            bool exchanged =
                CurrencyHelper.TryExchange(Currencies.NewCurrency, Currencies.NewCurrency1, exchangeAmount);
            if (exchanged)
            {
                Debug.Log($"Exchanged {exchangeAmount} {Currencies.NewCurrency} to {Currencies.NewCurrency1}");
                Debug.Log($"New {Currencies.NewCurrency}: {CurrencyHelper.GetAmount(Currencies.NewCurrency)}");
                Debug.Log($"New {Currencies.NewCurrency1}: {CurrencyHelper.GetAmount(Currencies.NewCurrency1)}");
            }
            else
            {
                Debug.LogWarning(
                    $"Exchange failed: {Currencies.NewCurrency} ➡ {Currencies.NewCurrency1} ({exchangeAmount})");
            }

            Debug.Log("<color=orange>--- Currency System Example End ---</color>");
        }
    }
}