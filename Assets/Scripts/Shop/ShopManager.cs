using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static UnityEditor.Progress;

public class ShopManager : MonoBehaviour
{
    #region Singleton
    public static ShopManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("more than one ShopManager");
            return;
        }
        instance = this;
    }
    #endregion

    public GameObject[] DefaultItems;   // ��һ�εĵ���
    public GameObject[] ItemList;       // ���е����б�
    List<Item_SO> SOList;               // ���е��ߵ�SO�б�

    public Transform[] ItemPoints;      // �̵����λ��
    public TMP_Text[] TextList;         // �۸���

    List<Item_SO> ItemSOInShop;      // �̵����е���
    List<GameObject> ItemGOInShop;      // �̵����е���

    public Transform storagePos;

    void Start()
    {
        SOList = new List<Item_SO>();
        ItemSOInShop = new List<Item_SO>();
        ItemGOInShop = new List<GameObject>();
        InitItem();
        InitSO();
    }


    /// <summary>
    /// �����̵�Ĭ����Ʒ
    /// </summary>
    void InitItem()
    {
        int index = 0;
        foreach (var item in DefaultItems)
        {
            GameObject newItem = Instantiate(item, ItemPoints[index].position, Quaternion.identity, transform);

            ItemMove itemMove = newItem.GetComponent<ItemMove>();
            itemMove.ShopIndex = index;

            Item_SO newItemSO = itemMove.item;

            TextList[index].text = newItemSO.cost.ToString();

            ItemSOInShop.Add(newItemSO);
            ItemGOInShop.Add(newItem);

            index++;
        }
    }

    void InitSO()
    {
        foreach (var item in ItemList)
        {
            ItemMove itemMove = item.GetComponent<ItemMove>();
            SOList.Add(itemMove.item);
        }
    }

    public void RefreshItem()
    {
        // ɾ�������е�
        ItemSOInShop = new List<Item_SO>();
        foreach (var item in ItemGOInShop)
        {
            Destroy(item);
        }
        ItemGOInShop = new List<GameObject>();

        HashSet<int> book = new HashSet<int>();

        // ���ˢ��һ��
        for (int shopIndex = 0; shopIndex < 5; shopIndex++)
        {
            int itemIndex = UnityEngine.Random.Range(0, ItemList.Length);
            while (book.Contains(itemIndex))
            {
                itemIndex = UnityEngine.Random.Range(0, ItemList.Length);
            }
            book.Add(itemIndex);

            GameObject newItem = Instantiate(ItemList[itemIndex], ItemPoints[shopIndex].position, Quaternion.identity, transform);

            ItemMove itemMove = newItem.GetComponent<ItemMove>();
            itemMove.ShopIndex = shopIndex;

            Item_SO newItemSO = itemMove.item;

            TextList[shopIndex].text = newItemSO.cost.ToString();

            ItemSOInShop.Add(newItemSO);
            ItemGOInShop.Add(newItem);
        }
    }

    public bool PurchaseItem(string _itemName)
    {
        int index = 0;
        foreach(var item in ItemSOInShop) 
        {
            if (item.itemName == _itemName)
            {
                // �̵������������ // ��Ǯ Ȼ���Ƴ��̵�
                if (PlayerState.instance.CostMoney(item.cost))
                {
                    ItemSOInShop.RemoveAt(index);
                    ItemGOInShop.RemoveAt(index);
                    return true;
                }
                return false;
            }
            index++;
        }

        return false;
    }

    public Item_SO FindItemSOByName(string itemName)
    {
        foreach (var so in SOList)
        {
            if (so.itemName == itemName)
                return so;
        }
        return null;

    }
}
