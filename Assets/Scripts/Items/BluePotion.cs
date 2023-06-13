using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BluePotion", menuName ="Items/Potion",order = 1)]
public class BluePotion : Item, IUseable
{
    public void Use()
    {
        Remove();
    }
}

