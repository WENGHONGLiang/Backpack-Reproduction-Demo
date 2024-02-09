using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɫ������Ϣ
/// </summary>
[CreateAssetMenu(fileName = "New PlayerState", menuName = "Player/PlayerState")]
public class PlayerState_SO : ScriptableObject
{
    public string PlayerName;
    public string PlayerMajor;

    public int Money = 12;
    public int Lives = 5;

    // ����ֵ // ʵ��ֵ = ����ֵ + ����ֵ
    public int BaseHealth = 35;
    public int BaseEndu = 5;

    public int Round = 0;
    public int Victories = 0;
}
