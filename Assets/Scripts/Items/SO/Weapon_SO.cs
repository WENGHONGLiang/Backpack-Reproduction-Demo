using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield", menuName = "Backpack/Weapon")]
public class Weapon_SO : Item_SO, ICloneable
{
    [Header("武器信息")]
    public int MaxDamage;
    public int MinDamage;
    public int EnduConsumption;
    public float HitRate;

    // 用于道具间相互影响
     public int AddExtraDamage;
     public float AddExtraHitRate;

    [HideInInspector] public float ExtraDamage;
    [HideInInspector] public float ExtraHitRate;

    public override void InitItem()
    {
        base.InitItem();

        ExtraDamage = 0;
        ExtraHitRate = 0;
    }

    public override void UseItem(PlayerAttribute source, PlayerAttribute target)
    {
        if (Time.time - lastUsedTime < CoolDown)
            return;

        base.UseItem(source, target);

        // 消耗耐力 攻击敌人

        if (CanUse && source.CostEndu(EnduConsumption))
        {
            Debug.Log(source.PlayerName + " 使用武器 [" + itemName + "], 攻击了 " + target.PlayerName);

            // 命中率 = 武器命中率 + 幸运值
            if (UnityEngine.Random.Range(0, 1) > HitRate + 0.05 * source.Luck)
                return;

            int damage = UnityEngine.Random.Range(MinDamage, MaxDamage);

            // 盾牌扛伤
            damage = target.UseShield(damage);

            // 防御增益
            damage = target.UseDefense(damage);
            target.CauseDamage(damage);
        }

        lastUsedTime = Time.time;
    }

    public override void InfluentItem(Item_SO otherItem)
    {
        if(otherItem.itemType == ItemType.Weapon) 
        {
            ((Weapon_SO)otherItem).MaxDamage += AddExtraDamage;
            ((Weapon_SO)otherItem).HitRate += AddExtraHitRate;
        }
    }

    public object Clone()
    {
        Weapon_SO clone = CreateInstance<Weapon_SO>();

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

        clone.MaxDamage = MaxDamage;
        clone.MinDamage = MinDamage;
        clone.EnduConsumption = EnduConsumption;
        clone.HitRate = HitRate;
        clone.ExtraDamage = ExtraDamage;
        clone.ExtraHitRate = ExtraHitRate;
        clone.AddExtraDamage = AddExtraDamage;
        clone.AddExtraHitRate = AddExtraHitRate;
        return clone;
    }
}
