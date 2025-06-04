using ergulburak.SaveSystem;

namespace ergulburak.CurrencySystem
{
    public class CurrencySaveData : ISaveable
    {
        public SerializableDictionary<string, float> Currencies = new();
    }
}