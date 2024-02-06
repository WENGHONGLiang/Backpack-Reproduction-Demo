using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 网格节点
/// </summary>
public class Node
{
    public Vector3 _worldPos;   // 节点坐标

    public int _gridRow, _gridCol;  // 网格索引

    public NodeType _type;

    public NodeState _state;

    // 若背包 Node 内有道具，那么记录道具信息 // 用于道具间互相影响
    public ItemType _itemType;
    public string _itemName;

    public Node(Vector3 worldPos, int gridX, int gridY, NodeType type, NodeState state = NodeState.Empty, ItemType itemType = ItemType.None)
    {
        _worldPos = worldPos;
        _gridRow = gridX;
        _gridCol = gridY;
        _type = type;
        _state = state;
        _itemType = itemType;
    }   
}
