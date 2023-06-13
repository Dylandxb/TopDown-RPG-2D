using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    private static HandScript instance;
    public static HandScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HandScript>();
            }
            return instance;
        }
    }
    private void Start()
    {
        icon = GetComponent<Image>();
    }

    private void Update()
    {
        icon.transform.position = Input.mousePosition+offset; //item follows mouse cursor
        DeleteItem(); //drops the item outside of the inventory
    }

    public IMoveable MyMoveable { get; set; }

    private Image icon;

    [SerializeField]
    private Vector3 offset;

    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable; //assign moveable to MyMoveable
        icon.sprite = moveable.MyIcon;
        icon.color = Color.white;
    }
    public IMoveable Put()
    {
        IMoveable tmp = MyMoveable;
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0);
        return tmp;
    }

    public void Drop()
    {
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0); //drops item in a slot, dissapears 
    }

    private void DeleteItem()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && MyInstance.MyMoveable != null)
        {
            if (MyMoveable is Item && InventoryScript.MyInstance.FromSlot != null) //makes sure items is being held in the hand, fromSlot is selected
            {
                (MyMoveable as Item).MySlot.Clear(); //cleared from inventory slot
            }
            Drop();
            InventoryScript.MyInstance.FromSlot = null; //removes the reference to that slot since item has been removed
        }
    }
}
