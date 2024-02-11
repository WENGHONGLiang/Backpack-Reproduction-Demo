using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Linq;
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

    public PlayerState_SO state;
    [HideInInspector] public PlayerAttribute attribute;


    private void Start()
    {
        InitPlayerState();
        InitPlayerAttribute();
    }

    public void AddMoney(int amount)
    {
        state.Money += amount;
        EventHandle.CallUpdatePlayerStateUIEvent(state);
    }

    public bool CostMoney(int amount)
    {
        if (state.Money < amount)
            return false;

        state.Money -= amount;
        EventHandle.CallUpdatePlayerStateUIEvent(state);
        return true;
    }

    public void VictoryUpdate()
    {
        state.Round++;
        state.Victories++;
        state.BaseHealth += 10;
        state.Money += 20;
        EventHandle.CallUpdatePlayerStateUIEvent(state);
    }

    public void DefeatedUpdate()
    {
        state.Round++;
        state.Lives--;
        state.BaseHealth += 10;
        state.Money += 20;
        EventHandle.CallUpdatePlayerStateUIEvent(state);
    }

    public void InitPlayerState()
    {
        state.Round = 0;
        state.Victories = 0;
        state.Lives = 5;
        state.Money = 25;
        state.BaseHealth = 35;
        state.BaseEndu = 5;
        UpdatePlayerState();
    }

    /// <summary>
    /// ��ʼ��״̬
    /// </summary>
    public void UpdatePlayerState()
    {
        EventHandle.CallUpdatePlayerStateUIEvent(state);
    }

    /// <summary>
    /// ��ʼ������
    /// </summary>
    public void InitPlayerAttribute()
    {
        attribute = GetComponent<PlayerAttribute>();
        attribute.InitAttribute(state);
        EventHandle.CallUpdatePlayerAttributeUIEvent(state, attribute);
    }

    /// <summary>
    /// �������Ա仯
    /// </summary>
    public void UpdateAttribute()
    {
        attribute.UpdateAttribute();
        EventHandle.CallUpdatePlayerAttributeUIEvent(state, attribute);
    }
}
