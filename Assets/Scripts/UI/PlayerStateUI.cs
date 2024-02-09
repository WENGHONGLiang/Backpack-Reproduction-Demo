using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStateUI : MonoBehaviour
{
    public TMP_Text Name;
    public TMP_Text Major;
    public TMP_Text Money;
    public TMP_Text Health;
    public TMP_Text Endu;
    public TMP_Text Round;
    public TMP_Text Succeed;
    public TMP_Text Lives;


    private void OnEnable()
    {
        EventHandle.UpdatePlayerStateUIEvent += UpdatePlayerStateText;
    }

    private void OnDisable()
    {
        EventHandle.UpdatePlayerStateUIEvent -= UpdatePlayerStateText;
    }

    public void UpdatePlayerStateText(PlayerState_SO state)
    {
        Name.text = state.PlayerName;
        Major.text = state.PlayerMajor;
        Money.text = state.Money.ToString();
        Health.text = state.BaseHealth.ToString();
        Endu.text = state.BaseEndu.ToString();
        Round.text = state.Round.ToString();
        Succeed.text = state.Victories.ToString();
        Lives.text = state.Lives.ToString();
    }
}
