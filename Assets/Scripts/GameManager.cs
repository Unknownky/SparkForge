using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 在这里进行游戏逻辑的更新
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Camera mainCamera;

    /// <summary>
    /// 由代码找到所有的Box tag的物体，根据总分判断是否达成目标
    /// </summary>
    private GameObject[] targetObjects;

    private bool isWin = true;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        isWin = true;
        mainCamera = Camera.main;
        mainCamera.orthographicSize = 9f;
    }


    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Dialogue1"))
                {
                    #if UNITY_EDITOR
                    Debug.Log("点击了对话物体");
                    #endif
                    TextManager.Instance.StartDialogueSystem("<#同事一>（把一踏文件扔在桌上）今天你把这沓文件做完，明天早上交到我办公桌上。<break><#我>啊……好的<break><#我>（今天又要加班了）<finish>");
                }
                if(hit.collider.CompareTag("Dialogue2")){
                    #if UNITY_EDITOR
                    Debug.Log("点击了对话物体");
                    #endif
                    TextManager.Instance.StartDialogueSystem("<#同事二>上次交给你的项目做完了吗？<break><#我>还差一点没有完成。<break><#同事二>一个小项目这么久还没做完(不耐烦)，赶快把文件交给我<break><#我>……好<break>");
                }
                if(hit.collider.CompareTag("Dialogue3")){
                    #if UNITY_EDITOR
                    Debug.Log("点击了对话物体");
                    #endif
                    TextManager.Instance.StartDialogueSystem("<#我>(看到桌子上的文件)这个不是我做的项目吗，怎么负责人是她的名字……");
                }
            }
        }
        #endif //用于代码测试


    }

    #region 事件触发函数
    public static void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }   

    public static void QuitGame()
    {
        #if UNITY_EDITOR
        Debug.Log("退出游戏");
        #endif

        // Application.Quit();
    }



    public static void StartDialogue(string dialogue)
    {
        TextManager.Instance.StartDialogueSystem(dialogue);
    }

    /// <summary>
    /// 打开设置
    /// </summary>
    public static void EnableSetting(){
        Debug.Log("打开设置");

    }
    #endregion

    public void CheckWin(){
        #if UNITY_EDITOR
        Debug.Log("检查胜利条件");
        #endif
        isWin = true;
        targetObjects = GameObject.FindGameObjectsWithTag("Box"); 
        foreach (var targetObject in targetObjects)
        {
            isWin = isWin && targetObject.GetComponent<ObjectController>().getGoal;
            #if UNITY_EDITOR
            Debug.Log(targetObject.name + " " + targetObject.GetComponent<ObjectController>().getGoal);
            Debug.Log(isWin);
            #endif
        }
        if(isWin){ // 如果所有的箱子都到达了目的地
        #if UNITY_EDITOR
            Debug.Log("游戏胜利");
#endif
            //TODO:进行剧情的播放
            // TextManager.Instance.StartDialogueSystem("<#我>（这个文件是我做的，为什么负责人是她的名字……）<break><#我>（我要去找她问个清楚）<finish>");
            LoadScene("Level_1-2");
        }
    }
}