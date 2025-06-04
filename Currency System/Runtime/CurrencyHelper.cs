using ergulburak.SaveSystem;

namespace ergulburak.CurrencySystem
{
    public static class CurrencyHelper
    {
        private static CurrencyData _currencyData;

        public static CurrencyData CurrencyData
        {
            get
            {
                if (!_currencyData)
                    _currencyData = Currencies.GetCurrencyData();
                return _currencyData;
            }
        }

        private static CurrencySaveData GetSaveData()
        {
            var data = SaveHelper.GetData<CurrencySaveData>();
            if (data == null)
            {
                data = new CurrencySaveData();
                foreach (var info in CurrencyData.currencyList)
                    data.Currencies[info.name] = info.defaultAmount;

                data.SaveData();
            }

            return data;
        }

        public static float GetAmount(string currencyKey)
        {
            var data = GetSaveData();
            return data.Currencies.TryGetValue(currencyKey, out var amount) ? amount : 0;
        }

        public static void SetAmount(string currencyKey, int amount)
        {
            var data = GetSaveData();
            data.Currencies[currencyKey] = amount;
            data.SaveData();
        }

        public static void Add(string currencyKey, int amount) => Change(currencyKey, amount);

        public static bool Subtract(string currencyKey, int amount)
        {
            if (GetAmount(currencyKey) < amount) return false;
            Change(currencyKey, -amount);
            return true;
        }

        public static void Change(string currencyKey, int delta)
        {
            var data = GetSaveData();
            if (!data.Currencies.ContainsKey(currencyKey))
                data.Currencies[currencyKey] = 0;

            data.Currencies[currencyKey] += delta;
            data.SaveData();
        }

        public static bool TryExchange(string fromKey, string toKey, int fromAmount)
        {
            var fromInfo = Currencies.GetCurrencyInformation(fromKey);
            var rateInfo = fromInfo?.exchangeRates.Find(x => x.targetCurrencyKey == toKey);
            if (rateInfo == null) return false;

            var toAmount = (int)(fromAmount * rateInfo.rate);
            if (!Subtract(fromKey, fromAmount)) return false;

            Add(toKey, toAmount);
            return true;
        }
    }
}