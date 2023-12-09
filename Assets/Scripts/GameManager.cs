using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 在这里进行游戏逻辑的更新
/// </summary>
/// 

public enum LevelLogic
{
    Level_0,
    Level_1,
    Level_2,
    Level_3,
    Level_4,
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Sprite mainSceneLightBackground;
    [SerializeField] private Sprite mainSceneNightBackground;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private DialogueContainer dialogueContainer;

    public GameObject barrier;
    private Camera mainCamera;

    private GameObject mainSceneBackground;
    /// <summary>
    /// 由代码找到所有的Box tag的物体，根据总分判断是否达成目标
    /// </summary>
    public GameObject[] targetObjects;

    /// <summary>
    /// 游戏通过这个变量来判断是否胜利
    /// </summary>
    [Tooltip("是否达成通关条件")]public bool isWin = false;

    /// <summary>
    /// 当前关卡的逻辑
    /// </summary>
    [HideInInspector] public LevelLogic levelLogic;

    #region 通关参数
    private GameObject[] _level_23Objects;

    #endregion

    /// <summary>
    /// 用于存储对话的字典
    /// </summary>
    private Dictionary<string, Dialogue> dialogueDictionary;

    // 异步加载操作对象
    private static AsyncOperation asyncOperation;

    private string currentSceneName => SceneManager.GetActiveScene().name;

    /// <summary>
    /// 是否已经显示过对话，避免重复对话
    /// </summary>
    private bool isResetCondition;


    private void Awake()
    {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
            Instance.isWin = false;
            mainCamera = Camera.main;
            mainCamera.orthographicSize = 9f;
            mainSceneBackground = GameObject.Find("MainSceneBackground");
        #if UNITY_EDITOR
            // BackgroundTest();
        #endif
            InitDictionary();
            //设置音量
            AudioListener.volume = PlayerPrefs.GetFloat("volume", 1f);//设置音量
    }

    private void InitDictionary()
    {
        Instance.dialogueDictionary = new Dictionary<string, Dialogue>();
        foreach (var dialogue in dialogueContainer?.dialogues)
        {
            Instance.dialogueDictionary.Add(dialogue.dialogueName, dialogue);
#if UNITY_EDITOR
            Debug.Log("添加了对话" + dialogue.dialogueName);
#endif
        }
    }

    private Dialogue GetDialogue(string dialogueName)
    {
        return Instance.dialogueDictionary[dialogueName];
    }

#if UNITY_EDITOR

    private void Start()
    {
#if UNITY_EDITOR
        Debug.Log("当前关卡" + SceneManager.GetActiveScene().name);
#endif
        //判断当前的关卡，如果为Level_1开头的场景，则加载Level_1的输入逻辑
        //根据关卡加载对应的输入逻辑
        MatchCurrentLevelLogic();
        Instance.targetObjects = GameObject.FindGameObjectsWithTag("Box");
        LoadCurrentLevelLogic(levelLogic);
    }

    
    IEnumerator LoadSceneAfterAs()
    {
        yield return new WaitForSecondsRealtime(2f);
        GameManagerInit(Instance.isResetCondition);
    }

    /// <summary>
    /// 用来在切换场景时进行初始化
    /// </summary>
    private void GameManagerInit(bool isReset)
    {
#if UNITY_EDITOR
        Debug.Log("当前关卡" + SceneManager.GetActiveScene().name);
        Debug.Log("进行GameManager的初始化,是否重置" + isReset);
#endif
        isWin = false;
        Instance.isWin = false;
        Instance.isResetCondition = isReset;
        //判断当前的关卡，如果为Level_1开头的场景，则加载Level_1的输入逻辑
        //根据关卡加载对应的输入逻辑
        MatchCurrentLevelLogic();
        Instance.targetObjects = GameObject.FindGameObjectsWithTag("Box");
        LoadCurrentLevelLogic(levelLogic);
    }

    private void MatchCurrentLevelLogic()
    {
        //判断当前的关卡，如果为Level_1开头的场景，则加载Level_1的输入逻辑
        //根据关卡加载对应的输入逻辑
        if (SceneManager.GetActiveScene().name.StartsWith("Level_0"))
        {
            //加载Level_1的输入逻辑
            Instance.levelLogic = LevelLogic.Level_0;
        }
        else if (SceneManager.GetActiveScene().name.StartsWith("Level_1"))
        {
            //加载Level_1的输入逻辑
            Instance.levelLogic = LevelLogic.Level_1;
        }
        else if (SceneManager.GetActiveScene().name.StartsWith("Level_2"))
        {
            //加载Level_2的输入逻辑
            Instance.levelLogic = LevelLogic.Level_2;
        }
        else if (SceneManager.GetActiveScene().name.StartsWith("Level_3"))
        {
            //加载Level_3的输入逻辑
            Instance.levelLogic = LevelLogic.Level_3;
        }
        else if (SceneManager.GetActiveScene().name.StartsWith("Level_4"))
        {
            //加载Level_4的输入逻辑
            Instance.levelLogic = LevelLogic.Level_4;
        }
    }

    private void LoadCurrentLevelLogic(LevelLogic levelLogic)
    {
        switch (levelLogic)
        {
            case LevelLogic.Level_0:
                Level_01Init();
                break;
            case LevelLogic.Level_1:
                if (currentSceneName == "Level_1-1")
                    Level_11Init();
                else if (currentSceneName == "Level_1-2")
                    Level_12Init();
                else if (currentSceneName == "Level_1-3")
                    Level_13Init();
                break;
            case LevelLogic.Level_2:
                if (currentSceneName == "Level_2-1")
                    Level_21Init();
                else if (currentSceneName == "Level_2-2")
                    Level_22Init();
                else if (currentSceneName == "Level_2-3")
                    Level_23Init();
                break;
            case LevelLogic.Level_3:
                if (currentSceneName == "Level_3-1")
                    Level_31Init();
                else if (currentSceneName == "Level_3-2")
                    Level_32Init();
                else if (currentSceneName == "Level_3-3")
                    Level_33Init();
                break;
            case LevelLogic.Level_4:
                Level_4XInit();
                break;
            default:
                break;
        }
    }



    private void BackgroundTest()
    {
        PlayerPrefs.SetInt("GameStatus", 1);
    }
#endif

    /// <summary>
    /// 根据游戏状态设置主场景的背景，0为白天，1为夜晚，使用PlayerPrefs来存储
    /// </summary>
    private void SetMainSceneBackground()
    {
        if (PlayerPrefs.GetInt("level_1") == 1 && PlayerPrefs.GetInt("level_2") == 1 && PlayerPrefs.GetInt("level_3") == 1 && PlayerPrefs.GetInt("level_4") == 1)
        {
            PlayerPrefs.SetInt("GameStatus", 1);
        }
        else
        {
            PlayerPrefs.SetInt("GameStatus", 0);
        }

        if (PlayerPrefs.GetInt("GameStatus") == 0)
        {
            mainSceneBackground.GetComponent<SpriteRenderer>().sprite = mainSceneLightBackground;
        }
        else if (PlayerPrefs.GetInt("GameStatus") == 1)
        {
            mainSceneBackground.GetComponent<SpriteRenderer>().sprite = mainSceneNightBackground;
            barrier.SetActive(false);

        }
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
                if (hit.collider.CompareTag("Dialogue2"))
                {
#if UNITY_EDITOR
                    Debug.Log("点击了对话物体");
#endif
                    TextManager.Instance.StartDialogueSystem("<#同事二>上次交给你的项目做完了吗？<break><#我>还差一点没有完成。<break><#同事二>一个小项目这么久还没做完(不耐烦)，赶快把文件交给我<break><#我>……好<break>");
                }
                if (hit.collider.CompareTag("Dialogue3"))
                {
#if UNITY_EDITOR
                    Debug.Log("点击了对话物体");
#endif
                    TextManager.Instance.StartDialogueSystem("<#我>(看到桌子上的文件)这个不是我做的项目吗，怎么负责人是她的名字……");
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Instance.isResetCondition = true;
            //重新加载场景，使用GameManager的方法
            LoadScene(SceneManager.GetActiveScene().name);
        }

        if(Input.GetKeyDown(KeyCode.C)){
            PlayerPrefs.SetInt("GameStatus", 1);
#if UNITY_EDITOR
            Debug.Log(PlayerPrefs.GetInt("GameStatus"));
            mainSceneBackground.GetComponent<SpriteRenderer>().sprite = mainSceneNightBackground;
            barrier.SetActive(false);
            #endif
        }

#endif //用于代码测试



        if (Input.GetKeyDown(KeyCode.Escape)) //退回主场景
        {
            ReturnToMainScene();
        }
        if (Input.GetKeyDown(KeyCode.O))    //直接修改胜利条件
        {
            Instance.isWin = true;
        }
        if(Input.GetKeyDown(KeyCode.P)){    //直接加载下一关
            LoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //重新加载场景
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }




    #region 事件触发函数
    public void ReturnToMainScene()
    {
        ResetGameStatus();
        LoadScene("Level_0-1");
    }

    public void LoadNextScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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
    public static void EnableSetting()
    {
        Debug.Log("打开设置");

    }

    /// <summary>
    /// 达成胜利条件
    /// </summary>
    public static void SetWin()
    {
        Instance.isWin = true;
    }

    public static void ResetWin()
    {
        Instance.isWin = false;
    }

    #endregion

    #region 辅助函数
    private static void ResetGameStatus()
    {
        Destroy(GameObject.Find("DialogGroup")); //删除游戏管理组件
    }
    #endregion

    public void CheckWin()
    {
        Instance.targetObjects = GameObject.FindGameObjectsWithTag("Box");

#if UNITY_EDITOR
        Debug.Log("检查胜利条件");
#endif
        bool temp = true;
        foreach (var targetObject in Instance.targetObjects)
        {
            temp = temp && targetObject.GetComponent<ObjectController>().getGoal;
#if UNITY_EDITOR
            Debug.Log(targetObject.name + " " + targetObject.GetComponent<ObjectController>().getGoal);
#endif
        }
        Instance.isWin = temp;
#if UNITY_EDITOR
        Debug.Log("CheckWin中当前胜利状态" + isWin);
        Debug.Log("在CheckWin中检查Instance的胜利条件" + GameManager.Instance.isWin);
#endif

        if (Instance.isWin)
        { // 如果所有的箱子都到达了目的地
#if UNITY_EDITOR
            Debug.Log("游戏胜利");
#endif
            //TODO:进行剧情的播放
            // TextManager.Instance.StartDialogueSystem("<#我>（这个文件是我做的，为什么负责人是她的名字……）<break><#我>（我要去找她问个清楚）<finish>");
            // LoadScene("Level_1-2");
        }
    }

    #region 用于第四关的逻辑控制
    public void ChangeControl()
    {
        PlayerController playerController = GameObject.Find("player").GetComponent<PlayerController>();
        PlayerController shadowPlayerController = GameObject.Find("shadowplayer").GetComponent<PlayerController>();
        playerController.isUnmoveable = !playerController.isUnmoveable;//更改玩家的可移动状态
        shadowPlayerController.isUnmoveable = !shadowPlayerController.isUnmoveable;//更改影子的可移动状态
#if UNITY_EDITOR
        Debug.Log("反转控制");
#endif
    }
    #endregion


    #region 关卡初始化逻辑
    private void Level_01Init()
    {
        SetMainSceneBackground();
    }

    private void Level_11Init()
    {
#if UNITY_EDITOR
        Debug.Log("初始化Level_11");
#endif
        
        TextManager.Instance.StartDialogueSystem(GetDialogue("1-A").dialogue);

    }

    private void Level_12Init()
    {
#if UNITY_EDITOR
        Debug.Log("初始化Level_12");
#endif
        
        TextManager.Instance.StartDialogueSystem(GetDialogue("1-B").dialogue);

    }

    private void Level_13Init()
    {
#if UNITY_EDITOR
        Debug.Log("初始化Level_13");
#endif
        
        TextManager.Instance.StartDialogueSystem(GetDialogue("1-C").dialogue);

    }

    private void Level_21Init()
    {
#if UNITY_EDITOR
        Debug.Log("初始化Level_21");
#endif
        TextManager.Instance.StartDialogueSystem(GetDialogue("2-A").dialogue);

    }

    private void Level_22Init()
    {
#if UNITY_EDITOR
        Debug.Log("初始化Level_22");
#endif
 
        TextManager.Instance.StartDialogueSystem(GetDialogue("2-B").dialogue);

    }

    private void Level_23Init()
    {
#if UNITY_EDITOR
        Debug.Log("初始化Level_23");
#endif
        foreach (var box in targetObjects)
        {
            box.GetComponent<ObjectController>().getGoal = false; //全部设置为到达目的地，进入判定区域时设置为false，出去时设置为true
        }
        TextManager.Instance.StartDialogueSystem(GetDialogue("2-C").dialogue);

    }

    private void Level_31Init()
    {
#if UNITY_EDITOR
        Debug.Log("初始化Level_31");
#endif
        // TextManager.Instance.StartDialogueSystem(GetDialogue("3-A").dialogue);

        TextManager.Instance.StartDialogueSystem(GetDialogue("2-C").dialogue);

    }

    private void Level_32Init()
    {
#if UNITY_EDITOR
        Debug.Log("初始化Level_32");
#endif
        TextManager.Instance.StartDialogueSystem(GetDialogue("3-B").dialogue);

    }

    private void Level_33Init()
    {
#if UNITY_EDITOR
        Debug.Log("初始化Level_33");
#endif
        TextManager.Instance.StartDialogueSystem("<#作者>剧情3-C还未完成<finish>");

    }

    public GameObject shadowPlayer;

    private void Level_4XInit()
    {
#if UNITY_EDITOR
        Debug.Log("初始化Level_41"+"播放剧情4-A");
#endif

        shadowPlayer.GetComponent<PlayerController>().isUnmoveable = true;
        foreach (var box in Instance.targetObjects)
        {
            box.GetComponent<ObjectController>().getGoal = false; //全部设置为到达目的地，进入判定区域时设置为false，出去时设置为true
        }
            TextManager.Instance.StartDialogueSystem("<#作者>剧情4-A还未完成<finish>");

    }


    #endregion


    //TODO
    #region 各个关卡胜利进行逻辑
    public void Level_11Win()
    {
#if UNITY_EDITOR
        Debug.Log("检查胜利条件" + GameManager.Instance.isWin);
#endif

        if (GameManager.Instance.isWin)
        {
            //TODO:进行剧情的播放
            LoadScene("Level_1-2");
        }
    }

    public void Level_12Win()
    {
        if (GameManager.Instance.isWin)
        {
            //TODO:进行剧情的播放
            LoadScene("Level_1-3");
        }

    }
    public void Level_13Win()
    {

        PlayerPrefs.SetInt("level_1", 1);
        //TODO:进行剧情的播放
        // TextManager.Instance.StartDialogueSystem("<#作者>1-D剧情还未完成<finish>");
        LoadScene("Level_0-1");
        // StartCoroutine(WaitUntilDialogueFinishThenLoadScene("Level_0-1"));
    
    }

    public void Level_21Win()
    {
        #if UNITY_EDITOR
        Debug.Log("在Levelwin中检查Instance的胜利条件" + GameManager.Instance.isWin);
        Debug.Log("检查胜利条件" + isWin);
        #endif
        if (GameManager.Instance.isWin)
        {
            LoadScene("Level_2-2");
        }
    }

    public void Level_22Win()
    {
        #if UNITY_EDITOR
        Debug.Log("检查Instance的胜利条件" + GameManager.Instance.isWin);
        #endif
        if (GameManager.Instance.isWin)
        {
            LoadScene("Level_2-3");
        }
    }

    public void Level_23Win()
    {
        foreach (var box in Instance.targetObjects)
        {
            if (box.GetComponent<ObjectController>().getGoal == false)
            {
                return;
            }
        }
        //TODO:进行剧情的播放
        // TextManager.Instance.StartDialogueSystem(GetDialogue("2-D").dialogue);
        // StartCoroutine(WaitUntilDialogueFinishThenLoadScene("Level_0-1"));
        PlayerPrefs.SetInt("level_2", 1);
        LoadScene("Level_0-1");
    }

    public void Level_31Win()
    {
        if (GameManager.Instance.isWin)
        {
            #if UNITY_EDITOR
            Debug.Log("level31检查胜利条件" + isWin);
            #endif
            PlayerPrefs.SetInt("level_3", 1);
            LoadScene("Level_0-1");
        }
    }

    public void Level_32Win()
    {
        if (GameManager.Instance.isWin)
        {

        }
    }

    public void Level_33Win()
    {
        if (GameManager.Instance.isWin)
        {
            PlayerPrefs.SetInt("level_3", 1);
            StartCoroutine(WaitUntilDialogueFinishThenLoadScene("Level_0-1"));
        }
    }

    public void Level_4XWin()
    {
        if (GameManager.Instance.isWin)
        {
            PlayerPrefs.SetInt("level_4", 1);
            // TextManager.Instance.StartDialogueSystem("<#作者>剧情4-B还未完成<finish>");
            LoadScene("Level-0-1");
        }
    }
    #endregion

    /// <summary>
    /// 协程等待对话结束
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitUntilDialogueFinishThenLoadScene(string sceneName)
    {
        while(dialoguePanel.activeSelf)
        {
            yield return new WaitForSecondsRealtime(0.2f);
        }
        LoadScene(sceneName);
    }
}