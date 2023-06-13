using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ring", menuName = "Items/Ring", order = 1)]
public class Ring : Item, IUseable
{
    public void Use()
    {
        Remove();
    }
}
