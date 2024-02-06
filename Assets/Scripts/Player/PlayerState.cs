using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    #region Singleton
    public static PlayerState instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("more than one PlayerState");
            return;
        }
        instance = this;
    }
    #endregion

    public int startMoney = 12;
    public int startLives = 5;
    public int startBaseHealth = 35;
    public int startBaseEndu = 5;

    public PlayerStateUI ui;

    public static string PlayerName = "翁洪亮";
    public static string PlayerMajor = "游侠";
    static int Money;
    static int Lives;

    // 基础值 // Attribute中 基础值 + 增益值 = 实际值
    static int BaseHealth;
    static int BaseEndu;

    static int Round;
    static int Victories;

    private void Start()
    {
        Money = startMoney;
        Lives = startLives;
        BaseHealth = startBaseHealth;
        BaseEndu = startBaseEndu;
        Round = 0;
        Victories = 0;
        ui.UpdatePlayerStateText(PlayerName, PlayerMajor, Money, BaseHealth, BaseEndu, Round, Victories, Lives);
    }

    public int GetMoney(){  return Money; }

    public void AddMoney(int amount)
    {
        Money += amount;
        ui.UpdatePlayerStateText(PlayerName, PlayerMajor, Money, BaseHealth, BaseEndu, Round, Victories, Lives);
    }

    public bool CostMoney(int amount)
    {
        if (Money < amount)
            return false;

        Money -= amount;
        ui.UpdatePlayerStateText(PlayerName, PlayerMajor, Money, BaseHealth, BaseEndu, Round, Victories, Lives);
        return true;
    }

    public int GetLives() {  return Lives; }

    public int GetRound() { return Round;}

    public int GetVictories() {  return Victories;}

    public void VictoryUpdate()
    {
        Round++;
        Victories++;
        BaseHealth += 10;
        ui.UpdatePlayerStateText(PlayerName, PlayerMajor, Money, BaseHealth, BaseEndu, Round, Victories, Lives);
    }

    public void DefeatedUpdate()
    {
        Round++;
        Lives--;
        BaseHealth += 10;
        ui.UpdatePlayerStateText(PlayerName, PlayerMajor, Money, BaseHealth, BaseEndu, Round, Victories, Lives);
    }
}
