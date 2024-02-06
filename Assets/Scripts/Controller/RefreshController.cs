using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshController : Interactable
{
    Vector3 startPos;
    bool bMoveToStart;
    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (bMoveToStart && Vector3.Distance(transform.position, startPos) > .01f)
            transform.position = Vector3.Lerp(transform.position, startPos, .1f);

        else if (bMoveToStart && Vector3.Distance(transform.position, startPos) <= .01f)
        {
            bMoveToStart = false;
            Invoke("ResumeCanInteract", 1f);
        }
    }

    public override void Press()
    {
        base.Press();

        bMoveToStart = false;
    }

    public override void Drag()
    {
        base.Drag();

        if (!CanInteract)
            return;

        Vector3 mouseMoveDelta = CursorManager.instance.GetMouseMoveDelta();

        if (mouseMoveDelta.y > 0) return;

        float tx = transform.position.x;
            
        transform.position += mouseMoveDelta;
        transform.position = new Vector3(tx, transform.position.y, 0);

        // 拉动足够距离 // 刷新商店
        if ((startPos.y - transform.position.y) > 1)
        {
            CanInteract = false;
            bMoveToStart = true;

            // 刷新商店
            if(PlayerState.instance.CostMoney(1))
                ShopManager.instance.RefreshItem();
        }
    }
    public override void Release()
    {
        base.Release();

        bMoveToStart = true;
    }

    void ResumeCanInteract()
    {
        CanInteract = true;
    }
}
