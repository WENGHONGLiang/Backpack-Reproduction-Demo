using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedBackpack : MonoBehaviour
{
    ItemMove item;
    public bool bEnable;
    void Awake()
    {
        item = GetComponent<ItemMove>();
    }

    // Fixme：改用其它方式固定背包
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Interactable" && bEnable)
        {
            item.CanInteract = false;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interactable" && bEnable)
            item.CanInteract = true;
    }
}
