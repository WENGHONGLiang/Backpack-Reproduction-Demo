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

    public GameObject[] DefaultItems;   // 第一次的道具
    public GameObject[] ItemList;       // 所有道具列表
    List<Item_SO> SOList;               // 所有道具的SO列表

    public Transform[] ItemPoints;      // 商店道具位置
    public TMP_Text[] TextList;         // 价格牌

    List<Item_SO> ItemSOInShop;      // 商店现有道具
    List<GameObject> ItemGOInShop;      // 商店现有道具

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
    /// 生成商店默认物品
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
        // 删除现在有的
        ItemSOInShop = new List<Item_SO>();
        foreach (var item in ItemGOInShop)
        {
            Destroy(item);
        }
        ItemGOInShop = new List<GameObject>();

        HashSet<int> book = new HashSet<int>();

        // 随机刷新一批
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
                // 商店里有且买得起 // 扣钱 然后移出商店
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
