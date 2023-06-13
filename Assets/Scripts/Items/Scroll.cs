using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scroll", menuName = "Items/Scroll", order = 1)]
public class Scroll : Item, IUseable
{
    public void Use()
    {
        Remove();
    }
}
