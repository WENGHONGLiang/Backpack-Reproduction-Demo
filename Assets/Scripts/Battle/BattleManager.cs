using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public GameObject VictoryPanel;
    public GameObject DefeatPanel;
    public GameObject InfoPanel;
    public TMP_Text VictoriesNum;
    public TMP_Text RetryNum;

    bool bEnd;
    private void Start()
    {
        bEnd = true;
        Invoke("StartBattle", 2);
        InitItems();
        PlayerState.instance.attribute.SetShield(BackpackManager.instance.shields);
        EnemyManager.instance.attribute.SetShield(EnemyManager.instance.GetShield());
    }

    void Update()
    {
        if (bEnd) return;
        UsePlayerItems();
        UseEnemyItems();
        PlayerState.instance.UpdateAttribute();
        EnemyManager.instance.UpdateAttribute();
        CheckResult();
    }

    void StartBattle()
    {
        bEnd = false;
    }

    void InitItems()
    {
        foreach (var weapon in BackpackManager.instance.weapons)
        {
            weapon.InitItem();
        }
        foreach (var shield in BackpackManager.instance.shields)
        {
            shield.InitItem();
        }
        foreach (var decoration in BackpackManager.instance.decorations)
        {
            decoration.InitItem();
        }

        foreach (var weapon in EnemyManager.instance.GetWeapons())
        {
            weapon.InitItem();
        }
        foreach (var shield in EnemyManager.instance.GetShield())
        {
            shield.InitItem();
        }
        foreach (var decoration in EnemyManager.instance.GetDecoration())
        {
            decoration.InitItem();
        }

        //foreach (var info in BackpackManager.instance.influenceInfos)
        //{
        //    info.influent.InfluentItem(info.target);
        //}
        //foreach (var info in EnemyManager.instance.influenceInfos)
        //{
        //    info.influent.InfluentItem(info.target);
        //}
    }

    void UsePlayerItems()
    {
        foreach (var weapon in BackpackManager.instance.weapons)
        {
            weapon.UseItem(PlayerState.instance.attribute, EnemyManager.instance.attribute);
        }
        foreach (var decoration in BackpackManager.instance.decorations)
        {
            decoration.UseItem(PlayerState.instance.attribute, EnemyManager.instance.attribute);
        }

    }

    void UseEnemyItems()
    {
        foreach (var weapon in EnemyManager.instance.GetWeapons())
        {
            Debug.Log(weapon.itemName);
            weapon.UseItem(EnemyManager.instance.attribute, PlayerState.instance.attribute);
        }
        foreach (var decoration in EnemyManager.instance.GetDecoration())
        {
            decoration.UseItem(EnemyManager.instance.attribute, PlayerState.instance.attribute);
        }
    }

    void CheckResult()
    {
        if (!EnemyManager.instance.attribute.IsAlive())
        {
            // ”Æ
            bEnd = true;

            PlayerState.instance.VictoryUpdate();
            VictoryPanel.SetActive(true);
            InfoPanel.SetActive(true);
            VictoriesNum.text = PlayerState.instance.state.Victories.ToString();
            RetryNum.text = PlayerState.instance.state.Lives.ToString();
            CheckGameEnd();
        }
        if (!PlayerState.instance.attribute.IsAlive())
        {
            //  ‰
            bEnd = true;

            PlayerState.instance.DefeatedUpdate();
            DefeatPanel.SetActive(true);
            InfoPanel.SetActive(true);
            VictoriesNum.text = PlayerState.instance.state.Victories.ToString();
            RetryNum.text = PlayerState.instance.state.Lives.ToString();
            CheckGameEnd();
        }
    }

    void CheckGameEnd()
    {
        if (PlayerState.instance.state.Round == 2)
        {
            // EndGame
        }
    }
}
