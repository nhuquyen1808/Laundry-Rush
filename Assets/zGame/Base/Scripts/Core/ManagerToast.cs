using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// by nt.Dev93
namespace ntDev
{
    internal struct Toast
    {
        internal string mess;
        internal float time;
        internal Toast(string m, float t)
        {
            mess = m;
            time = t;
        }
    }
    public class ManagerToast : MonoBehaviour
    {
        public static ManagerToast Instance;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        [SerializeField] CanvasGroup canvas;
        [SerializeField] HorizontalLayoutGroup layoutGroup;
        [SerializeField] LayoutElement layoutElement;
        [SerializeField] TextMeshProUGUI txtContent;

        List<Toast> listToast = new List<Toast>();

        public static void Show(string mess, float time = 3)
        {
            if (mess == "NOT_ENOUGH_BALANCE") mess = " NOT ENOUGH BALANCE ";
            else if (mess == "NOT_ENOUGH_BALANCE_TOKEN") mess = " NOT ENOUGH TOKEN ";
            else if (mess == "NOT_ENOUGH_BALANCE_COIN") mess = " NOT ENOUGH COIN ";
            else if (mess == "SPIN_ERROR") mess = " SPIN FAILED, PLEASE TRY AGAIN! ";
            else if (mess == "REACH_MAX_LEVEL_OF_ISLAND") mess = " YOUR ISLAND HAS REACHED THE MAXIMUM TIER ";
            else if (mess == "ITEM_NOT_REACH_MAX_LEVEL") mess = " BUILDING'S TIER IS NOT SUFFICIENT FOR ISLAND UPGRADE.\nLEVEL UP ALL BUILDING TO LEVEL 5 TO UPGRADE ISLAND ";
            else if (mess == "REACH_MAX_LEVEL_OF_ITEM") mess = " YOU BUILDING HAS REACHED THE MAXIMUM UPGRADE ";
            else if (mess == "DONT_HAVE_NEXT_TIER_NFT") mess = " NO NFT REQUIRED ";
            else if (mess == "ITEM_NEED_REPAIR") mess = " REPAIR YOUR BUILDING BEFORE UPGRADE! ";
            else if (mess == "ISLAND_ACTIVE_INVALID") mess = " YOUR ISLAND HAS NOT BEEN CONSTRUCTED YET ";
            else if (mess == "ISLAND_PROTECTED") mess = " THIS ISLAND IS CURRENTLY UNDER PROTECTION ";
            else if (mess == "ITEM_NOT_EXIST") mess = " THIS PROJECT HAS BEEN DESTROYED BY OTHERS ";
            else if (mess == "NOT_ENOUGH_STEAL") mess = " You don't have enough rob attempts ";
            else if (mess == "TOKEN_IS_INVALID") mess = " Session has expired. Please reload the game ";
            Instance.AddToast(mess, time);
        }

        protected void AddToast(string mess, float time)
        {
            if (listToast.Count > 0 && listToast[listToast.Count - 1].mess == mess) return;
            listToast.Add(new Toast(mess, time));
            if (listToast.Count == 1)
            {
                canvas.alpha = 0;
                StartCoroutine(ShowToast());
            }
        }

        public float maxWidth = 800;
        protected IEnumerator ShowToast()
        {
            while (listToast.Count > 0)
            {
                txtContent.text = listToast[0].mess;
                layoutElement.preferredWidth = -1;
                layoutGroup.childControlWidth = true;
                yield return null;
                if (txtContent.rectTransform.sizeDelta.x > maxWidth)
                {
                    layoutElement.preferredWidth = maxWidth;
                    layoutGroup.childControlWidth = true;
                }
                for (float i = 0; i <= 1; i += 0.03f)
                    yield return canvas.alpha = i;
                canvas.alpha = 1;
                yield return new WaitForSeconds(listToast[0].time);
                for (float i = 1; i >= 0; i -= 0.03f)
                    yield return canvas.alpha = i;
                canvas.alpha = 0;
                listToast.RemoveAt(0);
            }
        }
    }
}