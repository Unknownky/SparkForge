using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

/// <summary>
/// 文本预处理器接口，进行自定义的文本预处理，并且保存相关的数据
/// </summary>
public class AdvancedTextPreprocessor : ITextPreprocessor
{
    public Dictionary<int, float> IntervalDictionary;//存储间隔时间的字典，key为索引，value为间隔时间，用于打字机效果


    public AdvancedTextPreprocessor()//构造函数，初始化字典
    {
        IntervalDictionary = new Dictionary<int, float>();
    }

    /// <summary>
    /// 实现ITextPreprocessor接口的预处理方法，用于自定义文本预处理
    /// </summary>
    /// <param name="text">文本</param>
    /// <returns></returns>
    public string PreprocessText(string text)
    {
        IntervalDictionary.Clear();//清空字典，防止重复添加

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
            if (float.TryParse(label, out float result))
            {
                IntervalDictionary[matchIndex - 1] = result;
                Debug.Log(matchIndex - 1 + "  " + result);
            }

            //处理完当前标签后，移除当前标签，重新匹配
            processingText = processingText.Remove(matchIndex, matchLength);//移除匹配到的标签
            match = Regex.Match(processingText, pattern);//重新匹配
        }

        
        //第二次正则匹配，匹配出特定的标签，并且将其替换为interval
        processingText = text;
        pattern = @"<(\d+)(\.\d+)?>"; //匹配标签的正则表达式  .表示任意字符 *表示0个或多个 ?表示非贪婪模式(即尽可能少的匹配)
        processingText = Regex.Replace(processingText, pattern, "interval");//利用正则表达式移除匹配到的标签

        return processingText;//返回处理后的文本，作为当前脚本的文本

    }
}



/// <summary>
/// 继承自TextMeshProUGUI，用于实现文本的渲染效果
/// </summary>
public class AdvancedText : TextMeshProUGUI
{

    private int _typingIndex = 0; //打字机索引

    private float _defaultInterval = 0.2f; //默认间隔时间
    private AdvancedTextPreprocessor SelfPreprocessor => (AdvancedTextPreprocessor)textPreprocessor; //根据接口获取自定义的文本预处理器

    public AdvancedText()
    {   //构造函数，初始化文本预处理器
        textPreprocessor = new AdvancedTextPreprocessor();
    }

    /// <summary>
    /// 按照打字机效果显示文本
    /// </summary>
    /// <param name="content">要显示的内容</param>
    public void ShowTextByTyping(string content)
    {
        SetText(content); //内置的SetText方法，会设置文本内容, 并且会调用预处理器的预处理方法，将处理后的文本作为当前文本
        StartCoroutine(Typing());
    }

    //一个协程，用于实现打字机效果(协程按照顺序执行，只是添加了等待的过程，并且可以在等待的过程中执行其他的协程，互不影响)
    IEnumerator Typing()
    {

        ForceMeshUpdate(); //强制更新顶点信息
        for (int i = 0; i < m_characterCount; i++)//遍历所有字符,将透明度设置为0,即隐藏
        {
            SetSingleCharacterAlpha(i, 0);
        }

        _typingIndex = 0;
        while (_typingIndex < m_characterCount)//遍历所有字符,将透明度通过协程渐变设置为255,即显示
        {
            //如果当前字符可见,则设置透明度,用于修复空格的bug
            if(textInfo.characterInfo[_typingIndex].isVisible)
            {
                // SetSingleCharacterAlpha(_typingIndex, 255);
                StartCoroutine(FadeInCharacter(_typingIndex));//通过协程渐变设置透明度
            }
            if (SelfPreprocessor.IntervalDictionary.TryGetValue(_typingIndex, out float result)) //通过索引获取到对应的间隔时间
            { //如果当前索引有对应的间隔时间格式为<1>，则等待对应的时间
                yield return new WaitForSecondsRealtime(result);//yield return 跳不出循环，只是暂停当前协程，等待固定时间后继续执行
            }
            else
            {
                yield return new WaitForSecondsRealtime(_defaultInterval); //否则默认间隔0.1秒
            }
            _typingIndex++;
        }

    }

    //一个协程，用于实现渐变效果
    IEnumerator FadeInCharacter(int index, float duration = 0.5f)
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
