using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ItemMove : Interactable
{
    Node[,] itemGrid;  // 网格记录
    Vector3 gridStartPoint;
    ItemState state;
    Vector3 itemPos;
    List<Node> curGridNodeList;
    int rowCount;
    int colCount;

    FixedBackpack fixedBackpack;

    ItemInfluence itemInfluence;

    [Header("Test")]
    public GameObject NodeImagePrefab;

    [Header("网格设置")]
    public int startRowCount;
    public int startColCount;
    public float nodeRadius = .25f;

    [Header("基础设置")]
    public int ShopIndex;   // 在商店的哪一格
    public Item_SO item;



    private void Start()
    {
        // 初始化道具网格
        UpdateItemGrid(startRowCount, startColCount);

        // 初始化道具周边网格
        itemInfluence = GetComponent<ItemInfluence>();
        itemInfluence.InitItemInfluenceGrid(itemGrid, nodeRadius);

        state = ItemState.Sale;
        itemPos = ShopManager.instance.ItemPoints[ShopIndex].position;
        curGridNodeList = null;
        CanInteract = true;

        if (item.itemType == ItemType.Backpack)
        {
            fixedBackpack = GetComponent<FixedBackpack>();
            fixedBackpack.bEnable = false;
        }

        itemInfluence.HideStar();

        InvokeRepeating("CheckInfluence", 0.5f, 0.5f);
    }

    [ContextMenu("Generate Grid")]
    public void TestForGrid()
    {
        UpdateItemGrid(startRowCount, startColCount);
        itemInfluence = GetComponent<ItemInfluence>();
        itemInfluence.InitItemInfluenceGrid(itemGrid, nodeRadius);
    }

    public override void Press()
    {
        base.Press();

        if (!CanInteract)
            return;

        itemInfluence.ShowStar();

        // 点击时，改变item占用的GridNode的状态
        // 改变 GridNode 的item类型
        if (curGridNodeList != null && state == ItemState.Using)
        {
            if (item.itemType == ItemType.Backpack)
            {
                fixedBackpack.bEnable = false;
                GridManager.instance.ChangeNodeState(curGridNodeList, NodeState.Empty);
            }
            else
            {
                GridManager.instance.ChangeItemInfoForNodes(curGridNodeList, ItemType.None, "");

                GridManager.instance.ChangeNodeState(curGridNodeList, NodeState.Backpack);
            }
            
            curGridNodeList = null;
            BackpackManager.instance.RemoveItem(item);
        }

        // 捡起储物箱中的物品时，更改为使用状态
        if (state == ItemState.Unused)
        {
            state = ItemState.Using;

            // 初始化旋转
            transform.rotation = Quaternion.identity;

            UpdateItemGrid(startRowCount, startColCount);

            Rigidbody2D rigi = this.GetComponent<Rigidbody2D>();

            rigi.bodyType = RigidbodyType2D.Kinematic;

            if (item.itemType == ItemType.Backpack)
            {
                BoxCollider2D collider = rigi.GetComponent<BoxCollider2D>();
                collider.isTrigger = true;
            }
        }
    }

    public override void Drag()
    { 
        base.Drag();

        if (!CanInteract)
            return;

        // 拖动
        //transform.position = new Vector3(CursorManager.instance.GetMousePos().x, CursorManager.instance.GetMousePos().y, 0);
        transform.position += CursorManager.instance.GetMouseMoveDelta();

        
    }

    public override void Release()
    {
        base.Release();

        if (!CanInteract)
            return;

        itemInfluence.HideStar();

        // 正在售卖
        if (state == ItemState.Sale)
        {
            if (item.itemType == ItemType.Backpack)
            {
                // 背包需要检查Node是否为空
                List<Node> gridNodeList = CheckItemGrid(NodeState.Empty);

                if (gridNodeList == null)
                {
                    StartCoroutine(MoveTo(itemPos));
                    return;
                }

                // 检查钱够不够
                if (!ShopManager.instance.PurchaseItem(item.itemName))
                {
                    StartCoroutine(MoveTo(itemPos));
                    return;
                }

                UseItem(gridNodeList, ItemType.None);

                // 改变占用Node状态为Backpack
                GridManager.instance.ChangeNodeState(gridNodeList, NodeState.Backpack);

                fixedBackpack.bEnable = true;

                curGridNodeList = gridNodeList;

                itemPos = CalculateItemPos(gridNodeList);
                StartCoroutine(MoveTo(itemPos));
            }
      
            else
            {
                // 非背包物品，检查Node是否都有背包
                List<Node> gridNodeList = CheckItemGrid(NodeState.Backpack);

                if (gridNodeList == null)
                {
                    StartCoroutine(MoveTo(itemPos));
                    return;
                }

                // 检查钱够不够
                if (!ShopManager.instance.PurchaseItem(item.itemName))
                {
                    StartCoroutine(MoveTo(itemPos));
                    return;
                }

                UseItem(gridNodeList, item.itemType);

                // 改变占用Node状态为Item
                GridManager.instance.ChangeNodeState(gridNodeList, NodeState.Item);

                curGridNodeList = gridNodeList;

                itemPos = CalculateItemPos(gridNodeList);
                StartCoroutine(MoveTo(itemPos));
            }

        }

        // 正在使用
        else if (state == ItemState.Using)
        {
            if (item.itemType == ItemType.Backpack)
            {
                List<Node> gridNodeList = CheckItemGrid(NodeState.Empty);

                if (gridNodeList == null)
                {
                    // 放入储物箱
                    StorageItem();
                    StartCoroutine(MoveTo(itemPos));
                    return;
                }

                UseItem(gridNodeList, ItemType.None);
                GridManager.instance.ChangeNodeState(gridNodeList, NodeState.Backpack);

                fixedBackpack.bEnable = true;

                curGridNodeList = gridNodeList;

                itemPos = CalculateItemPos(gridNodeList);
                StartCoroutine(MoveTo(itemPos));
            }
            else
            {
                List<Node> gridNodeList = CheckItemGrid(NodeState.Backpack);

                if (gridNodeList == null)
                {
                    // 放入储物箱
                    StorageItem();
                    StartCoroutine(MoveTo(itemPos));
                    return;
                }

                UseItem(gridNodeList, item.itemType);
                GridManager.instance.ChangeNodeState(gridNodeList, NodeState.Item);

                curGridNodeList = gridNodeList;

                itemPos = CalculateItemPos(gridNodeList);
                StartCoroutine(MoveTo(itemPos));
            }
        }

    }

    public override void Interact()
    {
        base.Interact();

        if (!CanInteract)
            return;

        // 反过来传参，相当于旋转
        UpdateItemGrid(colCount, rowCount);

        transform.Rotate(new Vector3(0, 0, 90), Space.Self);
    }

    /// <summary>
    /// 检查周围是否都有指定状态的网格。
    /// 背包需要Empty，其它道具需要Backpack
    /// </summary>
    List<Node> CheckItemGrid(NodeState requireState)
    {
        UpdateItemGrid(rowCount, colCount);

        List<Node> gridNodeList = new List<Node>();

        // 检查是否所有节点附近都有Node
        foreach (var itemNode in itemGrid)
        {
            Node gridNode = GridManager.instance.IsInNode(itemNode._worldPos, requireState);
            if (gridNode == null)
            {
                Debug.Log("Not In Node return..");

                return null;
            }

            gridNodeList.Add(gridNode);
        }
        return gridNodeList;
    }

    /// <summary>
    /// 移动到指定位置
    /// </summary>
    IEnumerator MoveTo(Vector3 position)
    {
        while (Vector3.Distance(transform.position, position) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, position, .1f);
            yield return null; 
        }

    }

    /// <summary>
    /// 旋转时更新行和列
    /// </summary>
    public void UpdateItemGrid(int row, int col)
    {
        rowCount = row;
        colCount = col;
        itemGrid = new Node[rowCount, colCount];
        gridStartPoint = transform.position + rowCount * nodeRadius * Vector3.up - colCount * nodeRadius * Vector3.right;


        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                Vector3 worldPoint = gridStartPoint - Vector3.up * (i * 2 + 1) * nodeRadius + Vector3.right * (j * 2 + 1) * nodeRadius;
                itemGrid[i, j] = new Node(worldPoint, i, j, NodeType.ItemNode);
                //Instantiate(NodeImagePrefab, worldPoint, Quaternion.identity, transform);
            }
        }
    }

    /// <summary>
    /// 计算节点平均位置
    /// </summary>
    Vector3 CalculateItemPos(List<Node> gridNodeList)
    {
        Vector3 pos = Vector3.zero;
        foreach (var gridNode in gridNodeList)
        {
            pos += gridNode._worldPos;
        }
        pos /= gridNodeList.Count;
        return pos;
    }

    void UseItem(List<Node> nodeList, ItemType itemType)
    {
        state = ItemState.Using;

        BackpackManager.instance.AddItem(item);
        GridManager.instance.ChangeItemInfoForNodes(nodeList, itemType, item.itemName);
    }

    void StorageItem()
    {
        itemPos = ShopManager.instance.storagePos.position;
        state = ItemState.Unused;

        Invoke("SetPhy", .7f);
    }

    void SetPhy()
    {
        Rigidbody2D rigi = this.GetComponent<Rigidbody2D>();

        rigi.bodyType = RigidbodyType2D.Dynamic;
        rigi.gravityScale = 1.0f;

        BoxCollider2D collider = rigi.GetComponent<BoxCollider2D>();
        collider.isTrigger = false;

        rigi.AddForce(new Vector2(100, 0));
    }

    void CheckInfluence()
    {
        // 点亮星星并获得加成
        int index = 0;
        foreach (var star in itemInfluence.starObj)
        {
            if (GridManager.instance.CheckItemTypeForNode(star.transform.position, item.influenceType))
                itemInfluence.LightStar(index);
            else
                itemInfluence.ClearStar(index);
            index++;
        }
    }

    private void OnDrawGizmos()
    {
        gridStartPoint = transform.position + startRowCount * nodeRadius * Vector3.up - startColCount * nodeRadius * Vector3.right;
        //Gizmos.DrawSphere(gridStartPoint, .08f);

        for (int i = 0; i < startRowCount; i++)
        {
            for (int j = 0; j < startColCount; j++)
            {
                Vector3 worldPoint = gridStartPoint - Vector3.up * (i * 2 + 1) * nodeRadius + Vector3.right * (j * 2 + 1) * nodeRadius;
                Gizmos.DrawWireCube(worldPoint, new Vector2(nodeRadius * 2, nodeRadius * 2));
                Gizmos.DrawSphere(worldPoint, .08f);
            }
        }
    }
}
