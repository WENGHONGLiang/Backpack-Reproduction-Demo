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

    public void UpdatePlayerStateText(string _name, string _major, int _money, int _health, int _endu, int _round, int _succeed, int _lives)
    {
        Name.text = _name;
        Major.text = _major;
        Money.text = _money.ToString();
        Health.text = _health.ToString();
        Endu.text = _endu.ToString();
        Round.text = _round.ToString();
        Succeed.text = _succeed.ToString();
        Lives.text = _lives.ToString();
    }
}
