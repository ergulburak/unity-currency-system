using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ergulburak.CurrencySystem.Editor
{
    public class CurrencyConfigWindow : EditorWindow
    {
        private List<string> allCurrencyKeys;
        private CurrencyInformation currency;
        private Vector2 scrollPosition;
        private GUIStyle headerStyle;
        private GUIStyle textFieldStyle;

        public static void ShowWindow(CurrencyInformation currency)
        {
            var window = GetWindow<CurrencyConfigWindow>("Configure Currency");
            window.currency = currency;
            window.minSize = new Vector2(400, 650);
            window.maxSize = new Vector2(400, 650);
        }

        private void OnEnable()
        {
            allCurrencyKeys = Currencies.GetCurrencyData().currencyList.Select(c => c.name).ToList();
            headerStyle ??= new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 14,
                margin = new RectOffset(10, 10, 10, 10)
            };

            textFieldStyle ??= new GUIStyle(GUI.skin.textField)
            {
                fontSize = 12,
                padding = new RectOffset(5, 5, 5, 5),
                margin = new RectOffset(10, 10, 5, 5)
            };
        }

        private void OnGUI()
        {
            if (currency == null)
            {
                EditorGUILayout.HelpBox("Currency data is missing!", MessageType.Error);
                return;
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.LabelField("Currency Configuration", headerStyle);
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Shown Name");
            currency.shownName = EditorGUILayout.TextField(currency.shownName, textFieldStyle, GUILayout.Height(25));

            EditorGUILayout.LabelField("Description");
            currency.description = EditorGUILayout.TextArea(currency.description, textFieldStyle, GUILayout.Height(60));

            EditorGUILayout.LabelField("Symbol");
            currency.symbol = EditorGUILayout.TextField(currency.symbol, textFieldStyle, GUILayout.Height(25));

            EditorGUILayout.LabelField("Icon");
            currency.icon = (Texture)EditorGUILayout.ObjectField(currency.icon, typeof(Texture), false);

            EditorGUILayout.LabelField("Color");
            currency.color = EditorGUILayout.ColorField(currency.color);

            EditorGUILayout.LabelField("Default Amount");
            currency.defaultAmount = EditorGUILayout.FloatField(currency.defaultAmount);

            EditorGUILayout.LabelField("Decimal Places");
            currency.decimalPlaces = EditorGUILayout.IntField(currency.decimalPlaces);

            EditorGUILayout.LabelField("Use Maximum Amount");
            currency.useMaximumAmount = EditorGUILayout.Toggle(currency.useMaximumAmount);

            EditorGUILayout.LabelField("Maximum Amount");
            currency.maximumAmount = EditorGUILayout.FloatField(currency.maximumAmount);

            EditorGUILayout.Space(15);
            EditorGUILayout.LabelField("Exchange Rates", headerStyle);

            int removeIndex = -1;

            for (int i = 0; i < currency.exchangeRates.Count; i++)
            {
                var exchange = currency.exchangeRates[i];
                EditorGUILayout.BeginHorizontal();

                var validKeys = allCurrencyKeys.Where(k => k != currency.name).ToArray();
                var selectedIndex = Mathf.Max(0, System.Array.IndexOf(validKeys, exchange.targetCurrencyKey));
                var newIndex = EditorGUILayout.Popup(selectedIndex, validKeys);

                if (validKeys.Length > 0 && newIndex >= 0)
                    exchange.targetCurrencyKey = validKeys[newIndex];

                exchange.rate = EditorGUILayout.FloatField(exchange.rate);

                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    removeIndex = i;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (removeIndex >= 0)
            {
                currency.exchangeRates.RemoveAt(removeIndex);
            }

            if (GUILayout.Button("Add Exchange Rate", GUILayout.Height(30)))
            {
                var existingKeys = currency.exchangeRates.Select(x => x.targetCurrencyKey).ToHashSet();
                var validKeys = allCurrencyKeys.Where(k => k != currency.name && !existingKeys.Contains(k)).ToList();

                if (validKeys.Count > 0)
                {
                    currency.exchangeRates.Add(new CurrencyExchangeRate
                    {
                        targetCurrencyKey = validKeys[0],
                        rate = 1f
                    });
                }
                else
                {
                    EditorUtility.DisplayDialog("No Available Currencies", "All valid currencies already have exchange rates defined.", "OK");
                }
            }

            EditorGUILayout.Space(10);
            if (GUILayout.Button("Apply", GUILayout.Height(40)))
            {
                EditorUtility.SetDirty(Resources.Load<CurrencyData>(path: nameof(CurrencySystem) + "/CurrencyData"));
                Close();
            }

            EditorGUILayout.EndScrollView();
        }
    }
}