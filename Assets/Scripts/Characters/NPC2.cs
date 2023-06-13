using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class NPC2 : MonoBehaviour
{
    MessageDisplay messageBox;

    [SerializeField]
    private Item[] items;
    

    void Start()
    {
        messageBox = GameObject.Find("MessageHandler").GetComponent<MessageDisplay>();
    }

    void Update()
    {
        
    }

    

    public void BuyCallBack(bool answer)
    {
        if (answer)
        {
            //refer to slotscript, remove item from the slot, add item to the slot
            //InventoryScript.MyInstance.Removeitem.items("Gold Ring",-1);
            InventoryScript.MyInstance.RemoveItem("goldring");
            InventoryScript.MyInstance.AddItem("scroll");
            
            messageBox.ShowMultilineMessage("Good luck. Remember you must quaff a blue potion before reading the scroll\n" + "I have none but I remember leaving some by a shack near the forest edge");
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Character"))
        {
            //InventoryScript inv = coll.gameObject.GetComponent<InventoryScript>();
           // bool hasGold = inv.GetCount("Gold Ring") > 0; //count the number of gold rings in a slot, if true then return 1
            
            if (InventoryScript.MyInstance.IsInInventory("goldring"))
            {
                messageBox.YesNoMessage("I can sell you a powerful scroll for that gold ring. Do you want to buy it?", BuyCallBack);
            }
            else
            {
                messageBox.ShowMultilineMessage("I have powerful magic to sell, but you have no Gold!");
            }
        }
    }
}
