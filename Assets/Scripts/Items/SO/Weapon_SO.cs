using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield", menuName = "Backpack/Weapon")]
public class Weapon_SO : Item_SO
{
    [Header("������Ϣ")]
    public int MaxDamage;
    public int MinDamage;
    public int EnduConsumption;
    public float HitRate;

    // ���ڵ��߼��໥Ӱ��
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

        // �������� ��������

        if (source.CostEndu(EnduConsumption))
        {
            Debug.Log(source.PlayerName + " ʹ������ [" + itemName + "], ������ " + target.PlayerName);

            // ������ = ���������� + ����ֵ
            if (Random.Range(0, 1) > HitRate + 0.05 * source.Luck)
                return;

            int damage = Random.Range(MinDamage, MaxDamage);

            // ���ƿ���
            damage = target.UseShield(damage);

            // ��������
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
