using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 该脚本用于管理文本的显示,提供处理文本的方法
/// </summary>
public class TextManager : MonoBehaviour
{

    [SerializeField] private GameObject _dialoguePanel;//对话框物体
    [SerializeField] private AdvancedText _text;//要显示文本的对话框

    [SerializeField] private TMP_Text _nameText;//显示人物名字的文本框

    [SerializeField] private GameObject _characterImage;//显示人物图片的物体

    public string characterName = "我";//人物名字

    public byte characterSameRGB = 100;//人物图片的RGB值

    public static TextManager Instance;
    /// <summary>
    /// 处理文本后的结果，永远只显示第一个元素
    /// </summary>
    public List<string> result { get; private set; }

#if UNITY_EDITOR
    [SerializeField] public GameObject _background;//背景图片
    [SerializeField] public Sprite _Sunbackground;//背景图片
    [SerializeField] public Sprite _Rainbackground;//背景图片

#endif

    private Animator _dialoguePanelAnimator;//对话框的动画控制器

    private float _fadeTime = 0.8f;//对话框淡入淡出的时间

    //获取人物的图片的材质来改变其alpha值
    private Material _characterImageMaterial;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // DontDestroyOnLoad(gameObject);//切换场景时不销毁
        result = new List<string>();//初始化
        _dialoguePanelAnimator = _dialoguePanel?.GetComponent<Animator>();
        _characterImageMaterial = _characterImage?.GetComponent<Image>().material;

    }


    #region 暴露给外部的方法
    /// <summary>
    /// 开始对话系统
    /// </summary>
    public void StartDialogueSystem(string text)
    {
        AccordTextProduceResult(text);//处理文本
        ShowDialoguePanel();//显示对话框
        ShowNameAndFirstSentence();//显示第一句话，显示第一句话后即开启了TMP的文本处理系统，之后的文本将由TMP处理
    }

    /// <summary>
    /// 结束对话系统
    /// </summary>
    public void EndDialogueSystem()
    {
        StartCoroutine(HideDialoguePanel());//隐藏对话框
        result.Clear();//清空结果
    }

    public void SetTextEmpty()
    {
        _text.SetText("");
    }

    /// <summary>
    /// 显示第一句话,并将其从result中移除
    /// </summary>
    public void ShowNameAndFirstSentence()
    {
        ShowDialogueNameAndProcessPanel();//显示对话人物名字
        _text.ShowTextByTyping(result[0]);
        result.RemoveAt(0);
    }

    #endregion

    private void ShowDialogueNameAndProcessPanel()
    {
        string name = result[0].Substring(2, result[0].IndexOf(">") - 2);
        //显示对话人物名字
        _nameText.text = name;//显示人物名字
        //在本游戏中只需要处理主角的图片的alpha值即可
        if (name != characterName)
        {
            _characterImageMaterial.color = new Color32(characterSameRGB, characterSameRGB, characterSameRGB, 255);//隐藏人物图片
            #if UNITY_EDITOR
            Debug.Log(characterSameRGB);
            #endif
        }
        else{
            _characterImageMaterial.color = new Color32(255, 255, 255, 255);//显示人物图片
            #if UNITY_EDITOR
            Debug.Log("255");
            #endif
        }
    }


    private void ShowDialoguePanel()
    {
        //显示对话框
        _dialoguePanel.SetActive(true);
    }

    private IEnumerator HideDialoguePanel()
    {
        _dialoguePanelAnimator.Play("fadeout");
        yield return new WaitForSecondsRealtime(_fadeTime);
        //隐藏对话框
        _dialoguePanel.SetActive(false);
    }

    private void AccordTextProduceResult(string text)
    {
        result = TextSpilter.SplitText(text);//将文本分割后的结果

#if UNITY_EDITOR
        TextSpilter.PrintSplitText(result);
#endif
    }


}
