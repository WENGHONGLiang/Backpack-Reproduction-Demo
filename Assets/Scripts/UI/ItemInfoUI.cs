using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfoUI : MonoBehaviour
{

    public TMP_Text itemName;
    public TMP_Text itemDescription;

    public void UpdateItemInfoUI(Item_SO item)
    {
        itemName.text = item.itemName;
        itemDescription.text = item.itemDescription;
    }
}
