using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 该脚本控制推箱子游戏中的玩家
/// </summary>
public class PlayerController : MonoBehaviour
{

    [SerializeField] private float centerOffset = 0.5f; // 玩家的中心点偏移量  
    [SerializeField] private float raycasetDistance = 0.8f; // 射线检测的距离

    /// <summary>
    /// 人物是否进行镜像逻辑
    /// </summary>
    public bool inverseControl;

    /// <summary>
    /// 当前角色是否为影子
    /// </summary>
    public bool isUnmoveable;

    private Vector3 direction = Vector3.zero;   // 玩家的移动方向

    private bool canMove = true;    // 玩家是否可以移动

    private GameObject obj; // 玩家推动的物体

    private ObjectController objectController; // 箱子的控制器

    private Animator animator; // 玩家的动画控制器

    /// <summary>
    /// 关卡中所有的可互动物品
    /// </summary>
    private List<GameObject> levelObjects; // 该关卡中所有的物体，包括角色等，之后再添加，用于位置的回溯

    /// <summary>
    /// 当前关卡的逻辑
    /// </summary>
    public static LevelLogic levelLogic;

    public bool isObject = false;


    private void Start()
    {
        //判断当前的关卡，如果为Level_1开头的场景，则加载Level_1的输入逻辑
        //根据关卡加载对应的输入逻辑
        levelLogic = GameManager.Instance.levelLogic;//设置当前关卡的逻辑
        if(!isObject)
            animator = GetComponent<Animator>();
    }


    private void Update()
    {
        switch (levelLogic)
        {
            case LevelLogic.Level_0:
                Level_1_Input();
                break;
            case LevelLogic.Level_1:
                Level_1_Input();
                break;
            case LevelLogic.Level_2:
                Level_2_Input();
                break;
            case LevelLogic.Level_3:
                Level_3_Input();
                break;
            case LevelLogic.Level_4:
                Level_4_Input();
                break;
            default:
                break;
        }
    }

    //  1.在经典推箱子机制的基础上，推动箱子的时候，镜像位置的箱子-影永远保持在对应箱子的镜像位置。
    //2.踩到切换砖的时候切换角色的操纵权，原角色则挂机，失去控制。
    private void Level_4_Input()
    {
        if (isUnmoveable)
        {
            return;
        }
        else
        {
            Level_1_Input();
        }
    }

    //同时移动两个角色，两个角色的操作镜像
    private void Level_3_Input()
    {
        //镜像移动逻辑
        if (inverseControl)
        {
            InverseLevel_1_Input();
        }
        else
        {//正常移动逻辑
            Level_1_Input();
        }
    }

    //按方向键的时候，所有可移动物品往同一方向一起移动一格。人物不动。
    private void Level_2_Input()
    {
        if (gameObject.tag == "Player")
        {
            if (isUnmoveable)
            {
                return;
            }
            else
            {
                Level_1_Input();
            }
        }
        else if (gameObject.tag == "Box")
        {
            Level_1_Input();
        }
    }

    private void InverseLevel_1_Input()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector3.up;
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
            canMove = CanVirtualCharacterMove(transform.position, direction);


    }


    /// <summary>
    /// 最普通的角色移动逻辑
    /// </summary>
    private void Level_1_Input()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
#if UNITY_EDITOR
            Debug.Log("角色检测到按下W键");
#endif
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
            canMove = CanVirtualCharacterMove(transform.position, direction);

    }

    private void Move(Vector3 direction)
    {
        transform.Translate(direction);
    }

    /// <summary>
    /// 检测虚拟人物是否可以移动
    /// </summary>
    /// <param name="startPosition">虚拟人物的中心位置</param>
    /// <param name="direction">移动方向</param>
    /// <returns></returns>
    private bool CanVirtualCharacterMove(Vector3 startPosition, Vector3 direction)
    {
        RaycastHit2D raycastTry = Physics2D.Raycast(startPosition + direction * centerOffset, direction, raycasetDistance);
        if (raycastTry)
        {
            RaycastHit2D[] raycastHit2Ds = Physics2D.RaycastAll(startPosition + direction * centerOffset, direction, raycasetDistance);
            foreach (var raycastHit2D in raycastHit2Ds)
            {
#if UNITY_EDITOR
                Debug.Log("人物射线检测击中物体名字" + raycastHit2D.collider.gameObject.name);
#endif
                string hitObjectTag = raycastHit2D.collider.gameObject.tag;
#if UNITY_EDITOR
                Debug.Log("人物射线检测击中物体tag" + hitObjectTag);
#endif
                switch (hitObjectTag)
                {
                    case "Boarder":
                        return false;
                    // case "PublicArea":
                    //     //以目标的位置的中心位置为起点，以当前的方向为方向，再次进行射线检测
                    //     return CanVirtualCharacterMove(raycastHit2D.collider.gameObject.transform.position, raycasetDistance * direction);
                    case "Wall":
                        if (inverseControl) //反向代码的逻辑
                            return true;
                        return false;
                    case "Floor":
                        if (inverseControl)
                        {
                            return false;
                        }
                        return true;
                    case "ShadowBox":
                        return false;

                    case "Box":
                        obj = raycastHit2D.collider.gameObject;
                        objectController = obj.GetComponent<ObjectController>();
                        if (!objectController.CanObjectMove(direction))
                        {
                            return false;
                        }
                        else //如果当前推动的箱子可以移动
                        {

                            if (!isObject)
                            {
                                if (objectController.haveShadowObject)
                                {
                                    Vector3 newdirection = objectController.LevelBoxMoveDirectionFix(direction);//修正用于检测的方向
                                    if (objectController.ShadowObject.GetComponent<ObjectController>().CanObjectMove(newdirection))
                                    {
                                        objectController.Move(direction);
                                    }
                                }
                                else
                                {
                                    objectController.Move(direction);
                                }
                            }
                            return true;
                        }
                    case "Destination":
                        Destination();
                        break;
                    case "Player":
                        return false;
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
            animator?.SetFloat("horizontal",direction.x);
            animator?.SetFloat("vertical",direction.y);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * centerOffset, transform.position + Vector3.up * centerOffset + Vector3.up * raycasetDistance);
        Gizmos.DrawLine(transform.position + Vector3.down * centerOffset, transform.position + Vector3.down * centerOffset + Vector3.down * raycasetDistance);
        Gizmos.DrawLine(transform.position + Vector3.left * centerOffset, transform.position + Vector3.left * centerOffset + Vector3.left * raycasetDistance);
        Gizmos.DrawLine(transform.position + Vector3.right * centerOffset, transform.position + Vector3.right * centerOffset + Vector3.right * raycasetDistance);
    }
}