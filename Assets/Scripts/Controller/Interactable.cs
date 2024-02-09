using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool CanInteract = true;
    // ×ó¼ü
    public virtual void Press()
    {
        if (!CanInteract)
            return;
        //Debug.Log("Press : " + transform.name);
    }

    public virtual void Drag()
    {
        if (!CanInteract)
            return;
        //Debug.Log("Drag : " + transform.name);
    }

    public virtual void Release()
    {
        if (!CanInteract)
            return;
        //Debug.Log("Release : " + transform.name);
    }

    // ÍÏ×§Ê±ÓÒ¼ü
    public virtual void DragAndInteract()
    {
        if (!CanInteract)
            return;
        //Debug.Log("Interact : " + transform.name);
    }

    // ÓÒ¼üµã»÷
    public virtual void Interact()
    {
        if (!CanInteract)
            return;
        //Debug.Log("Interact : " + transform.name);
    }

    // ÓÒ¼üËÉ¿ª
    public virtual void InteractComplete()
    {
        if (!CanInteract)
            return;
        //Debug.Log("InteractComplete : " + transform.name);
    }


}
