using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "New Item", menuName = "Backpack/Item")]
public class Item_SO : ScriptableObject
{
    [Header("»ù´¡ÐÅÏ¢")]
    public string itemName = "xxx";
    public Sprite icon = null;
    public int itemLevel;
    public ItemType itemType;
    public int cost;
    public ItemType influenceType;
    public virtual void UseItem()
    {
        Debug.Log("use item : " + itemName);
        RemoveFromInventory();
    }

    public void RemoveFromInventory()
    {

    }
}
