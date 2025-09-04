using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevDuck
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(SavePositonGame16), true)]
    public class SetMapEditor_16 : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SavePositonGame16 savePositonGame16 = (SavePositonGame16)target;
            if (GUILayout.Button("Update Data"))
            {
                savePositonGame16.UpdateJson();
            }
            if (GUILayout.Button("Save Data"))
            {
                savePositonGame16.SaveToJson();
            }
            if (GUILayout.Button("Load Data"))
            {
                savePositonGame16.LoadFromJson();
            }
        }
    }
    #endif
}
