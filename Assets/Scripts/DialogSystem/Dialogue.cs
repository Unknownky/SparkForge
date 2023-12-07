using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("DialogueSystem/NewDialogue"), fileName = ("NewDialogue"), order = 1)]
public class Dialogue : ScriptableObject
{
    [SerializeField] public string dialogueName;
    [Multiline(10)]
    [SerializeField] public string dialogue;
}
