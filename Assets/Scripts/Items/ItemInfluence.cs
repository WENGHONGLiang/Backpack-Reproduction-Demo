using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfluence : MonoBehaviour
{
    public List<int> SpecialNodeIndex;
    public GameObject starPrefab;
    public Transform starRoot;

    public Sprite starFull;
    public Sprite starEmpty;

    List<Node> itemInfluenceGrid;
    public List<GameObject> starObj;

    public Dictionary<int, Item_SO> influenceItems;

    private void Start()
    {
        influenceItems = new Dictionary<int, Item_SO>();
    }

    /// <summary>
    /// 遍历周围一圈的点
    /// </summary>
    public void InitItemInfluenceGrid(Node[,] itemGrid, float nodeRadius)
    {
        itemInfluenceGrid = new List<Node>();
        starObj = new List<GameObject>();
        int index = 0;
        foreach (var itemNode in itemGrid)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = 1; y > -2; y--)
                {
                    if(x == 0 && y == 0)
                        continue;

                    Vector3 newWorldPos = itemNode._worldPos + Vector3.up * x * 2 * nodeRadius + Vector3.right * y * 2 * nodeRadius;

                    bool bExist = false;
                    foreach (var itemInfluenceNode in itemInfluenceGrid)
                    {
                        // 已经存下来了
                        if (Vector3.Distance(itemInfluenceNode._worldPos, newWorldPos) < .1f)
                        {
                            bExist = true;
                            break;
                        }
                    }
                    if (bExist) continue;
                    foreach (var itemNodeCheck in itemGrid)
                    {
                        // 和道具本身网格重合
                        if (Vector3.Distance(itemNodeCheck._worldPos, newWorldPos) < .1f)
                        {
                            bExist = true;
                            break;
                        }
                    }
                    if (bExist) continue;

                    // 编辑器下所有点都显示，方便确定位置 // 运行时只保存需要的点位
                    Node node = new Node(newWorldPos, -1, -1, NodeType.InfluenceNode);
                    if (SpecialNodeIndex.Contains(index) && Application.isPlaying)
                    {
                        GameObject star = Instantiate(starPrefab, starRoot, true);
                        star.transform.position = newWorldPos;
                        star.transform.rotation = Quaternion.identity;
                        starObj.Add(star);
                        itemInfluenceGrid.Add(node);
                    }

                    if (!Application.isPlaying)
                    {
                        itemInfluenceGrid.Add(node);
                    }
                   index++;

                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (itemInfluenceGrid == null) return;

        Gizmos.color = Color.red;
        int index = 0;
        foreach (var influenceNode in itemInfluenceGrid)
        {
            if(SpecialNodeIndex.Contains(index) && !Application.isPlaying) Gizmos.color = Color.green;
            Gizmos.DrawSphere(influenceNode._worldPos, .07f);
            Gizmos.color = Color.red;
            index++;
        }   
    }

    public void ShowStar()
    {
        starRoot.gameObject.SetActive(true);
    }

    public void HideStar()
    {
        starRoot.gameObject.SetActive(false);
    }

    public List<GameObject> GetStars()
    {
        return starObj;
    }

    public void LightStar(int index)
    {
        SpriteRenderer starSprite = starObj[index].GetComponent<SpriteRenderer>();
        if (starSprite.sprite == starFull) return;

        starSprite.sprite = starFull;

        // 根据Node得到Influence的对象，对其加成
        Node node = GridManager.instance.GetNode(starObj[index].transform.position);
        if (node != null)
        {
            Item_SO so = ShopManager.instance.FindItemSOByName(node._itemName);
            if (so != null) 
            {
                influenceItems.Add(index, so);
            }
        }
        //Debug.Log(name + " : Add  " + influenceItems.Count);

    }

    public void ClearStar(int index)
    {
        SpriteRenderer starSprite = starObj[index].GetComponent<SpriteRenderer>();
        if (starSprite.sprite == starEmpty) return;

        starSprite.sprite = starEmpty;
        influenceItems.Remove(index);

        //Debug.Log(name + " : Clear " + influenceItems.Count);

    }
}
