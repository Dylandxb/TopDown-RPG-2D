using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPC1 : MonoBehaviour
{
    MessageDisplay messageBox;
    [SerializeField]
    private Item[] items;

    void Start()
    {
        messageBox = GameObject.Find("MessageHandler").GetComponent<MessageDisplay>();
        
    }



    void answerFunc(bool answer)
    {
        if (answer)
            messageBox.ShowMultilineMessage("Glad to hear it!");
        else
            messageBox.ShowMultilineMessage("Cheer up, it could be worse!");
    }

    void MagicCallback(bool answer)
    {
        if (answer)
        {
            // find the stone in the world
            GameObject stone = GameObject.Find("StoneParent");
            stone.transform.GetChild(0).gameObject.SetActive(false);
            stone.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Character"))
        {
            
            //bool hasScroll = inv.GetCount("Scroll") > 0;
            //bool hasPotion = inv.GetCount("Potion") > 0;
            if (!InventoryScript.MyInstance.IsInInventory("scroll") && !InventoryScript.MyInstance.IsInInventory("potion"))
            {
                messageBox.ShowMultilineMessage(
                "You want to cross the river? This stone has been here many years\n" +
                "It is too heavy for any on the island to move. Only magic will move it.\n" +
                "There is someone in the forest who may be able to help, but he will need to be paid");
            }
            else if (InventoryScript.MyInstance.IsInInventory("scroll") && !InventoryScript.MyInstance.IsInInventory("potion"))
            {
                messageBox.ShowMultilineMessage("Ah, you have the scroll but it is useless without quaffing the blue potion.\n You must find the blue potion");
            }
            else if (InventoryScript.MyInstance.IsInInventory("scroll") && InventoryScript.MyInstance.IsInInventory("potion"))
            {
                messageBox.YesNoMessage("Aha, you have the magic to move the stone. Would you like to quaff the potion and read the scroll?", MagicCallback);
            }

        }
            
        
    } 
}
