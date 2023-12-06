using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该脚本控制推箱子游戏中的物体
/// </summary>
public class ObjectController : MonoBehaviour
{
    [SerializeField]private float centerOffset = 0.5f; // 物体的中心点偏移量
    [SerializeField]private float raycasetDistance = 0.8f; // 射线检测的距离

    public bool getGoal = false; // 该物体是否达成目的

    private void Start() {
        getGoal = false;
    }

    public void Move(Vector3 direction)
    {
        transform.Translate(direction);
    }

    public bool CanObjectMove(Vector3 direction)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position + direction * centerOffset, direction, raycasetDistance);
        if (raycastHit2D)
        {
            string hitObjectTag = raycastHit2D.collider.gameObject.tag;
            if (hitObjectTag == "Wall")
            {
                return false;
            }
            else if (hitObjectTag == "Box")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return true;

    }

    //暴露给Triger事件来触发
    public void ObjectArriveDestination()
    {
#if UNITY_EDITOR
        Debug.Log(gameObject.name + "到达目的地");
#endif
        getGoal = true;
        GameManager.Instance.CheckWin();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * centerOffset, transform.position + Vector3.up * centerOffset + Vector3.up * raycasetDistance);
        Gizmos.DrawLine(transform.position + Vector3.down * centerOffset, transform.position + Vector3.down * centerOffset + Vector3.down * raycasetDistance);
        Gizmos.DrawLine(transform.position + Vector3.left * centerOffset, transform.position + Vector3.left * centerOffset + Vector3.left * raycasetDistance);
        Gizmos.DrawLine(transform.position + Vector3.right * centerOffset, transform.position + Vector3.right * centerOffset + Vector3.right * raycasetDistance);
    }
}
