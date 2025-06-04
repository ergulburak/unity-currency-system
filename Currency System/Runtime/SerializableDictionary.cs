using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using System;

namespace ergulburak.CurrencySystem
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new();
        [SerializeField] private List<TValue> values = new();

        private Dictionary<TKey, TValue> dictionary = new();

        public void OnAfterDeserialize()
        {
            dictionary = new Dictionary<TKey, TValue>();
            for (int i = 0; i < Math.Min(keys.Count, values.Count); i++)
            {
                if (!dictionary.ContainsKey(keys[i]))
                    dictionary.Add(keys[i], values[i]);
            }
        }

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (var pair in dictionary)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        public TValue this[TKey key]
        {
            get => dictionary[key];
            set => dictionary[key] = value;
        }

        public ICollection<TKey> Keys => dictionary.Keys;
        public ICollection<TValue> Values => dictionary.Values;
        public int Count => dictionary.Count;
        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value) => dictionary.Add(key, value);
        public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);
        public bool Remove(TKey key) => dictionary.Remove(key);
        public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);
        public void Add(KeyValuePair<TKey, TValue> item) => dictionary.Add(item.Key, item.Value);
        public void Clear() => dictionary.Clear();
        public bool Contains(KeyValuePair<TKey, TValue> item) => dictionary.Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            ((IDictionary<TKey, TValue>)dictionary).CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<TKey, TValue> item) => dictionary.Remove(item.Key);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}