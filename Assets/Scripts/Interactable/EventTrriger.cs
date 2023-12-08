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
    [Tooltip("进入触发器发生的事件")]public UnityEvent unityEvent;

    [Tooltip("离开触发器发生的事件")]public UnityEvent exitUnityEvent;
    [Tooltip("是否启动自动为物体加分")]public bool isAutoMatch = false;
    [Tooltip("触发触发器的物体名字")]public string targetobjectName;

    /// <summary>
    /// 进入触发器
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        #if UNITY_EDITOR
        Debug.Log(collision.gameObject.name + "进入触发器");
        #endif

        if (collision.gameObject.name == targetobjectName)
        {
            if(isAutoMatch)
                collision.gameObject.GetComponent<ObjectController>().getGoal = true;
            unityEvent.Invoke();
        }
    }

    /// <summary>
    /// 离开触发器
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision) {
        #if UNITY_EDITOR
        Debug.Log(collision.gameObject.name + "离开触发器");
        #endif
        if(collision.gameObject.name == targetobjectName)
        {
            if (isAutoMatch)
                collision.gameObject.GetComponent<ObjectController>().getGoal = false;
            exitUnityEvent.Invoke();
        }
    }
}
