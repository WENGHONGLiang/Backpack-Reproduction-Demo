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

    // Item �� prefab ��ռ������λ�õ�ӳ�� // ���Ը� prefab Ϊ so
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
