using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region Singleton
    public static EnemyManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("more than one MonoBehaviour");
            return;
        }
        instance = this;
    }
    #endregion

    [HideInInspector] public PlayerAttribute attribute;
    public List<InfluenceInfo> influenceInfos;

    // �������� // ÿ������һ��
    public PlayerState_SO state;            
    public PlayerBackpack_SO backpack;

    private void OnEnable()
    {
        attribute = GetComponent<PlayerAttribute>();
        influenceInfos = new List<InfluenceInfo>();
    }

    private void Start()
    {
        InitEnemyItem();
        InitEnemyAttribute();
    }

    /// <summary>
    /// ʹ�ü�¼�ĵ�����Ϣ��ʼ�����˵���λ��
    /// </summary>
    public void InitEnemyItem()
    {
        backpack.InitBackpack();
        // �������е���
        foreach (var itemToNodeIndex in backpack.ItemToNodeIndex)
        {
            //�������ߵ����нڵ� // �������Ӧ���ڵ�����λ��
            Vector3 worldPos = CalculateItemPos(itemToNodeIndex.nodeIndexs);

            // ʵ���� ���� so �Ž� backpack �� BattleManager ����
            GameObject item = Instantiate(itemToNodeIndex.itemPrefab, worldPos, Quaternion.Euler(0, 0, 90 * itemToNodeIndex.rotateNum), transform);
            Item_SO so = item.GetComponent<ItemMove>().item;
            if (so == null) return;

            switch (so.itemType) 
            {
                case ItemType.Weapon:
                    backpack.weapons.Add((Weapon_SO)so);
                    break;
                case ItemType.Shield:
                    backpack.shields.Add((ShieldItem_SO)so);
                    break;
                case ItemType.Decoration:
                    backpack.decorations.Add((Decoration_SO)so);
                    break;
            }
        }
    }

    Vector3 CalculateItemPos(List<Vector2Int> nodeIndexs)
    {
        Vector3 pos = Vector3.zero;
        foreach (var nodeIndex in nodeIndexs)
        {
            pos += GridManager.instance.GetNodePosByIndex(nodeIndex);
        }
        pos /= nodeIndexs.Count;

        return pos;
    }

    public List<Weapon_SO> GetWeapons()
    {
        return backpack.weapons;
    }

    public List<ShieldItem_SO> GetShield()
    {
        return backpack.shields;
    }

    public List<Decoration_SO> GetDecoration()
    {
        return backpack.decorations;
    }

    public void InitEnemyAttribute()
    {
        attribute.InitAttribute(state);
        UpdateAttribute();
    }

    public void UpdateAttribute()
    {
        attribute.UpdateAttribute();
        EventHandle.CallUpdateEnemyAttributeUIEvent(state, attribute);
    }
}
