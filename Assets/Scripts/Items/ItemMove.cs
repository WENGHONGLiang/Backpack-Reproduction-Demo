using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ItemMove : Interactable
{
    Node[,] itemGrid;  // �����¼
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

    [Header("��������")]
    public int startRowCount;
    public int startColCount;
    public float nodeRadius = .25f;

    [Header("��������")]
    public int ShopIndex;   // ���̵����һ��
    public Item_SO item;



    private void Start()
    {
        // ��ʼ����������
        UpdateItemGrid(startRowCount, startColCount);

        // ��ʼ�������ܱ�����
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

        // ���ʱ���ı�itemռ�õ�GridNode��״̬
        // �ı� GridNode ��item����
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

        // ���������е���Ʒʱ������Ϊʹ��״̬
        if (state == ItemState.Unused)
        {
            state = ItemState.Using;

            // ��ʼ����ת
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

        // �϶�
        //transform.position = new Vector3(CursorManager.instance.GetMousePos().x, CursorManager.instance.GetMousePos().y, 0);
        transform.position += CursorManager.instance.GetMouseMoveDelta();

        
    }

    public override void Release()
    {
        base.Release();

        if (!CanInteract)
            return;

        itemInfluence.HideStar();

        // ��������
        if (state == ItemState.Sale)
        {
            if (item.itemType == ItemType.Backpack)
            {
                // ������Ҫ���Node�Ƿ�Ϊ��
                List<Node> gridNodeList = CheckItemGrid(NodeState.Empty);

                if (gridNodeList == null)
                {
                    StartCoroutine(MoveTo(itemPos));
                    return;
                }

                // ���Ǯ������
                if (!ShopManager.instance.PurchaseItem(item.itemName))
                {
                    StartCoroutine(MoveTo(itemPos));
                    return;
                }

                UseItem(gridNodeList, ItemType.None);

                // �ı�ռ��Node״̬ΪBackpack
                GridManager.instance.ChangeNodeState(gridNodeList, NodeState.Backpack);

                fixedBackpack.bEnable = true;

                curGridNodeList = gridNodeList;

                itemPos = CalculateItemPos(gridNodeList);
                StartCoroutine(MoveTo(itemPos));
            }
      
            else
            {
                // �Ǳ�����Ʒ�����Node�Ƿ��б���
                List<Node> gridNodeList = CheckItemGrid(NodeState.Backpack);

                if (gridNodeList == null)
                {
                    StartCoroutine(MoveTo(itemPos));
                    return;
                }

                // ���Ǯ������
                if (!ShopManager.instance.PurchaseItem(item.itemName))
                {
                    StartCoroutine(MoveTo(itemPos));
                    return;
                }

                UseItem(gridNodeList, item.itemType);

                // �ı�ռ��Node״̬ΪItem
                GridManager.instance.ChangeNodeState(gridNodeList, NodeState.Item);

                curGridNodeList = gridNodeList;

                itemPos = CalculateItemPos(gridNodeList);
                StartCoroutine(MoveTo(itemPos));
            }

        }

        // ����ʹ��
        else if (state == ItemState.Using)
        {
            if (item.itemType == ItemType.Backpack)
            {
                List<Node> gridNodeList = CheckItemGrid(NodeState.Empty);

                if (gridNodeList == null)
                {
                    // ���봢����
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
                    // ���봢����
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

        // ���������Σ��൱����ת
        UpdateItemGrid(colCount, rowCount);

        transform.Rotate(new Vector3(0, 0, 90), Space.Self);
    }

    /// <summary>
    /// �����Χ�Ƿ���ָ��״̬������
    /// ������ҪEmpty������������ҪBackpack
    /// </summary>
    List<Node> CheckItemGrid(NodeState requireState)
    {
        UpdateItemGrid(rowCount, colCount);

        List<Node> gridNodeList = new List<Node>();

        // ����Ƿ����нڵ㸽������Node
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
    /// �ƶ���ָ��λ��
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
    /// ��תʱ�����к���
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
    /// ����ڵ�ƽ��λ��
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
        // �������ǲ���üӳ�
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
