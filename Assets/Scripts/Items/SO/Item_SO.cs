using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Backpack/Item")]
public class Item_SO : ScriptableObject
{
    [Header("»ù´¡ÐÅÏ¢")]
    public string itemName = "xxx";
    public string itemDescription = "xxx";
    public Sprite icon = null;
    public int itemLevel;
    public ItemType itemType;
    public int cost;
    public ItemType influenceType;
    public float CoolDown;
    public bool UseOnce = false;
    [HideInInspector] public float lastUsedTime = 0;
    [HideInInspector] public bool CanUse;

    public virtual void InitItem()
    {
        lastUsedTime = 0;
        CanUse = true;
    }

    public virtual void UseItem(PlayerAttribute source, PlayerAttribute target)
    {
        //Debug.Log(source.GetPlayerName() + " use item : " + itemName + " for " + target.GetPlayerName());
    }

    public virtual void InfluentItem(Item_SO otherItem)
    {
        
    }
}
