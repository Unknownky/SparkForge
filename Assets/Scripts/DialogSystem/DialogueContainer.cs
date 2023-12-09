using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("DialogueSystem/NewDialogueContainer"), fileName = ("NewDialogueContainer"), order = 1)]
public class DialogueContainer : ScriptableObject
{
    [SerializeField] public List<Dialogue> dialogues;

    private Dictionary<string, Dialogue> dialogueDictionary;

    private void Awake() {
        InitDictionary();//初始化字典
    }

    private void InitDictionary()
    {
        dialogueDictionary = new Dictionary<string, Dialogue>();
        foreach (var dialogue in dialogues)
        {
            dialogueDictionary.Add(dialogue.dialogueName, dialogue);
#if UNITY_EDITOR
            Debug.Log("添加了对话" + dialogue.dialogueName);
#endif
        }
    }
}
