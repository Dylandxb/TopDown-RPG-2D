using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;
    private CanvasGroup canvasGroup;

    private List<SlotScript> slots = new List<SlotScript>();

    public bool IsOpen
    {
        get
        {
            return canvasGroup.alpha > 0; //return true if larger than 0 so its open
        }
    }

    public List<SlotScript> MySlots
    {
        get
        {
            return slots;
        }
    }

    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;

            foreach (SlotScript slot in MySlots)
            {
                if (slot.IsEmpty)
                {
                    count++;
                }
            }
            return count;
        }
    }
            

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public List<Item> GetItems()
    {
        List<Item> items = new List<Item>(); //occupies the list with all the items in this specific bag
        
        foreach (SlotScript slot in slots)
        {
            if (!slot.IsEmpty)
            {
                foreach (Item item in slot.MyItems)
                {
                    items.Add(item);
                }
            }
        }
        return items;
        
    }

    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slot.MyBag = this; //slot knows what bag it belongs to
            MySlots.Add(slot);
        }
    }


    

    public bool AddItem(Item item)
    {
        foreach (SlotScript slot in MySlots) //runs through all the slots belonging to each bag
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(item);
                return true; //successfully added
            }
        }
        return false; //if bag is Full
    }

    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1; //sets canvas alpha to 1 if shown and 0 if not shown

        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }
}
