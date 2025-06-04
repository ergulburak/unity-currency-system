using UnityEditor;
using UnityEngine;

namespace ergulburak.CurrencySystem.Editor
{
    [CustomEditor(typeof(CurrencyData))]
    public class CurrencyDataDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This asset is managed exclusively through the configuration window.",
                MessageType.Info);
            if (GUILayout.Button("Open Configuration Window"))
            {
                CurrencyDataWindow.OpenWindow();
            }
        }
    }
}