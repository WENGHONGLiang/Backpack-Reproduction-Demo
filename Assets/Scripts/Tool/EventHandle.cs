using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandle
{
    public static event Action<PlayerState_SO> UpdatePlayerStateUIEvent;
    public static event Action<PlayerState_SO, PlayerAttribute> UpdatePlayerAttributeUIEvent;
    public static event Action<PlayerState_SO, PlayerAttribute> UpdateEnemyAttributeUIEvent;

    public static void CallUpdatePlayerStateUIEvent(PlayerState_SO state)
    {
        UpdatePlayerStateUIEvent?.Invoke(state);
    }
    public static void CallUpdatePlayerAttributeUIEvent(PlayerState_SO state, PlayerAttribute attribute)
    {
        UpdatePlayerAttributeUIEvent?.Invoke(state, attribute);
    }
    public static void CallUpdateEnemyAttributeUIEvent(PlayerState_SO state, PlayerAttribute attribute)
    {
        UpdateEnemyAttributeUIEvent?.Invoke(state, attribute);
    }
}
