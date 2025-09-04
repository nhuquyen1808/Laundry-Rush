using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using DG.Tweening;
using UnityEngine;

namespace DevDuck
{
    public class NpcGame5 : MonoBehaviour
    {
        public GameObject food1, food2, food3;
        public int idFood1, idFood2, idFood3;
        public GameObject boxOrder1Food, boxOrder2Food, boxOrder3Food;
        public List<int> listID = new List<int>();

        public virtual void ShowOrder()
        {
            // Debug.Log("Show order");
            int a = GetRandomBox();
            switch (a)
            {
                case 1:
                    idFood1 = GetRandomFood();
                    listID.Add(idFood1);
                    food1.GetComponent<SpriteRenderer>().sprite =
                        Resources.Load<Sprite>($"Art/game5/easy_mode/food_{idFood1}");
                    boxOrder1Food.SetActive(true);
                    food1.transform.SetParent(boxOrder1Food.transform);
                    food1.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    food1.transform.localPosition = new Vector3(0.2f, -0.25f, 0f);
                    boxOrder1Food.transform.DOScale(.6f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.5f);
                    break;
                case 2:
                    idFood1 = GetRandomFood();
                    food1.GetComponent<SpriteRenderer>().sprite =
                        Resources.Load<Sprite>($"Art/game5/easy_mode/food_{idFood1}");
                    idFood2 = GetRandomFood();
                    food2.GetComponent<SpriteRenderer>().sprite =
                        Resources.Load<Sprite>($"Art/game5/easy_mode/food_{idFood2}");
                    boxOrder2Food.SetActive(true);
                    listID.Add(idFood1);
                    listID.Add(idFood2);

                    food1.transform.SetParent(boxOrder2Food.transform);
                    food2.transform.SetParent(boxOrder2Food.transform);
                    food1.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    food2.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    food1.transform.localPosition = new Vector3(0.2f, -0.25f, 0f);
                    food2.transform.localPosition = new Vector3(1.15f, -0.25f, 0f);
                    boxOrder2Food.transform.DOScale(.6f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.5f);

                    break;
                case 3:
                    idFood1 = GetRandomFood();
                    food1.GetComponent<SpriteRenderer>().sprite =
                        Resources.Load<Sprite>($"Art/game5/easy_mode/food_{idFood1}");
                    idFood2 = GetRandomFood();
                    food2.GetComponent<SpriteRenderer>().sprite =
                        Resources.Load<Sprite>($"Art/game5/easy_mode/food_{idFood2}");
                    while (idFood3 != idFood1 && idFood3 != idFood2)
                    {
                        idFood3 = GetRandomFood();
                    }

                    food3.GetComponent<SpriteRenderer>().sprite =
                        Resources.Load<Sprite>($"Art/game5/easy_mode/food_{idFood3}");
                    boxOrder3Food.SetActive(true);
                    food1.transform.SetParent(boxOrder3Food.transform);
                    food2.transform.SetParent(boxOrder3Food.transform);
                    food3.transform.SetParent(boxOrder3Food.transform);
                    listID.Add(idFood1);
                    listID.Add(idFood2);
                    listID.Add(idFood3);
                    food1.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    food2.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    food3.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    food1.transform.localPosition = new Vector3(0.2f, -0.25f, 0f);
                    food2.transform.localPosition = new Vector3(1.15f, -0.25f, 0f);
                    food3.transform.localPosition = new Vector3(2.1f, -0.25f, 0f);
                    boxOrder3Food.transform.DOScale(.6f, 0.3f).SetEase(Ease.OutBack).SetDelay(0.5f);
                    break;
            }
        }

        private int GetRandomBox()
        {
            return Random.Range(1, 4);
        }

        public int GetRandomFood()
        {
            return Random.Range(1, 5);
        }

        public void HideOrder()
        {
            if (boxOrder1Food.activeSelf) boxOrder1Food.SetActive(false);
            if (boxOrder2Food.activeSelf) boxOrder2Food.SetActive(false);
            if (boxOrder3Food.activeSelf) boxOrder3Food.SetActive(false);
        }

        public bool CheckNpcHasOrder(int idFood)
        {
            if (listID.Contains(idFood))
            {
                //  Debug.Log("TRUE");
                listID.Remove(idFood);
                if (idFood1 == idFood)
                {
                    idFood1 = -1;
                  //  LogicUiGame6.instance.ShowTickImage(food1);
                    StartCoroutine(DelayShowTick(food1));
                }
                else if (idFood2 == idFood)
                {
                    idFood2 = -1;
                  //  LogicUiGame6.instance.ShowTickImage(food2);
                    StartCoroutine(DelayShowTick(food2));
                }
                else if (idFood3 == idFood)
                {
                    idFood3 = -1;
                    StartCoroutine(DelayShowTick(food3));
                   // LogicUiGame6.instance.ShowTickImage(food3);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckIsFullOrder()
        {
            if (listID.Count == 0) return true;
            else return false;
        }

        IEnumerator DelayShowTick(GameObject o)
        {
            yield return new WaitForSeconds(0.8f);
            GameObject a = Instantiate( LogicUiGame6.instance.tickObject, o.transform.position, Quaternion.identity);
            a.transform.SetParent(o.transform);
            a.transform.localScale = new Vector3(1.02f, 1.02f, 1f);
            a.transform.localPosition = new Vector3(-0.4f, 0.42f, 1f);
            
          //  a.GetComponent<SpriteRenderer>().sprite = LogicUiGame6.instance.tickSprite;
            a.GetComponent<SpriteRenderer>().sortingOrder = 3;
            a.GetComponent<Animator>().Play("tickAnimation");
        }
    }
}