using DevDuck;
using IAP_Dev;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


#if UNITY_EDITOR
namespace NQDev
{
    [CustomEditor(typeof(IAPController))]
    public class CustomIAP_KEY : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space();
            GUIStyle redBoldLabel = new GUIStyle(EditorStyles.boldLabel);
            redBoldLabel.normal.textColor = Color.floralWhite;

            EditorGUILayout.LabelField("Auto Game Info", redBoldLabel);
            EditorGUILayout.LabelField("Game Name", Application.productName, redBoldLabel);
            EditorGUILayout.LabelField("Game ID", Application.identifier, redBoldLabel);
            EditorGUILayout.LabelField("Game version", Application.version, redBoldLabel);

            if (GUILayout.Button("Create KEY"))
            {
                IAPController iapController = (IAPController)target;
                iapController.CreateKeyCode();

                ShopController shopController = FindAnyObjectByType<ShopController>();
                shopController.UpdateItemCoin();
                Duck.Log("Create KeyCode And Set KEY, sprite");
            }
            if (GUILayout.Button("Update price"))
            {
                ShopController shopController = FindAnyObjectByType<ShopController>();
                shopController.SetPriceValue();
                Duck.Log("Price was updated");
            }

            if (GUILayout.Button("Get Infor"))
            {
                IAPController iapController = (IAPController)target;
                iapController.GetGameInfor();
                Duck.Log("game infor was updated");

            }
        }

     
    }
}
#endif