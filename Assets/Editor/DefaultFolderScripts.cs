using UnityEditor;
using UnityEngine;
using System.IO;
public class DefaultFolderScripts : Editor
{

    [MenuItem("Assets/Create/C# Script in Default Folder", false, 80)]
    public static void CreateScriptInDefaultFolder()
    {
        string defaultPath = "Assets/Scripts";
        if (!AssetDatabase.IsValidFolder(defaultPath))
        {
            AssetDatabase.CreateFolder("Assets", "Scripts");
        }

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
            "Assets/Editor/Templates/NewScriptTemplate.cs.txt",
            $"{defaultPath}/NewScript.cs");
    }

}
