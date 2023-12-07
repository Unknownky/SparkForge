using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("DialogueSystem/NewDialogueContainer"), fileName = ("NewDialogueContainer"), order = 1)]
public class DialogueContainer : ScriptableObject
{
    [SerializeField] public List<Dialogue> dialogues = new List<Dialogue>();
    Dictionary<string, Dialogue> dialogueDictionary = new Dictionary<string, Dialogue>();

    private void OnEnable()
    {
        foreach (Dialogue dialogue in dialogues)
        {
            dialogueDictionary.Add(dialogue.dialogueName, dialogue);
        }
    }
    public Dialogue GetDialogue(string dialogueName)
    {
        if (dialogueDictionary.ContainsKey(dialogueName))
        {
            return dialogueDictionary[dialogueName];
        }
        else
        {
            Debug.LogError("Dialogue " + dialogueName + " not found!");
            return null;
        }
    }
}
