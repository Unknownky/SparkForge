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
        RaycastHit2D raycastTry = Physics2D.Raycast(transform.position + direction * 0.5f, direction, 1f);
        if (raycastTry)
        {
            RaycastHit2D[] raycastHit2Ds = Physics2D.RaycastAll(transform.position + direction * 0.5f, direction, 1f);
            foreach (var raycastHit2D in raycastHit2Ds)
            {
#if UNITY_EDITOR
                Debug.Log(raycastHit2D.collider.gameObject.name);
#endif
                string hitObjectTag = raycastHit2D.collider.gameObject.tag;
                Debug.Log(hitObjectTag);

                switch (hitObjectTag)
                {
                    case "Wall":
                        return false;
                    case "Box":
                        obj = raycastHit2D.collider.gameObject;
                        objectController = obj.GetComponent<ObjectController>();
                        if (!objectController.CanObjectMove(direction))
                        {
                            return false;
                        }
                        else
                        {
                            objectController.Move(direction);
                            return true;
                        }
                    case "Destination":
                        Destination();
                        break;
                    default:
                        break;
                }
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