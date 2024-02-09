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

// 背包和道具都有自己的Node
public enum NodeType
{
    GridNode,
    ItemNode,
    InfluenceNode
}

// 背包需要Empty，道具需要Backpack
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