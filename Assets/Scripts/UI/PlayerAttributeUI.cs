using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 战斗界面中敌我的属性显示
/// </summary>
public class PlayerAttributeUI : MonoBehaviour
{
    public TMP_Text PlayerName;
    public TMP_Text PlayerHealth;
    public TMP_Text PlayerEndu;
    public TMP_Text PlayerRecover;
    public TMP_Text PlayerLuck;
    public TMP_Text PlayerShield;
    public TMP_Text PlayerPoisoned;
    public TMP_Text PlayerBlind;

    public Image PlayerHealthProgress;
    public Image PlayerEnduProgress;

    public TMP_Text EnemyName;
    public TMP_Text EnemyHealth;
    public TMP_Text EnemyEndu;
    public TMP_Text EnemyRecover;
    public TMP_Text EnemyLuck;
    public TMP_Text EnemyShield;
    public TMP_Text EnemyPoisoned;
    public TMP_Text EnemyBlind;

    public Image EnemyHealthProgress;
    public Image EnemyEnduProgress;

    private void OnEnable()
    {
        EventHandle.UpdatePlayerAttributeUIEvent += UpdatePlayerAttributeUI;
        EventHandle.UpdateEnemyAttributeUIEvent += UpdateEnemyAttributeUI;
    }

    private void OnDisable()
    {
        EventHandle.UpdatePlayerAttributeUIEvent -= UpdatePlayerAttributeUI;
        EventHandle.UpdateEnemyAttributeUIEvent -= UpdateEnemyAttributeUI;
    }

    void UpdatePlayerAttributeUI(PlayerState_SO state, PlayerAttribute attribute)
    {
        PlayerHealthProgress.fillAmount = attribute.CurHealth * 1f / attribute.MaxHealth;
        PlayerEnduProgress.fillAmount = attribute.CurEndu * 1f / attribute.MaxEndu;

        PlayerName.text = state.PlayerName;
        PlayerHealth.text = attribute.CurHealth.ToString() + " / " + attribute.MaxHealth;
        PlayerEndu.text = attribute.CurEndu.ToString() + " / " + attribute.MaxEndu;
        PlayerRecover.text = attribute.Recover.ToString();
        PlayerLuck.text = attribute.Luck.ToString();
        PlayerShield.text = attribute.Defense.ToString();
        PlayerPoisoned.text = attribute.Poisoned.ToString();
        PlayerBlind.text = attribute.Blind.ToString();
    }

    void UpdateEnemyAttributeUI(PlayerState_SO state, PlayerAttribute attribute)
    {
        EnemyHealthProgress.fillAmount = attribute.CurHealth * 1f / attribute.MaxHealth;
        EnemyEnduProgress.fillAmount = attribute.CurEndu * 1f / attribute.MaxEndu;

        EnemyName.text = state.PlayerName;
        EnemyHealth.text = attribute.CurHealth.ToString() + " / " + attribute.MaxHealth;
        EnemyEndu.text = attribute.CurEndu.ToString() + " / " + attribute.MaxEndu;
        EnemyRecover.text = attribute.Recover.ToString();
        EnemyLuck.text = attribute.Luck.ToString();
        EnemyShield.text = attribute.Defense.ToString();
        EnemyPoisoned.text = attribute.Poisoned.ToString();
        EnemyBlind.text = attribute.Blind.ToString();
    }
}
