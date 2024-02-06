using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield", menuName = "Backpack/Weapon")]
public class Weapon_SO : Item_SO
{
    [Header("Œ‰∆˜–≈œ¢")]
    public int MaxDamage;
    public int MinDamage;
    public int EnduConsumption;
    public float HitRate;
    public float CoolDown;
}
