using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool CanInteract = true;
    // ���
    public virtual void Press()
    {
        if (!CanInteract)
            return;
        Debug.Log("Press : " + transform.name);
    }

    public virtual void Drag()
    {
        if (!CanInteract)
            return;
        Debug.Log("Drag : " + transform.name);
    }

    public virtual void Release()
    {
        if (!CanInteract)
            return;
        Debug.Log("Release : " + transform.name);
    }

    // �Ҽ�
    public virtual void Interact()
    {
        if (!CanInteract)
            return;
        Debug.Log("Interact : " + transform.name);
    }

}