using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BagButton : MonoBehaviour, IPointerClickHandler
{
    private Bag bag;

    [SerializeField]
    private Sprite full, empty;

    public Bag MyBag
    {
        get
        {
            return bag;
        }
        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = full;
            }
            else
            {
                GetComponent<Image>().sprite = empty;
            }
            bag = value;
        }
    }

    public void OpenClose(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.MyInstance.FromSlot != null && HandScript.MyInstance.MyMoveable!= null && HandScript.MyInstance.MyMoveable is Bag)
            {
                if (MyBag != null) //if bag already equipped
                {
                    InventoryScript.MyInstance.SwapBags(MyBag, HandScript.MyInstance.MyMoveable as Bag);
                }
                else
                {
                    Bag tmp = (Bag)HandScript.MyInstance.MyMoveable; //take the bag being carried in hand
                    tmp.MyBagButton = this; //set bag button to this to know what to equip
                    tmp.Use();
                    MyBag = tmp;
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }
            }

            else if (Input.GetKey(KeyCode.LeftAlt)) //held down takes off the bag
            {
                HandScript.MyInstance.TakeMoveable(MyBag); //holds bag in hand equipped to the bag button
            }

            else if (bag != null)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }

    public void RemoveBag()
    {
        InventoryScript.MyInstance.RemoveBag(MyBag); //removes bag from inventory
        MyBag.MyBagButton = null; //button not bound to any bag

        foreach (Item item in MyBag.MyBagScript.GetItems()) //take all the items from the bag and put them back into inv
        {
            InventoryScript.MyInstance.AddItem(item);
        }
        MyBag = null; //allows for re-equipping a bag later
    }
}