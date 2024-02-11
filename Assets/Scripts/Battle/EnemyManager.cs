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

    [System.Serializable]
    public struct Enemy
    {
        public PlayerState_SO    state;
        public PlayerBackpack_SO backpack;
    }

    [HideInInspector] public PlayerAttribute attribute;
    public List<InfluenceInfo> influenceInfos;

    // �������� // ѡ��һ���������ͬ�غ�����
    public Enemy[] enemys;
    Enemy curEnemy;

    private void OnEnable()
    {
        attribute = GetComponent<PlayerAttribute>();
        influenceInfos = new List<InfluenceInfo>();
        InitEnemyState();
        InitEnemyItem();
        
    }
    private void Start()
    {
        InitEnemyAttribute();
    }

    void InitEnemyState()
    {
        foreach (var enemy in enemys) 
        {
            if (enemy.state.Round == PlayerState.instance.state.Round)
            {
                curEnemy = enemy;
                break;
            }
        }
    }

    /// <summary>
    /// ʹ�ü�¼�ĵ�����Ϣ��ʼ�����˵���λ��
    /// </summary>
    public void InitEnemyItem()
    {
        curEnemy.backpack.InitBackpack();
        // �������е���
        foreach (var itemToNodeIndex in curEnemy.backpack.ItemToNodeIndex)
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
                    curEnemy.backpack.weapons.Add((Weapon_SO)((Weapon_SO)so).Clone());
                    break;
                case ItemType.Shield:
                    curEnemy.backpack.shields.Add((ShieldItem_SO)((ShieldItem_SO)so).Clone());
                    break;
                case ItemType.Decoration:
                    curEnemy.backpack.decorations.Add((Decoration_SO)((Decoration_SO)so).Clone());
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
        return curEnemy.backpack.weapons;
    }

    public List<ShieldItem_SO> GetShield()
    {
        return curEnemy.backpack.shields;
    }

    public List<Decoration_SO> GetDecoration()
    {
        return curEnemy.backpack.decorations;
    }

    public void InitEnemyAttribute()
    {
        attribute.InitAttribute(curEnemy.state);
        UpdateAttribute();
    }

    public void UpdateAttribute()
    {
        attribute.UpdateAttribute();
        EventHandle.CallUpdateEnemyAttributeUIEvent(curEnemy.state, attribute);
    }
}
