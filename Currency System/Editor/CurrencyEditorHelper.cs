using System.Text.RegularExpressions;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace ergulburak.CurrencySystem.Editor
{
    public static class CurrencyEditorHelper
    {
        private const string currenciesPath = "Packages/com.ergulburak.currency-system/Runtime/Currencies.cs";
        private const string currencyDataPath = "Assets/Resources/CurrencySystem/CurrencyData.asset";

        public static void CheckCurrencyData(out CurrencyData currencyData)
        {
            currencyData = AssetDatabase.LoadAssetAtPath<CurrencyData>(currencyDataPath);

            if (currencyData != null) return;

            Debug.LogWarning("CurrencyData not found in Resources folder. Creating a new one...");

            string resourcesPath = "Assets/Resources";
            string currencySystemPath = "Assets/Resources/CurrencySystem";

            if (!AssetDatabase.IsValidFolder(resourcesPath))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }

            if (!AssetDatabase.IsValidFolder(currencySystemPath))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "CurrencySystem");
            }

            currencyData = ScriptableObject.CreateInstance<CurrencyData>();
            AssetDatabase.CreateAsset(currencyData, currencyDataPath);
            AssetDatabase.SaveAssets();
        }

        public static void UpdateCurrencies()
        {
            var currencyData = AssetDatabase.LoadAssetAtPath<CurrencyData>(currencyDataPath);
            if (currencyData == null) return;

            string code = "using UnityEngine;\n\n";
            code += "namespace ergulburak.CurrencySystem\n{\n";
            code += "    public static class Currencies\n    {\n";

            foreach (var currency in currencyData.currencyList)
            {
                string normalized = NormalizeCurrencyName(currency.name);
                code += $"        public static string {normalized} => \"{currency.name}\";\n";
            }

            code += "\n        public static CurrencyData GetCurrencyData()\n        {\n";
            code += "            return Resources.Load<CurrencyData>(\"CurrencySystem/CurrencyData\");\n";
            code += "        }\n\n";

            code +=
                "        public static CurrencyInformation GetCurrencyInformation(string currencyName)\n        {\n";
            code += "            var data = GetCurrencyData();\n";
            code += "            if (data == null) return null;\n";
            code += "            return data.currencyList.Find(c => c.name == currencyName);\n";
            code += "        }\n\n";

            code += "        public static string GetName(string currencyName)\n        {\n";
            code += "            var info = GetCurrencyInformation(currencyName);\n";
            code += "            return info != null ? info.name : null;\n";
            code += "        }\n\n";

            code += "        public static string GetShownName(string currencyName)\n        {\n";
            code += "            var info = GetCurrencyInformation(currencyName);\n";
            code += "            return info != null ? info.shownName : null;\n";
            code += "        }\n\n";

            code += "        public static string GetDescription(string currencyName)\n        {\n";
            code += "            var info = GetCurrencyInformation(currencyName);\n";
            code += "            return info != null ? info.description : null;\n";
            code += "        }\n\n";

            code += "        public static string GetSymbol(string currencyName)\n        {\n";
            code += "            var info = GetCurrencyInformation(currencyName);\n";
            code += "            return info != null ? info.symbol : null;\n";
            code += "        }\n\n";

            code += "        public static Texture GetIcon(string currencyName)\n        {\n";
            code += "            var info = GetCurrencyInformation(currencyName);\n";
            code += "            return info != null ? info.icon : null;\n";
            code += "        }\n\n";

            code += "        public static Color GetColor(string currencyName)\n        {\n";
            code += "            var info = GetCurrencyInformation(currencyName);\n";
            code += "            return info != null ? info.color : Color.white;\n";
            code += "        }\n\n";

            code += "        public static int GetDecimalPlaces(string currencyName)\n        {\n";
            code += "            var info = GetCurrencyInformation(currencyName);\n";
            code += "            return info != null ? info.decimalPlaces : 0;\n";
            code += "        }\n\n";

            code += "        public static bool GetUseMaximumAmount(string currencyName)\n        {\n";
            code += "            var info = GetCurrencyInformation(currencyName);\n";
            code += "            return info != null ? info.useMaximumAmount : false;\n";
            code += "        }\n\n";

            code += "        public static float GetMaximumAmount(string currencyName)\n        {\n";
            code += "            var info = GetCurrencyInformation(currencyName);\n";
            code += "            return info != null ? info.maximumAmount : 0f;\n";
            code += "        }\n\n";

            code += "        public static float GetDefaultAmount(string currencyName)\n        {\n";
            code += "            var info = GetCurrencyInformation(currencyName);\n";
            code += "            return info != null ? info.defaultAmount : 0f;\n";
            code += "        }\n";

            code += "    }\n}\n";

            File.WriteAllText(currenciesPath, code);
            AssetDatabase.Refresh();
            Debug.Log("Currencies generated automatically!");
        }

        private static string NormalizeCurrencyName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "_";

            var builder = new System.Text.StringBuilder();

            foreach (char c in input)
            {
                if (char.IsLetterOrDigit(c))
                {
                    builder.Append(c);
                }
                else
                {
                    builder.Append("U" + ((int)c).ToString("X4"));
                }
            }

            string raw = builder.ToString();

            var words = Regex.Matches(raw, @"[A-Za-z][a-z]*|\d+|U[0-9A-F]{4}")
                .Select(m => m.Value)
                .ToArray();

            string normalized = string.Concat(words.Select(word =>
                char.IsLetter(word[0]) ? (char.ToUpperInvariant(word[0]) + word.Substring(1)) : word
            ));

            if (!char.IsLetter(normalized[0]) && normalized[0] != '_')
                normalized = "_" + normalized;

            string[] csharpKeywords = new[]
            {
                "abstract", "as", "base", "bool", "break", "byte", "case", "catch",
                "char", "checked", "class", "const", "continue", "decimal", "default",
                "delegate", "do", "double", "else", "enum", "event", "explicit", "extern",
                "false", "finally", "fixed", "float", "for", "foreach", "goto", "if",
                "implicit", "in", "int", "interface", "internal", "is", "lock", "long",
                "namespace", "new", "null", "object", "operator", "out", "override",
                "params", "private", "protected", "public", "readonly", "ref", "return",
                "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string",
                "struct", "switch", "this", "throw", "true", "try", "typeof", "uint",
                "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void",
                "volatile", "while"
            };

            if (csharpKeywords.Contains(normalized.ToLowerInvariant()))
                normalized += "_";

            return normalized;
        }
    }
}