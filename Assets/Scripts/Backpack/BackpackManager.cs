using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEditor.Progress;


/// <summary>
/// 记录背包内所有道具的数据
/// 提供添加、删除、使用道具
/// </summary>
public class BackpackManager : MonoBehaviour
{
    #region Singleton
    public static BackpackManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("more than one BackpackManager");
            return;
        }
        instance = this;
    }
    #endregion

    public Transform itemUsingParent;
    public Transform itemUnusedParent;

    public List<Weapon_SO> weapons;
    public List<ShieldItem_SO> shields;
    public List<Decoration_SO> decorations;

    public List<InfluenceInfo> influenceInfos;

    private void Start()
    {
        weapons = new List<Weapon_SO>();
        shields = new List<ShieldItem_SO>();
        decorations = new List<Decoration_SO>();
        influenceInfos = new List<InfluenceInfo>();
    }

    public void AddItem(Item_SO new_item)
    {
        switch (new_item.itemType)
        {
            case ItemType.None:
                break;
            case ItemType.Weapon:
                weapons.Add((Weapon_SO)new_item);
                break;
            case ItemType.Shield:
                shields.Add((ShieldItem_SO)new_item);
                break;
            case ItemType.Decoration:
                decorations.Add((Decoration_SO)new_item);
                break;
            case ItemType.Nature:
                break;
            case ItemType.Potion:
                break;
            case ItemType.Pet:
                break;
            case ItemType.Gem:
                break;
            case ItemType.Backpack:
                break;
            default:
                break;
        }

    }

    public void RemoveItem(Item_SO new_item)
    {
        switch (new_item.itemType)
        {
            case ItemType.None:
                break;
            case ItemType.Weapon:
                weapons.Remove((Weapon_SO)new_item);
                break;
            case ItemType.Shield:
                shields.Remove((ShieldItem_SO)new_item);
                break;
            case ItemType.Decoration:
                decorations.Remove((Decoration_SO)new_item);
                break;
            case ItemType.Nature:
                break;
            case ItemType.Potion:
                break;
            case ItemType.Pet:
                break;
            case ItemType.Gem:
                break;
            case ItemType.Backpack:
                break;
            default:
                break;
        }
    }
}
