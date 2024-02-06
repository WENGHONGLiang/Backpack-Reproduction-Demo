using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ڵ�
/// </summary>
public class Node
{
    public Vector3 _worldPos;   // �ڵ�����

    public int _gridRow, _gridCol;  // ��������

    public NodeType _type;

    public NodeState _state;

    // ������ Node ���е��ߣ���ô��¼������Ϣ // ���ڵ��߼以��Ӱ��
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
