using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevDuck
{
    #if UNITY_EDITOR
    [CustomEditor(typeof(DataFashionGame),true)]
    public class SaveDataFashionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            /*DataFashionGame data = (DataFashionGame)target;
            if (GUILayout.Button("Save Data"))
            {
                data.SaveJsonDataFashionGame();
            }
            if (GUILayout.Button("Update Data"))
            {
                data.UpdateJsonDataFashionGame();
            }
            if (GUILayout.Button("Load Data"))
            {
                data.LoadJsonDataFashionGame();
            }*/
        }
    }
    #endif
}
