using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoldKey", menuName = "Items/GoldKey", order = 1)]
public class GoldKey : Item, IUseable
{
    public void Use()
    {
        Remove();
    }
}
