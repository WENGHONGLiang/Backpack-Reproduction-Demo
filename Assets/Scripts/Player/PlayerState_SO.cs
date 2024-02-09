using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色基础信息
/// </summary>
[CreateAssetMenu(fileName = "New PlayerState", menuName = "Player/PlayerState")]
public class PlayerState_SO : ScriptableObject
{
    public string PlayerName;
    public string PlayerMajor;

    public int Money = 12;
    public int Lives = 5;

    // 基础值 // 实际值 = 基础值 + 增益值
    public int BaseHealth = 35;
    public int BaseEndu = 5;

    public int Round = 0;
    public int Victories = 0;
}
