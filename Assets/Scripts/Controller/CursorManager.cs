using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    #region Singleton
    public static CursorManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("more than one CursorManager");
            return;
        }
        instance = this;
    }
    #endregion

    Vector3 mouseWorldPos => Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    Vector3 lastMouseWorldPos = Vector3.zero;
    Vector3 mouseMoveDelta = Vector3.zero;
    Interactable interacter_left = null;
    Interactable interacter_right = null;


    public Vector3 GetMousePos()
    {
        return mouseWorldPos;
    }
    public Vector3 GetMouseMoveDelta()
    {
        return mouseMoveDelta;
    }

    Collider2D[] ObjectAtMousePosition()
    {
        return Physics2D.OverlapPointAll (mouseWorldPos);
    }

    void Update()
    {
        // 鼠标点击可交互对象 // 获取并记录
        if (Input.GetMouseButtonDown(0))
        {
            var interactObjects = ObjectAtMousePosition();
            Collider2D interactObject = null;
            if (interactObjects.Length > 1)
                interactObject = interactObjects[0].gameObject.layer != LayerMask.NameToLayer("Backpack") ? interactObjects[0] : interactObjects[1];
            else if(interactObjects.Length > 0)
                interactObject = interactObjects[0];



            if (interactObject && interactObject.tag == "Interactable")
            {
                interacter_left = interactObject.GetComponent<Interactable>();
                interacter_left.Press();

                GridManager.instance.ShowGrid();
            }

        }

        if (Input.GetMouseButton(0))
        {
            if (interacter_left)
            {
                interacter_left.Drag();
            }
        }

        // 鼠标松开可交互对象
        if (Input.GetMouseButtonUp(0))
        {
            if (interacter_left)
            {
                interacter_left.Release();
                interacter_left = null;
            }
            GridManager.instance.HideGrid();
        }

        // 左右键同时
        if (Input.GetMouseButtonDown(1))
        {
            if (interacter_left)
            {
                interacter_left.DragAndInteract();
            }
        }

        if (Input.GetMouseButtonDown(1) && !interacter_left)
        {
            var interactObjects = ObjectAtMousePosition();
            Collider2D interactObject = null;
            if (interactObjects.Length > 1)
                interactObject = interactObjects[0].gameObject.layer != LayerMask.NameToLayer("Backpack") ? interactObjects[0] : interactObjects[1];
            else if (interactObjects.Length > 0)
                interactObject = interactObjects[0];

            if (interactObject && interactObject.tag == "Interactable")
            {
                interacter_right = interactObject.GetComponent<Interactable>();
                interacter_right.Interact();
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
         
            if (interacter_right != null)
            {
                interacter_right.InteractComplete();
                interacter_right = null;
            }
        }

        mouseMoveDelta = mouseWorldPos - lastMouseWorldPos;

        mouseMoveDelta = mouseMoveDelta.magnitude > 9 ? Vector3.zero : mouseMoveDelta;

        lastMouseWorldPos = mouseWorldPos;
    }

}
