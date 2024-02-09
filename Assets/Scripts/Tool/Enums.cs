public enum ItemType
{
    None,
    Weapon,
    Shield,
    Decoration,
    Nature,
    Potion,
    Pet,
    Gem,
    Backpack
}

public enum ItemState
{
    Sale,
    Using,
    Unused,
}

// �����͵��߶����Լ���Node
public enum NodeType
{
    GridNode,
    ItemNode,
    InfluenceNode
}

// ������ҪEmpty��������ҪBackpack
public enum NodeState
{
    Empty,
    Backpack,
    Item
}

public enum Scenes
{
    ManagerScene,
    ShopScene,
    BattleScene
}