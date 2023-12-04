using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 该脚本用于管理文本的显示,提供处理文本的方法
/// </summary>
public class TextManager : MonoBehaviour
{

    [SerializeField]private GameObject _dialoguePanel;//对话框物体
    [SerializeField]private AdvancedText _text;//要显示文本的对话框

    public static TextManager Instance;
    /// <summary>
    /// 处理文本后的结果，永远只显示第一个元素
    /// </summary>
    public List<string> result { get; private set;}


#if UNITY_EDITOR
    [Multiline]
    [SerializeField] private string text;//测试用文本

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AccordTextProduceResult(text);
        }
    }
#endif

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);//切换场景时不销毁
        result = new List<string>();//初始化
    }

#region 暴露给外部的方法
    /// <summary>
    /// 开始对话系统
    /// </summary>
    public void StartDialogueSystem(string text)
    {
        AccordTextProduceResult(text);//处理文本
        ShowDialoguePanel();//显示对话框
        ShowFirstSentence();//显示第一句话，显示第一句话后即开启了TMP的文本处理系统，之后的文本将由TMP处理
    }

    /// <summary>
    /// 结束对话系统
    /// </summary>
    public void EndDialogueSystem()
    {
        HideDialoguePanel();//隐藏对话框
        result.Clear();//清空结果
    }


    /// <summary>
    /// 显示第一句话,并将其从result中移除
    /// </summary>
    public void ShowFirstSentence()
    {
        _text.ShowTextByTyping(result[0]);
        result.RemoveAt(0);
    }

#endregion

    private void ShowDialoguePanel()
    {
        //显示对话框
        _dialoguePanel.SetActive(true);
    }

    private void HideDialoguePanel()
    {
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
