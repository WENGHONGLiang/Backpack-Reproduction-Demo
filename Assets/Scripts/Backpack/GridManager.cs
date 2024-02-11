using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// 划分网格并记录网格节点信息
/// </summary>
public class GridManager : MonoBehaviour
{
    #region Singleton
    public static GridManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("more than one GridManager");
            return;
        }
        InitGrid();
        instance = this;
    }
    #endregion

    Node[,] grid;  // 背包网格
    Vector3 gridStartPoint;

    [Header("调整网格数/网格半径 需要与物品对应")]
    public int gridCount_Row;
    public int gridCount_Col;
    public float nodeRadius = .25f;

    [Header("Node")]
    public Transform NodeImageParent;
    public GameObject NodeImagePrefab;

    public void ShowGrid()
    {
        NodeImageParent.gameObject.SetActive(true);
    }

    public void HideGrid()
    {
        if(NodeImageParent != null)
            NodeImageParent.gameObject.SetActive(false);
    }


    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other && other.tag == "Interactable")
    //    {
    //        ShowGrid();
    //    }
    //}
    //
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision && collision.tag == "Interactable")
    //    {
    //        HideGrid();
    //    }
    //}

    private void InitGrid()
    {
        grid = new Node[gridCount_Row, gridCount_Col];
        gridStartPoint = transform.position + gridCount_Row * nodeRadius * Vector3.up - gridCount_Col * nodeRadius * Vector3.right;

        for (int i = 0; i < gridCount_Row; i++)
        {
            for (int j = 0; j < gridCount_Col; j++)
            {
                Vector3 worldPoint = gridStartPoint - Vector3.up * (i * 2 + 1) * nodeRadius + Vector3.right * (j * 2 + 1) * nodeRadius;
                grid[i, j] = new Node(worldPoint, i, j, NodeType.GridNode);
                Instantiate(NodeImagePrefab, worldPoint, Quaternion.identity, NodeImageParent);
            }
        }
    }

    private void OnDrawGizmos()
    {
        gridStartPoint = transform.position + gridCount_Row * nodeRadius * Vector3.up - gridCount_Col * nodeRadius * Vector3.right;
        //Gizmos.DrawSphere(gridStartPoint, .5f);


        for (int i = 0; i < gridCount_Row; i++)
        {
            for (int j = 0; j < gridCount_Col; j++)
            {
                Vector3 worldPoint = gridStartPoint - Vector3.up * (i * 2 + 1) * nodeRadius + Vector3.right * (j * 2 + 1) * nodeRadius;
                Gizmos.DrawWireCube(worldPoint, new Vector2(nodeRadius * 2, nodeRadius * 2));
                Gizmos.DrawSphere(worldPoint, .1f);
            }
        }
    }

    public Node GetNode(Vector3 point)
    {
        float dx = point.x - gridStartPoint.x;
        float dy = gridStartPoint.y - point.y;

        int x = Mathf.FloorToInt(dx / (2 * nodeRadius));
        int y = Mathf.FloorToInt(dy / (2 * nodeRadius));

        if (x > gridCount_Col - 1 || x < 0 || y > gridCount_Row - 1 || y < 0)
            return null;

        return grid[y, x];
    }

    /// <summary>
    /// 检查点是否附近有Node
    /// 且Node需要是指定的状态
    /// </summary>
    public Node IsInNode(Vector3 point, NodeState requireState)
    {
        
        float dx = point.x - gridStartPoint.x;
        float dy = gridStartPoint.y - point.y;

        int x = Mathf.FloorToInt(dx / (2 * nodeRadius));
        int y = Mathf.FloorToInt(dy / (2 * nodeRadius));

        // 不在网格区域
        if(x > gridCount_Col - 1 || x < 0 || y > gridCount_Row - 1 || y < 0 || grid[y, x]._state != requireState)
            return null;

        return grid[y, x];
    }

    public bool CheckItemTypeForNode(Vector3 point, ItemType requiredType)
    {

        float dx = point.x - gridStartPoint.x;
        float dy = gridStartPoint.y - point.y;

        int x = Mathf.FloorToInt(dx / (2 * nodeRadius));
        int y = Mathf.FloorToInt(dy / (2 * nodeRadius));

        // 不在网格区域
        if (x > gridCount_Col - 1 || x < 0 || y > gridCount_Row - 1 || y < 0 || grid[y, x]._itemType != requiredType)
            return false;

        return true;
    }

    /// <summary>
    /// 改变Node为指定状态
    /// </summary>
    public void ChangeNodeState(List<Node> nodeList, NodeState state)
    {
        foreach (var node in nodeList)
        {
            grid[node._gridRow, node._gridCol]._state = state;
        }
    }
    
    /// <summary>
    /// 记录Node中存放的道具类型，用于道具间互相影响
    /// </summary>
    public void ChangeItemInfoForNodes(List<Node> nodeList, ItemType type, string itemName)
    {
        foreach (var node in nodeList)
        {
            grid[node._gridRow, node._gridCol]._itemType = type;
            grid[node._gridRow, node._gridCol]._itemName = itemName;
        }
    }

    /// <summary>
    /// 根据索引获得实际世界坐标
    /// </summary>
    public Vector3 GetNodePosByIndex(Vector2Int index)
    {
        return grid[index.y, index.x]._worldPos;
    }
}
