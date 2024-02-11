using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerBackpack", menuName = "Player/PlayerBackpack")]
public class PlayerBackpack_SO : ScriptableObject
{
    [System.Serializable]
    public struct ItemNodeMap
    {
        public GameObject itemPrefab;
        public List<Vector2Int> nodeIndexs;
        public int rotateNum;
    }

    // Item 的 prefab 和占用网格位置的映射 // 可以改 prefab 为 so
    public List<ItemNodeMap> ItemToNodeIndex;

    public List<Weapon_SO> weapons;
    public List<ShieldItem_SO> shields;
    public List<Decoration_SO> decorations;

    public void InitBackpack()
    {
        weapons = new List<Weapon_SO>();
        shields = new List<ShieldItem_SO>();
        decorations = new List<Decoration_SO>();
    }
}
