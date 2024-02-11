using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 战斗时的属性变化值
/// </summary>
public class PlayerAttribute : MonoBehaviour
{
    [HideInInspector] public string PlayerName;

    [HideInInspector] public int MaxHealth = 35;
    [HideInInspector] public int MaxEndu = 5;
                      
    [HideInInspector] public int CurHealth;
    [HideInInspector] public int CurEndu;
                      
    [HideInInspector] public int Defense = 0;
    [HideInInspector] public int Recover = 0;
    [HideInInspector] public int Luck = 0;

    // 其它增益...

    [HideInInspector] public int Poisoned = 0;
    [HideInInspector] public int Blind = 0;

    // 其它减益...

    List<ShieldItem_SO> ShieldList;


    // 各属性变化频率与计数器
    float lastRecoverTime = 0;
    float lastPoisonedTime = 0;
    float lastEnduTime = 0;

    float RecoverCoolDown;
    float PoisonedCoolDown;
    float EnduCoolDown;

    public void InitAttribute(PlayerState_SO state)
    {

        PlayerName = state.PlayerName;

        // Fixme：应该是 MaxHealth = BaseHealth + 道具加成
        MaxHealth = state.BaseHealth;
        MaxEndu = state.BaseEndu;

        CurHealth = MaxHealth;
        CurEndu = MaxEndu;
        Recover = 0;
        Luck = 0;
        Defense = 0;
        Poisoned = 0;
        Blind = 0;

        lastRecoverTime = 0;
        lastPoisonedTime = 0;
        lastEnduTime = 0;

        // hardcode
        RecoverCoolDown = 4;
        PoisonedCoolDown = 4;
        EnduCoolDown = 2f;
    }

    /// <summary>
    /// 更新属性变化
    /// </summary>
    public void UpdateAttribute()
    {
        if (Time.time - lastRecoverTime >= RecoverCoolDown)
        {
            CurHealth += Recover;
            CurHealth = Mathf.Min(CurHealth, MaxHealth);

            lastRecoverTime = Time.time;
        }
        if (Time.time - lastPoisonedTime >= PoisonedCoolDown)
        {
            CurHealth -= Poisoned;
            CurHealth = Mathf.Max(CurHealth, 0);

            lastPoisonedTime = Time.time;
        }
        if (Time.time - lastEnduTime >= EnduCoolDown)
        {
            CurEndu += 1;
            CurEndu = Mathf.Min(CurEndu, MaxEndu);

            lastEnduTime = Time.time;
        }
    }

    public int UseDefense(int amount)
    {
        if (Defense > amount)
        {
            Defense -= amount;
            return 0; 
        }
        amount -= Defense;
        Defense = 0;
        return amount;
        
    }

    public int UseShield(int amount)
    {
        if (ShieldList == null)
            return 0;

        foreach (var shield in ShieldList)
        {
            // 防御概率
            if (UnityEngine.Random.Range(0, 1) > shield.DefenseRate)
                continue;
            amount -= shield.DefenseValue;
            Debug.Log("使用盾牌");

            if (amount < 0)
                return 0;
        }
        return amount;
    }

    public void CauseDamage(int amount)
    {
        CurHealth -= amount;
        CurHealth = Mathf.Max(CurHealth, 0);

        //Debug.Log(PlayerName + " Health : " + CurHealth);
    }

    public bool CostEndu(int amount)
    {
        if(amount > CurEndu)
            return false;

        CurEndu -= amount;
        //Debug.Log(PlayerName + " Endu : " + CurEndu);
        return true;
    }

    public void SetShield(List<ShieldItem_SO> shieldList)
    {
        ShieldList = shieldList;
    }

    public bool IsAlive()
    {
        return CurHealth > 0;
    }
}
