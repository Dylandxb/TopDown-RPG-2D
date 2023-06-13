using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string Name;
    public Item item;

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Character")
        {
            InventoryScript.MyInstance.AddItem(item);
            Destroy(gameObject);
            MessageDisplay disp = GameObject.Find("MessageHandler").GetComponent<MessageDisplay>();
            disp.ShowMessage("You picked up a " + Name, 2.0f);
        }
    }
}
