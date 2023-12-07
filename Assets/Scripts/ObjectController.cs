using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该脚本控制推箱子游戏中的物体
/// </summary>
public class ObjectController : MonoBehaviour
{
    [SerializeField] private float centerOffset = 0.5f; // 物体的中心点偏移量
    [SerializeField] private float raycasetDistance = 0.8f; // 射线检测的距离
    [SerializeField] private float reraycastCenterOffset = 0.52f;
    public bool getGoal = false; // 该物体是否达成目的

    public bool haveShadowObject = false; // 该物体是否有影子物体
    public GameObject ShadowObject = null; // 影子物体，默认为空

    private ObjectController _shadowObjectController;   // 影子物体的控制器

    private PlayerController _nextPlayerController;

    private ObjectController _nextObjectController;

    private void Start()
    {
        getGoal = false;
        if (haveShadowObject)
        {
            _shadowObjectController = ShadowObject.GetComponent<ObjectController>();
        }
    }


    public void Move(Vector3 direction)
    {
        transform.Translate(direction);
        if (haveShadowObject)
        {
            if (direction == Vector3.up)
                ShadowObject.transform.Translate(Vector3.down);
            else if (direction == Vector3.down)
                ShadowObject.transform.Translate(Vector3.up);
            else if (direction == Vector3.left)
                ShadowObject.transform.Translate(Vector3.left);
            else if (direction == Vector3.right)
                ShadowObject.transform.Translate(Vector3.right);
            else if (direction == Vector3.zero)
                ShadowObject.transform.Translate(direction);
        }
    }

    /// <summary>
    /// 修正用来检测的方向，用于适应第四关的逻辑
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Vector3 LevelBoxMoveDirectionFix(Vector3 direction){
        //在这里进行位置的更新来适应shadowObject的移动检测(第四关的逻辑修正)
        if(haveShadowObject){
            if(direction == Vector3.up)
                direction = Vector3.down;
            else if(direction == Vector3.down)
                direction = Vector3.up;
            else if(direction == Vector3.left)
                direction = Vector3.left;
            else if(direction == Vector3.right)
                direction = Vector3.right;
            else if(direction == Vector3.zero)
                direction = Vector3.zero;
        }
        return direction;
    }


    public bool CanObjectMove(Vector3 direction)
    {

        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position + direction * centerOffset, direction, raycasetDistance);
        if (raycastHit2D)
        {
            string hitObjectTag = raycastHit2D.collider.gameObject.tag;
            #if UNITY_EDITOR
            Debug.Log("箱子射线检测击中"+hitObjectTag);
            #endif
            if (PlayerController.levelLogic == LevelLogic.Level_3)//第三关的逻辑
            {
                if (hitObjectTag == "Boarder")
                {
                    return false;
                }
                else if (hitObjectTag == "Player")
                {
                    return false;
                }
                else if (hitObjectTag == "Box")
                {
                    return false;
                }
                else if (hitObjectTag == "Floor" || hitObjectTag == "Wall") //箱子在边界上，且箱子会先打到边界(先于箱子、角色、边界)，那么在进行一次射线检测
                {
                    RaycastHit2D newraycastHit2D = Physics2D.Raycast(transform.position + direction * reraycastCenterOffset, direction, raycasetDistance / 2);
                    if (newraycastHit2D)
                    {
                        
#if UNITY_EDITOR    
                        Debug.Log("箱子再次射线检测击中物体"+newraycastHit2D.collider.gameObject.tag);
                        #endif
                        if (newraycastHit2D.collider.gameObject.tag == "Box")
                        {
                            return false;
                        }
                        else if (newraycastHit2D.collider.gameObject.tag == "Player")
                            return false;
                        else if(newraycastHit2D.collider.gameObject.tag == "Boarder")
                        {
                            return false;
                        }
                        else{
                            return true;
                        }

                    }

                }
            }
            else
            {
                if (hitObjectTag == "Wall")
                {
                    return false;
                }
                else if (hitObjectTag == "Box")
                {
                    if (raycastHit2D.collider.gameObject.TryGetComponent<PlayerController>(out _nextPlayerController))
                    {
                        _nextObjectController = raycastHit2D.collider.gameObject.GetComponent<ObjectController>();
                        return _nextObjectController.CanObjectMove(direction);
                    }
                    return false;
                }
                else if (hitObjectTag == "Player")
                {
                    return false;
                }
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
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * centerOffset, transform.position + Vector3.up * centerOffset + Vector3.up * raycasetDistance);
        Gizmos.DrawLine(transform.position + Vector3.down * centerOffset, transform.position + Vector3.down * centerOffset + Vector3.down * raycasetDistance);
        Gizmos.DrawLine(transform.position + Vector3.left * centerOffset, transform.position + Vector3.left * centerOffset + Vector3.left * raycasetDistance);
        Gizmos.DrawLine(transform.position + Vector3.right * centerOffset, transform.position + Vector3.right * centerOffset + Vector3.right * raycasetDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.up*reraycastCenterOffset,  transform.position + Vector3.up + Vector3.up*raycasetDistance / 2);
        Gizmos.DrawLine(transform.position + Vector3.down*reraycastCenterOffset,  transform.position + Vector3.down + Vector3.down*raycasetDistance / 2);
        Gizmos.DrawLine(transform.position + Vector3.left*reraycastCenterOffset,  transform.position + Vector3.left + Vector3.left*raycasetDistance / 2);
        Gizmos.DrawLine(transform.position + Vector3.right*reraycastCenterOffset,  transform.position + Vector3.right +Vector3.right*raycasetDistance / 2);
    }
}
