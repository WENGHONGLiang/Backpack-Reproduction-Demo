using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Decoration", menuName = "Backpack/Decoration")]
public class Decoration_SO : Item_SO, ICloneable
{
    [Header("�ṩ�������")]
    public int Defense = 0;
    public int Recover = 0;
    public int Luck = 0;

    public int Poisoned = 0;
    public int Blind = 0;


    public override void UseItem(PlayerAttribute source, PlayerAttribute target)
    {
        if (Time.time - lastUsedTime < CoolDown || !CanUse)
            return;

        // �Լ�����
        source.Defense += Defense;
        source.Recover += Recover;
        source.Luck += Luck;

        // ��������
        target.Poisoned += Poisoned;
        target.Blind += Blind;

        if(Defense > 0)
            Debug.Log(source.PlayerName + " ʹ�õ��� [" + itemName + "], ������ " + Defense + " �������");
        if(Recover > 0)
            Debug.Log(source.PlayerName + " ʹ�õ��� [" + itemName + "], ������ " + Recover + " ��ָ�");
        if(Luck > 0)
            Debug.Log(source.PlayerName + " ʹ�õ��� [" + itemName + "], ������ " + Luck + " ������");

        lastUsedTime = Time.time;

        if (UseOnce)
            CanUse = false;
    }

    public object Clone()
    {
        Decoration_SO clone = CreateInstance<Decoration_SO>();

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


        clone.Defense = Defense;
        clone.Recover = Recover;
        clone.Luck = Luck;
        clone.Poisoned = Poisoned;
        clone.Blind = Blind;
        return clone;
    }
}
