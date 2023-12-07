using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointClickForDialogue : MonoBehaviour
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)//如果点击的是鼠标左键
        {
            // 获取点击的场景物体
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if
            (
                Physics.Raycast(ray, out RaycastHit hitInfo) &&
                hitInfo.collider.gameObject == gameObject
            )
            {
                //TODO
                Debug.Log("点击了鼠标左键");
            }
        }
    }
}
