using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DevDuck
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ItemFashionInit), true)]
    public class GetPosItem : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ItemFashionInit myTarget = (ItemFashionInit)target;
            if (GUILayout.Button("Get Position"))
            {
                myTarget.positionOnModel = myTarget.transform.localPosition;
                myTarget.name = myTarget.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.name;
                myTarget.transform.name = "fashion_" + myTarget.name;
            }

            if (GUILayout.Button("Unpack prefab"))
            {
                if (PrefabUtility.IsPartOfPrefabInstance(myTarget.gameObject))
                {
                    PrefabUtility.UnpackPrefabInstance(myTarget.gameObject, PrefabUnpackMode.Completely,
                        InteractionMode.AutomatedAction);
                    Debug.Log("Prefab unpacked!");
                }
                else
                {
                    Debug.LogError("this is not a prefab");
                }
            }

            if (GUILayout.Button("Clear Sprite"))
            {
                myTarget.ClearAllItemSprites();
            }
        }
    }
#endif
}
