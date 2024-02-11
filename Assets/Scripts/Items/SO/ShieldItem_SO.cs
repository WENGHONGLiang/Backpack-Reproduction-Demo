using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield", menuName = "Backpack/Shield")]
public class ShieldItem_SO : Item_SO, ICloneable
{
    [Header("ª§∂‹–≈œ¢")]
    public float DefenseRate;
    public int DefenseValue;

    public object Clone()
    {
        ShieldItem_SO clone = CreateInstance<ShieldItem_SO>();

        clone.itemName = this.itemName;
        clone.itemDescription = this.itemDescription;
        clone.icon = this.icon;
        clone.itemType = this.itemType;
        clone.cost = this.cost;
        clone.itemLevel = this.itemLevel;
        clone.influenceType = this.influenceType;
        clone.CoolDown = this.CoolDown;
        clone.UseOnce = this.UseOnce;
        clone.CanUse = this.CanUse;
        clone.lastUsedTime = this.lastUsedTime;

        clone.DefenseRate = DefenseRate;
        clone.DefenseValue = DefenseValue;

        return clone;
    }
}
