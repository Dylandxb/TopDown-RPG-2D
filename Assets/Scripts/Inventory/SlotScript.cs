using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{

    private ObservableStack<Item> items = new ObservableStack<Item>(); //pushes an item on top of another in a stack format

    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text stackSize;

    public BagScript MyBag {get;set;}

    private static SlotScript instance;

    public static SlotScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SlotScript>();
            }
            return instance;
        }
    }

    public bool IsEmpty
    {
        get
        {
            return MyItems.Count == 0;
        }
    }

    public bool IsFull
    {
        get
        {
            if (IsEmpty) //if slot is empty or count is less than the stack size, then it is not Full
            {
                return false;
            }
            else if (MyCount < MyItem.MyStackSize)
            {
                return false;
            }
            return true;
        }   
    }

    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return MyItems.Peek();
            }

            return null;
        }
    }

    public Image MyIcon {
        get
        {
            return icon;
        }
        set
        {
            icon = value;
        }
    }

    public int MyCount
    {
        get
        {
            return MyItems.Count;
        }
    }

    public Text MyStackText
    {
        get
        {
            return stackSize;
        }
    }

    public ObservableStack<Item> MyItems
    {
        get
        {
            return items;
        }
    }

    private void Awake()
    {
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    public bool AddItem(Item item)
    {

        MyItems.Push(item);
        icon.sprite = item.MyIcon; //assigns sprite to icon being added
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType()) //if is empty or the new items type then put that item in a slot of the same item type
        {
            int count = newItems.Count; //take count on amount of new items
            for (int i = 0; i < count; i++) //run through count on newitems
            {
                if (IsFull)
                {
                    return false; //if slot is full then return false
                }
                AddItem(newItems.Pop()); //if not full then take item from item stack and place it on the slot
            }
            return true;
        }
        return false;
    }
    

    public void RemoveItem(Item item) //pass in an item
    {
        if (!IsEmpty)
        {
            MyItems.Pop(); //if it isnt empty then pop an item from the stack
            
        }
    }

    public void Clear()
    {
        if (MyItems.Count > 0)
        {
            MyItems.Clear();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left) //used to carry the item
        {
            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty) //if nothing is in my hand then we dont have something to move
            {
                if (HandScript.MyInstance.MyMoveable != null && HandScript.MyInstance.MyMoveable is Bag)
                {
                    if (MyItem is Bag)
                    {
                        InventoryScript.MyInstance.SwapBags(HandScript.MyInstance.MyMoveable as Bag, MyItem as Bag); //swaps something when item is a bag
                    }
                }
                else
                {
                    HandScript.MyInstance.TakeMoveable(MyItem as IMoveable); //item sitting on the slot
                    InventoryScript.MyInstance.FromSlot = this; //fromslot is equal to whatever has been clicked on
                }    
            }
            else if (InventoryScript.MyInstance.FromSlot == null && IsEmpty && (HandScript.MyInstance.MyMoveable is Bag)) //if handscript carrying a  bag around
            {
                Bag bag = (Bag)HandScript.MyInstance.MyMoveable;

                if (bag.MyBagScript != MyBag && InventoryScript.MyInstance.MyEmptySlotCount - bag.Slots > 0) //
                { 
                    AddItem(bag);
                    bag.MyBagButton.RemoveBag();
                    HandScript.MyInstance.Drop(); //drops bag from hand
                }
                
            }
            else if (InventoryScript.MyInstance.FromSlot != null) //if there is something to move
            {
                if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) || SwapItems(InventoryScript.MyInstance.FromSlot) || AddItems(InventoryScript.MyInstance.FromSlot.MyItems)) //if true then item is dropped from hand
                {
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null; //resets FromSlot so you can put more than 1 item back
                }
            }
            
        }
        if (eventData.button == PointerEventData.InputButton.Right) //right click mouse to open new Bag
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize) //checks if the new item has the same name as what is stored
        {
            MyItems.Push(item); //stacks
            item.MySlot = this;
            return true;
        }
        return false; // not able to stack an item to a slot
    }

    private bool PutItemBack()
    {
        if (InventoryScript.MyInstance.FromSlot == this) //if this is true then item is trying to be put back in the same slot
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white; //resets color to normal
            return true; //its possible
        }
        return false;
    }

    private bool SwapItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false; 
        }
        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize) //if item moving is different from item being clicked on then swap places, or if the count of items is larger than total stack size, then swap, max stack size is 5
        {
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems); //copies the items needed to swap
            from.MyItems.Clear(); //clears the slot

            //copies item from slot B to A
            from.AddItems(MyItems);

            //Clears slot B
            MyItems.Clear();

            //Move items from the A copy to B
            AddItems(tmpFrom);

            return true;
        }
        return false;
    }


    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull) //checks if the item being moved around is the same type and if theres room
        {
            int free = MyItem.MyStackSize - MyCount; //free slots in the stack
            for (int i = 0; i < free; i++)
            {
                AddItem(from.MyItems.Pop());
            }
            return true;
        }
        return false;
    }

    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }
}
