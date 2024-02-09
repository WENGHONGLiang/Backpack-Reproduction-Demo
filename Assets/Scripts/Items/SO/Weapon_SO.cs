using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield", menuName = "Backpack/Weapon")]
public class Weapon_SO : Item_SO
{
    [Header("武器信息")]
    public int MaxDamage;
    public int MinDamage;
    public int EnduConsumption;
    public float HitRate;

    // 用于道具间相互影响
     public float AddExtraDamage;
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

        //base.UseItem(source, target);

        // 消耗耐力 攻击敌人

        if (source.CostEndu(EnduConsumption))
        {
            Debug.Log(source.PlayerName + " 使用武器 [" + itemName + "], 攻击了 " + target.PlayerName);

            // 命中率 = 武器命中率 + 幸运值
            if (Random.Range(0, 1) > HitRate + 0.05 * source.Luck)
                return;

            int damage = Random.Range(MinDamage, MaxDamage);

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
            ((Weapon_SO)otherItem).ExtraDamage += AddExtraDamage;
            ((Weapon_SO)otherItem).ExtraDamage += AddExtraHitRate;
        }

    }
}
