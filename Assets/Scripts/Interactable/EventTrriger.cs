using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 由该脚本来判断是否要求的物体到达目的地
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class EventTrriger : MonoBehaviour
{
    public UnityEvent unityEvent;
    public string targetobjectName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        #if UNITY_EDITOR
        Debug.Log(collision.gameObject.name + "进入触发器");
        #endif

        if (collision.gameObject.name == targetobjectName)
        {
            unityEvent.Invoke();
        }
    }
}
