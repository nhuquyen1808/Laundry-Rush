using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Accessibility;

public class LogicGame : MonoBehaviour
{
    Camera cam;
    [SerializeField] thing currentThing;
    [SerializeField] List<Slot> Slots = new List<Slot>();

    [SerializeField] List<thing> _listCurrentThing = new List<thing>();

    private void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                if (hitInfo.collider != null)
                {
                    currentThing = hitInfo.collider.gameObject.GetComponent<thing>();
                    if (currentThing != null)
                    {
                        _listCurrentThing.Add(currentThing);
                        int emptySlot = FirstEmptySlots();
                        currentThing.Move(Slots[emptySlot].transform.position + new Vector3(0, 0, 1));
                        Slots[emptySlot].currentThing = currentThing;
                        Slots[emptySlot].currentThingID = currentThing.ID;
                        
                        CheckListCurrentThing();
                        /*HandleTileClicked(currentThing);
                        CheckListCurrentThing();*/
                    }
                }
            }
        }
    }

    public int FirstEmptySlots()
    {
        int firstEmptySlotID = -1;
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].empty)
            {
                firstEmptySlotID = Slots[i].ID;
                Slots[i].empty = false;
                break;
            }
        }

        return firstEmptySlotID;
    }

    List<thing> tempList = new List<thing>();

    public void CheckListCurrentThing()
    {
        Dictionary<int, int> countMap = new Dictionary<int, int>();
        for (int i = 0; i < _listCurrentThing.Count; i++)
        {
            int number = _listCurrentThing[i].ID;

            if (countMap.ContainsKey(number))
            {
                countMap[number]++;
            }
            else
            {
                countMap.Add(number, 1);
            }
        }

        foreach (KeyValuePair<int, int> pair in countMap)
        {
            if (pair.Value >= 3)
            {
                tempList.Clear();
                for (int i = 0; i < _listCurrentThing.Count; i++)
                {
                    if (_listCurrentThing[i].ID == pair.Key)
                    {
                        tempList.Add(_listCurrentThing[i]);
                    }
                }

                foreach (var slot in Slots)
                {
                    if (slot.currentThingID == pair.Key)
                    {
                        slot.empty = true;
                        slot.currentThingID = -1;
                        slot.currentThing = null;
                        _listCurrentThing.Remove(slot.currentThing);
                        Debug.Log("Empty slot ???");
                    }
                }
                Hide();
            }
        }
    }

    public void Hide()
    {
        for (int i = 0; i < tempList.Count; i++)
        {
            var a = i;
            tempList[a].transform.DOScale(0, 0.3f).SetDelay(0.5f);
            if (_listCurrentThing.Contains(tempList[i]))
            {
                _listCurrentThing.Remove(tempList[i]);
            }
        }
    }
    
    
    public void HandleTileClicked(thing tile)
    {
        int emptySlotCount = Slots.Count;
        int sameID = -1;
        for (int i = emptySlotCount - 1; i >= 0; i--)
        {
            thing currentTile = Slots[i].currentThing;
            if (currentTile != null && currentTile.ID == tile.ID)
            {
                sameID = i;
                break;
            }
        }

        if (sameID != -1)
        {
            int insertionIndex = sameID + 1;
            if (insertionIndex < emptySlotCount)
            {
                MoveTilesToRight(insertionIndex);
                PutTileToEmptySlot(tile, insertionIndex, 0.35f, true);
            }
        }
        else
        {
            int emptySlot = FindFirstEmptySlot();
            if (emptySlot != -1)
            {
                PutTileToEmptySlot(tile, emptySlot, 0.35f, true);
            }
        }
    }
    private int FindFirstEmptySlot()
    {
        foreach (Slot desGame in Slots)
        {
            if (desGame.currentThing == null)
                return desGame.ID;
        }

        return -1;
    }
    
    private void MoveTilesToRight(int fromIndex)
    {
        for (int i = Slots.Count - 2; i >= fromIndex; i--)
        {
            Slot currentSlot = Slots[i];
            if (currentSlot.currentThing != null)
            {
                thing tileToMove = currentSlot.currentThing;
                int targetIndex = i + 1;
                if (targetIndex >= Slots.Count) continue;


                if (Slots[targetIndex].currentThing == null)
                {
                    PutTileToEmptySlot(tileToMove, targetIndex, 0.2f, false);
                    currentSlot.currentThing = null;
                }
                else
                {
                    MoveTilesToRight(targetIndex);
                    if (Slots[targetIndex].currentThing == null)
                    {
                        PutTileToEmptySlot(tileToMove, targetIndex, 0.2f, false);
                        currentSlot.currentThing = null;
                    }
                }
            }
        }
    }

    private void PutTileToEmptySlot(thing tile, int slotIndex, float timer, bool isCheck)
    {
        /*int emptySlots = FirstEmptySlots();
        Slots[emptySlots].empty = false;
        Slots[emptySlots].currentThingID = currentThing.ID;
        Slots[emptySlots].currentThing = currentThing;*/
        
        Slots[slotIndex].empty = false;
        tile.transform.SetParent(Slots[slotIndex].transform);
        currentThing.Move(Slots[slotIndex].transform.position + new Vector3(0, 0, 1));
        
        
        /*Slots[slotIndex].currentThing = tile;
        tile.transform.DOJump(Slots[slotIndex].transform.position + new Vector3(0, 0.1f, 0),
                2, 1, timer, false)
            .OnStart(() => { tile.transform.DORotate(new Vector3(0, 0, 0), timer); })
            .OnComplete(() =>
            {
               
            });*/
    }
}