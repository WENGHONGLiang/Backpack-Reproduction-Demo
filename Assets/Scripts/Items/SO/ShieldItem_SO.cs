using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shield", menuName = "Backpack/Shield")]
public class ShieldItem_SO : Item_SO
{
    [Header("������Ϣ")]
    public float DefenseRate;
    public int DefenseValue;
}
