using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该脚本控制推箱子游戏中的玩家
/// </summary>
public class PlayerController : MonoBehaviour
{

    private Vector3 direction = Vector3.zero;   // 玩家的移动方向

    private bool canMove = true;    // 玩家是否可以移动


    private GameObject obj; // 玩家推动的物体
    private ObjectController objectController; // 箱子的控制器

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector3.right;
        }
        if (direction != Vector3.zero)
            canMove = CanCharacterMove(direction);

    }

    private void Move(Vector3 direction)
    {
        transform.Translate(direction);
    }

    /// <summary>
    /// 检测玩家是否可以移动
    /// </summary>
    private bool CanCharacterMove(Vector3 direction)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position + direction * 0.5f, direction, 1f);
        if (raycastHit2D)
        {
            string hitObjectTag = raycastHit2D.collider.gameObject.tag;
            Debug.Log(hitObjectTag);
            if (hitObjectTag == "Wall")
            {
#if UNITY_EDITOR
                Debug.Log("Wall");
#endif
                return false;
            }
            else if (hitObjectTag == "Box")
            {
                // 1. 玩家可以推动箱子
                // canBoxMove = true;
                // 2. 获取玩家推动的箱子
                obj = raycastHit2D.collider.gameObject;
                // 3. 获取箱子的控制器
                objectController = obj.GetComponent<ObjectController>();
                // 4. 检测箱子是否可以移动
                if (!objectController.CanObjectMove(direction))
                {
                    return false;
                }
                else
                { // 箱子可以移动
                    objectController.Move(direction);
                }
            }
            else if (hitObjectTag == "Destination")
            {
                Destination();
            }
        }

        return true;
    }


    private void FixedUpdate()
    {
        //更新玩家和箱子的位置
        if (canMove)
        {
            Move(direction);
            canMove = false;
        }

        direction = Vector3.zero;
    }

    private void Destination()
    {
        // 玩家到达终点
        // 1. 玩家到达终点后，玩家不可以移动
        canMove = false;
        // 2. 玩家到达终点后，玩家不可以推动箱子
        // canBoxMove = false;
        // 3. 玩家到达终点后，玩家的颜色变为绿色
        GetComponent<Renderer>().material.color = Color.green;
    }
}