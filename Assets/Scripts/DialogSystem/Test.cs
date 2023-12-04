using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private AdvancedText _text;

    [Multiline]
    [SerializeField] private string _content;

    private void Start() {
        _text.ShowTextByTyping(_content);
    }

}
