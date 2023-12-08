using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

/// <summary>
/// 文本预处理器接口，进行自定义的文本预处理，并且保存所有标签的数据
/// </summary>
public class AdvancedTextPreprocessor : ITextPreprocessor
{
    /// <summary>
    /// 用于存储所有标签的数据
    /// </summary>
    public Dictionary<int, List<string>> LabelDictionary;//存储间隔时间的字典，key为索引，value为多标签


    public AdvancedTextPreprocessor()//构造函数，初始化字典
    {
        LabelDictionary = new Dictionary<int, List<string>>();
    }

    /// <summary>
    /// 实现ITextPreprocessor接口的预处理方法，用于自定义文本预处理
    /// </summary>
    /// <param name="text">文本</param>
    /// <returns></returns>
    public string PreprocessText(string text)
    {
        LabelDictionary.Clear();//清空字典，防止重复添加

        //第一次正则匹配，匹配出所有的间隔时间标签，并且将其存入字典
        string processingText = text;//要处理的文本
        string pattern = "<.*?>";//匹配标签的正则表达式  .表示任意字符 *表示0个或多个 ?表示非贪婪模式(即尽可能少的匹配)
        Match match = Regex.Match(processingText, pattern);//正则匹配规则


        while (match.Success)//如果匹配成功
        {
            string matchValue = match.Value;//匹配到的值
            int matchIndex = match.Index;//匹配到的索引
            int matchLength = match.Length;//匹配到的长度
            string label = matchValue.Substring(1, matchLength - 2);//标签名
            if (label != "")
            {
                if (LabelDictionary.ContainsKey(matchIndex - 1) == false)//如果字典中没有当前索引的key，则创建一个list<string>
                {
                    LabelDictionary.Add(matchIndex - 1, new List<string>());
                }
                LabelDictionary[matchIndex - 1].Add(label);//将当前索引的标签添加到字典中
#if UNITY_EDITOR
                Debug.Log(matchIndex - 1);
                foreach (var item in LabelDictionary[matchIndex - 1])
                {
                    Debug.Log(item);
                }
#endif
            }

            //处理完当前标签后，移除当前标签，重新匹配
            processingText = processingText.Remove(matchIndex, matchLength);//移除匹配到的标签
            match = Regex.Match(processingText, pattern);//重新匹配
        }
        //完成了字典的赋值

#if UNITY_EDITOR
        Debug.Log("The Label count is:" + LabelDictionary.Count);
        foreach (var item in LabelDictionary)
        {
            Debug.Log(item.Key + "  " + item.Value);
        }
#endif


        //第二次正则匹配，匹配出特定的标签，并且将其替换为interval
        processingText = text;
        // pattern = @"<(\d+)(\.\d+)?>"; //匹配标签的正则表达式  .表示任意字符 *表示0个或多个 ?表示非贪婪模式(即尽可能少的匹配)
        pattern = @"<.*?>";
#if UNITY_EDITOR
        processingText = Regex.Replace(processingText, pattern, "");//利用正则表达式移除匹配到的标签
#else
        {
            processingText = Regex.Replace(processingText, pattern, "");//利用正则表达式移除匹配到的标签
        }
#endif
        return processingText;//返回处理后的文本，作为当前脚本的文本
    }
}



/// <summary>
/// 继承自TextMeshProUGUI，用于实现文本的渲染效果
/// </summary>
public class AdvancedText : TextMeshProUGUI
{

    private int _typingIndex = 0; //打字机索引

    private float _defaultInterval = 0.02f; //默认间隔时间

    private float _interval; //间隔时间

    private AdvancedTextPreprocessor SelfPreprocessor => (AdvancedTextPreprocessor)textPreprocessor; //根据接口获取自定义的文本预处理器

    public AdvancedText()
    {   //构造函数，初始化文本预处理器
        textPreprocessor = new AdvancedTextPreprocessor();
    }

    private void Update()
    {
        if(TextManager.Instance.result.Count <= 2){ //只剩最后一段
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(TextManager.Instance.result.Count == 1){ //只剩最后一段
                return;
            }
            _typingIndex = m_characterCount - 1;//将索引设置为最后一个字符的索引
            TextManager.Instance.SetTextEmpty();//清空文本
            TextManager.Instance.ShowNameAndFirstSentence();//显示下一句话
        }
    }

    /// <summary>
    /// 按照打字机效果显示文本
    /// </summary>
    /// <param name="content">要显示的内容</param>
    public void ShowTextByTyping(string content)
    {
        SetText(content); //内置的SetText方法，会设置文本内容, 并且会调用预处理器的预处理方法，将处理后的文本作为当前文本
        StartCoroutine(TypingSequence());
    }

    //一个协程，用于实现打字机效果(协程按照顺序执行，只是添加了等待的过程，并且可以在等待的过程中执行其他的协程，互不影响)
    IEnumerator TypingSequence()
    {

        ForceMeshUpdate(); //强制更新顶点信息
        for (int i = 0; i < m_characterCount; i++)//遍历所有字符,将透明度设置为0,即隐藏
        {
            SetSingleCharacterAlpha(i, 0);
        }

        _typingIndex = 0;

        //以下代码按照字符的顺序类似于字符的TimeLine
        while (_typingIndex < m_characterCount)//遍历所有字符,将透明度通过协程渐变设置为255,即显示
        {
            //如果当前字符可见,则设置透明度,用于修复空格的bug
            if (textInfo.characterInfo[_typingIndex].isVisible)
            {
                // SetSingleCharacterAlpha(_typingIndex, 255);
                StartCoroutine(FadeInCharacter(_typingIndex));//通过协程渐变设置透明度
            }

            //在这里添加标签事件逻辑
            SequenceEventTriger(_typingIndex);//标签事件逻辑

            yield return new WaitForSecondsRealtime(_interval);

            _typingIndex++;
        }

    }

    public void SequenceEventTriger(int index)
    {

        //如果当前索引有对应的标签事件，则触发对应的标签事件
        if (SelfPreprocessor.LabelDictionary.ContainsKey(index))
        {
            foreach (var label in SelfPreprocessor.LabelDictionary[index])
            {
                //获取需要间隔的时间
                string pattern = @"<(\d+)(\.\d+)?>";
                Match match = Regex.Match(label, pattern);//正则匹配规则
                if (match.Success)
                {
                    _interval = float.Parse(match.Value.Substring(1, match.Length - 2));
#if UNITY_EDITOR
                    Debug.Log("经过标签处理的间隔时间为：" + _interval);
#endif
                    continue;
                }
                _interval = _defaultInterval;

                //触发标签事件
                CallbackLabelEventHandler(label);
            }

        }
    }

    /// <summary>
    /// 标签事件回调函数，之后添加标签事件增加对应的case即可
    /// </summary>
    /// <param name="label"></param>
    private void CallbackLabelEventHandler(string label)
    {
        switch (label)
        {
            case LabelContainer.break_L:
#if UNITY_EDITOR
                Debug.Log("触发了标签事件break");
#endif
                StartCoroutine(BreakInvoke());
                break;
            case LabelContainer.rain_L:
#if UNITY_EDITOR
                Debug.Log("触发了标签事件rain");
#endif
                // RainInvoke();
                break;
            case LabelContainer.finish_L:
#if UNITY_EDITOR
                Debug.Log("触发了标签事件finish");
#endif
                StartCoroutine(FinishInvoke());
                break;
            default:
                break;
        }
    }

    #region 标签事件函数
    IEnumerator BreakInvoke()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        TextManager.Instance.SetTextEmpty();//清空文本
        TextManager.Instance.ShowNameAndFirstSentence();//显示下一句话
    }

    #if UNITY_EDITOR
    private void RainInvoke()
    {
        TextManager.Instance._background.GetComponent<SpriteRenderer>().sprite = TextManager.Instance._Rainbackground;
    }
    #endif
    IEnumerator FinishInvoke()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        TextManager.Instance.SetTextEmpty();//清空文本
        TextManager.Instance.EndDialogueSystem();//结束对话系统
    }

    #endregion


    //一个协程，用于实现渐变效果
    IEnumerator FadeInCharacter(int index, float duration = 0.1f)
    {
        if (duration <= 0)
        {
            SetSingleCharacterAlpha(index, 255);
        }
        else
        {
            float timer = 0;
            while (timer < duration)
            {
                //Time.unscaledDeltaTime是真实时间，不受Time.timeScale影响，对话框出现时，游戏暂停，但是对话框的渐变效果不受影响
                timer = Mathf.Min(duration, timer + Time.unscaledDeltaTime);//Mathf.Min返回最小值，防止超过duration，造成透明度超过255
                SetSingleCharacterAlpha(index, (byte)(255 * (timer / duration)));
                yield return null;
            }
        }

    }

    //newAlpha范围是0-255
    private void SetSingleCharacterAlpha(int index, byte newAlpha)
    {
        TMP_CharacterInfo charInfo = textInfo.characterInfo[index]; //获取单个字符信息
        int matIndex = charInfo.materialReferenceIndex; //获取材质索引
        int vertIndex = charInfo.vertexIndex; //获取顶点索引
        for (int i = 0; i < 4; i++)
        {
            textInfo.meshInfo[matIndex].colors32[vertIndex + i].a = newAlpha; //设置文字每个顶点的透明度
        }
        UpdateVertexData(); //更新顶点信息,让修改生效
    }


}

public static class LabelContainer
{
    public const string break_L = "break";
    public const string rain_L = "rain";
    public const string finish_L = "finish";
}
