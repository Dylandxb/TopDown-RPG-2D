using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Apple", menuName = "Items/Apple", order = 1)]
public class Apple : Item, IUseable
{

    [SerializeField]
    private  int hunger;

    public void Use()
    { 
        if(CharacterControl.MyInstance.Hunger.MyCurrentValue < CharacterControl.MyInstance.Hunger.MyMaxValue) //in the case where hunger is 100 dont use an apple, is not possible as the hunger bar has a constant decreasing rate
        {
            Remove();
            CharacterControl.MyInstance.Hunger.MyCurrentValue += hunger;
           
        }

    }


}
