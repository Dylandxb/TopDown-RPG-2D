using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bag",menuName = "Items/Bag",order =1)]
public class Bag : Item, IUseable
{

    private int slots;

    [SerializeField]
    protected GameObject bagPrefab;

    

    public BagScript MyBagScript { get; set; }

    public BagButton MyBagButton { get; set; }


    public int Slots
    {
        get
        {
            return slots;
        }
    }

    public void Initialize(int slots)
    {
        this.slots = slots;
    }

    public void Use()
    {
        if (InventoryScript.MyInstance.CanAddBag) //can only add bag if less than 5
        {

            Remove();
            MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();
            MyBagScript.AddSlots(slots);

            if (MyBagButton == null) //if bag doesnt have a preset bagbutton
            {
                InventoryScript.MyInstance.AddBag(this); //then equip bag normally and add to first empty bagbutton
            }
            else
            {
                InventoryScript.MyInstance.AddBag(this, MyBagButton); //calls overload function , needs to equip this bag on this specific bagbutton
            }

            
        }
        
    }
    

    
}

