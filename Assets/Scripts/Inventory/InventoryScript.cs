using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryScript : MonoBehaviour
{
    private static InventoryScript instance;

    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }
            return instance;
        }
    }


    [SerializeField]
    private Item[] items;


    private SlotScript fromSlot;

    private List<Bag> bags = new List<Bag>();

    [SerializeField]
    private BagButton[] bagButtons;

    

    public bool CanAddBag
    {
        get { return bags.Count < 5; } //if count less than 5 then you can add a bag
    }


    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;

            foreach (Bag bag in bags)
            {
                count += bag.MyBagScript.MyEmptySlotCount; //count all the slots and return it
            }

            return count;
        }
    }

    public int MyTotalSlotCount
    {
        get
        {
            int count = 0;

            foreach(Bag bag in bags)
            {
                count += bag.MyBagScript.MySlots.Count; //counts all the bags open slots
            }
            return count;
        }
    }

    
    public int MyFullSlotCount
    {
        get
        {
            return MyTotalSlotCount - MyEmptySlotCount; //returns total amount of slots available in inventory
        }
    }

    public SlotScript FromSlot
    {
        get
        {
            return fromSlot;
        }
        set
        {
            fromSlot = value; //set the slot to what has been clicked on
            if(value != null) 
            {
                fromSlot.MyIcon.color = Color.grey; //sets color of item being held to grey
            }
            fromSlot = value;
        }
    }

    private void Awake()
    {
    
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(20);
            bag.Use();
        

    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(8);
            bag.Use();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(20);
            AddItem(bag); //adds item to bag
        }

        /*if (Input.GetKeyDown(KeyCode.L))   Apple debugging Purposes
        {
            Apple apple = (Apple)Instantiate(items[1]);
            AddItem(apple);
            
        }*/
    }


    public void AddBag(Bag bag)
    {
        foreach (BagButton bagButton in bagButtons)
        {
            if(bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;
                bags.Add(bag);
                bag.MyBagButton = bagButton; //bag has reference to button its sitting on
                break;
            }

        }
    }

    public void AddBag(Bag bag, BagButton bagButton) //overload the AddBag function with different set of parameters
    {
        bags.Add(bag);
        bagButton.MyBag = bag;
    }

    public void RemoveBag(Bag bag)
    {
        bags.Remove(bag); //removes specific bag item
        Destroy(bag.MyBagScript.gameObject); //gets rid of the game object
    }

    public void SwapBags(Bag oldBag, Bag newBag)
    {
        int newSlotCount = (MyTotalSlotCount - oldBag.Slots) + newBag.Slots; //old bag slot count reduced from total slot count

        if (newSlotCount - MyFullSlotCount >= 0)
        {
            //Swap
            List<Item> bagItems = oldBag.MyBagScript.GetItems();

            RemoveBag(oldBag);

            newBag.MyBagButton = oldBag.MyBagButton; //use this bag button to assign the new bag as it has a new bag button

            newBag.Use(); //equips bag to correct bagbutton when used

            AddItem(oldBag); //puts oldbag in right place

            HandScript.MyInstance.Drop();
            MyInstance.FromSlot = null; //stops generating unlimited amount of bags

            foreach (Item item in bagItems)
            {
                if (item != newBag) //stops duplicates
                {
                    AddItem(item);
                }
            }

        }
    }


    public void AddItem(Item item)
    {
       if(item.MyStackSize > 0) //stack size assigned as a serialized field in the inventory UI
        {
            if (PlaceInStack(item)) //if you look through the slots and cant be placed on top of item
            {
                return;
            }
        }
        PlaceInEmpty(item);
    }

    public void RemoveItem(string itemName)
    {
        foreach (Item i in items)
        {
            if (i.itemName == itemName)
            {
                if (!i.MySlot.IsEmpty)
                {
                    i.MySlot.Clear();
                }
            }
        }
    }

    public bool GetCount(string item)
    {
        if (item.Contains(item))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //foreach loop loops over items and checks if item name is equal to itemName, check the item and its slot returns thatitem.slot.isempty
    //bool function to get name , set name in inspector

    public bool IsInInventory(string itemName)
    {
        foreach (Item i in items)
        {
            if (i.itemName == itemName)
            {
                if (i.MySlot != null)
                {
                    return !i.MySlot.IsEmpty;
                }
            }
            
        }
        return false;
    }

    public void AddItem(string itemName)
    {
        foreach (Item i in items)
        {
            if (i.itemName == itemName)
            {
                SlotScript.MyInstance.AddItem(i);   
            }
        }
    }



    private void PlaceInEmpty(Item item)
    {
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.AddItem(item)) //looks at each bag if its possible to add
            {
                return;
            }
        }
    }

    private bool PlaceInStack(Item item)
    {
        foreach (Bag bag in bags) //refer to a Bag in bags as bag
        {
            foreach (SlotScript slots in bag.MyBagScript.MySlots) //each slot in bag
            {
                if (slots.StackItem(item)) //places item into a stack
                {
                    return true;
                }
            }
        }
        return false;
    }


    public void OpenClose()
    {
        bool closedBag = bags.Find(x => !x.MyBagScript.IsOpen); //, only open the ones closed, if a single bag is closed then set it to true

        foreach (Bag bag in bags)
        {
            if (bag.MyBagScript.IsOpen != closedBag)
            {
                bag.MyBagScript.OpenClose();
            }
        }
    }
}

  
 

