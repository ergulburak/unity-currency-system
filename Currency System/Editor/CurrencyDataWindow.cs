using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ergulburak.CurrencySystem.Editor
{
    public class CurrencyDataWindow : EditorWindow
    {
        private CurrencyData currencyData;
        private Vector2 scrollPosition;
        private GUIStyle headerStyle;
        private GUIStyle buttonStyle;
        private GUIStyle textFieldStyle;
        private bool stylesInitialized = false;

        [MenuItem("Tools/Currency Manager")]
        public static void OpenWindow()
        {
            var window = GetWindow<CurrencyDataWindow>("Currency Manager");
            window.minSize = new Vector2(300, 400);
            window.maxSize = new Vector2(300, 400);
        }

        private void OnEnable()
        {
            CurrencyEditorHelper.CheckCurrencyData(out currencyData);
        }

        private void OnGUI()
        {
            if (!stylesInitialized)
            {
                InitializeStyles();
                stylesInitialized = true;
            }

            if (currencyData == null)
            {
                EditorGUILayout.HelpBox("CurrencyData could not be loaded!", MessageType.Error);
                return;
            }

            EditorGUILayout.LabelField("Currency List", headerStyle);
            EditorGUILayout.Space(10);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true));

            for (int i = 0; i < currencyData.currencyList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label($"{i + 1}:", headerStyle, GUILayout.Width(15));
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
                string newValue = EditorGUILayout.TextField(currencyData.currencyList[i].name, textFieldStyle,
                    GUILayout.Height(30), GUILayout.ExpandWidth(true));
                if (newValue != currencyData.currencyList[i].name)
                {
                    if (currencyData.currencyList.Any(c => c.name == newValue))
                    {
                        EditorUtility.DisplayDialog("Warning", "A currency with the same name already exists!", "OK");
                        GUI.FocusControl(null);
                    }
                    else
                    {
                        currencyData.currencyList[i].name = newValue;
                        EditorUtility.SetDirty(currencyData);
                    }
                }

                if (GUILayout.Button("Config", buttonStyle, GUILayout.Width(80), GUILayout.Height(30)))
                {
                    CurrencyConfigWindow.ShowWindow(currencyData.currencyList[i]);
                }

                if (GUILayout.Button("X", buttonStyle, GUILayout.Width(30), GUILayout.Height(30)))
                {
                    currencyData.currencyList.RemoveAt(i);
                    EditorUtility.SetDirty(currencyData);
                    i--;
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(5);
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space(10);
            if (GUILayout.Button("Add New Currency", buttonStyle, GUILayout.Height(40)))
            {
                string newCurrency = "NewCurrency";
                int suffix = 1;
                while (currencyData.currencyList.Any(c => c.name == newCurrency))
                {
                    newCurrency = $"NewCurrency{suffix++}";
                }

                currencyData.currencyList.Add(new CurrencyInformation { name = newCurrency });
                EditorUtility.SetDirty(currencyData);
            }

            EditorGUILayout.Space(10);
            if (GUILayout.Button("Update Currencies", buttonStyle, GUILayout.Height(40)))
            {
                if (currencyData.currencyList.GroupBy(c => c.name).Any(g => g.Count() > 1))
                {
                    EditorUtility.DisplayDialog("Warning",
                        "A currency with the same name already exists! Please remove duplicates before generating.",
                        "OK");
                }
                else
                {
                    try
                    {
                        CurrencyEditorHelper.UpdateCurrencies();
                        EditorUtility.DisplayDialog("Successfully", "Currencies updated successfully!", "OK");
                    }
                    catch (Exception e)
                    {
                        EditorUtility.DisplayDialog("Error", e.Message, "OK");
                        throw;
                    }
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(currencyData);
            }
        }

        private void InitializeStyles()
        {
            headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 16,
                margin = new RectOffset(10, 10, 10, 10)
            };

            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 14,
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(10, 10, 5, 5)
            };

            textFieldStyle = new GUIStyle(GUI.skin.textField)
            {
                fontSize = 14,
                padding = new RectOffset(5, 5, 5, 5),
                margin = new RectOffset(10, 10, 5, 5)
            };
        }
    }
}