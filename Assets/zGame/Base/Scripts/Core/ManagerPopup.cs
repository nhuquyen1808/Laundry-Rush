using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    public class ManagerPopup : MonoBehaviour
    {
        static ManagerPopup Instance;
        [SerializeField] Transform nShadow;
        [SerializeField] List<Popup> listPopup;

        void Awake()
        {
            Instance = this;
            nShadow.gameObject.SetActive(false);
            ManagerEvent.RegEvent(EventCMD.EVENT_POPUP_CLOSE, CheckPopup);
        }

        void CheckPopup(object e)
        {
            bool checkShadow = false;
            foreach (Popup pop in Instance.listPopup)
                if (pop.gameObject.activeSelf)
                {
                    checkShadow = true;
                    break;
                }
            nShadow.SetAsFirstSibling();
            nShadow.gameObject.SetActive(checkShadow);
        }

        public static async Task<T> ShowPopup<T>() where T : Popup
        {
            Instance.nShadow.gameObject.SetActive(true);
            Instance.nShadow.SetAsLastSibling();

            ManagerEvent.RaiseEvent(EventCMD.EVENT_POPUP_SHOW, typeof(T));
            T pop = null;
            foreach (Popup p in Instance.listPopup)
                if (p is T && !p.gameObject.activeSelf)
                {
                    pop = p as T;
                    break;
                }
            if (pop == null)
            {
                var pr = (await ManagerAsset.LoadAssetAsync<GameObject>(typeof(T).Name)).GetComponent<T>();
                pop = Instantiate(pr, Instance.transform);
                Instance.listPopup.Add(pop);
            }
            if (pop == null)
            {
                Instance.nShadow.gameObject.SetActive(false);
                return null;
            }
            pop.gameObject.SetActive(false);
            Instance.nShadow.SetAsLastSibling();
            pop.transform.SetAsLastSibling();
            return pop;
        }

        public static bool IsShowing()
        {
            foreach (Popup pop in Instance.listPopup)
                if (pop.gameObject.activeSelf) return true;
            return false;
        }
    }
}