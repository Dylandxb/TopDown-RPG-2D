using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject, IMoveable
{
    [SerializeField]
    private Sprite icon;
    public string itemName;

    [SerializeField] private int stackSize;

    public SlotScript MySlot;

    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }
    public int MyStackSize
    {
        get
        {
            return stackSize;
        }
    }

    /*public string Name 
    {
        get
        {
            return itemName;
        }
    }*/

    protected SlotScript Slot
    {
        get
        {
            return MySlot;
        }
        set
        {
            MySlot = value;
        }

    }

    public void Remove()
    {
        if(MySlot != null)
        {
            MySlot.RemoveItem(this); //remove itself
        }
    }
}
