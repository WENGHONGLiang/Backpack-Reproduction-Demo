using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Decoration", menuName = "Backpack/Decoration")]
public class Decoration_SO : Item_SO
{
    [Header("提供增益减益")]
    public int Defense = 0;
    public int Recover = 0;
    public int Luck = 0;

    public int Poisoned = 0;
    public int Blind = 0;


    public override void UseItem(PlayerAttribute source, PlayerAttribute target)
    {
        if (Time.time - lastUsedTime < CoolDown || !CanUse)
            return;

        // 自己增益
        source.Defense += Defense;
        source.Recover += Recover;
        source.Luck += Luck;

        // 敌人增益
        target.Poisoned += Poisoned;
        target.Blind += Blind;

        if(Defense > 0)
            Debug.Log(source.PlayerName + " 使用道具 [" + itemName + "], 增加了 " + Defense + " 点防御力");
        if(Recover > 0)
            Debug.Log(source.PlayerName + " 使用道具 [" + itemName + "], 增加了 " + Recover + " 点恢复");
        if(Luck > 0)
            Debug.Log(source.PlayerName + " 使用道具 [" + itemName + "], 增加了 " + Luck + " 点幸运");

        lastUsedTime = Time.time;

        if (UseOnce)
            CanUse = false;
    }
}
