using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 在这里进行游戏逻辑的更新
/// </summary>
public class GameManager : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Dialogue"))
                {
                    Debug.Log("点击了对话物体");
                    TextManager.Instance.StartDialogueSystem("<#小明>你好呀，今天的天气真好！<break><#小李>是呀，这正是散步的好日子<rain><break><#小明>……<break><#小李>……<finish>");
                }
            }
        }
    }
}