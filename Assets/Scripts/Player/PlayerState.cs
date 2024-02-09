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
        state.Money += 9;
        EventHandle.CallUpdatePlayerStateUIEvent(state);
    }

    public void DefeatedUpdate()
    {
        state.Round++;
        state.Lives--;
        state.BaseHealth += 10;
        state.Money += 9;
        EventHandle.CallUpdatePlayerStateUIEvent(state);
    }

    /// <summary>
    /// 初始化状态
    /// </summary>
    public void InitPlayerState()
    {
        EventHandle.CallUpdatePlayerStateUIEvent(state);
    }

    /// <summary>
    /// 初始化属性
    /// </summary>
    public void InitPlayerAttribute()
    {
        attribute = GetComponent<PlayerAttribute>();
        attribute.InitAttribute(state);
        EventHandle.CallUpdatePlayerAttributeUIEvent(state, attribute);
    }

    /// <summary>
    /// 更新属性变化
    /// </summary>
    public void UpdateAttribute()
    {
        attribute.UpdateAttribute();
        EventHandle.CallUpdatePlayerAttributeUIEvent(state, attribute);
    }
}
