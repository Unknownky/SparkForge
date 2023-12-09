using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("DialogueSystem/NewDialogueContainer"), fileName = ("NewDialogueContainer"), order = 1)]
public class DialogueContainer : ScriptableObject
{
    [SerializeField] public List<Dialogue> dialogues = new List<Dialogue>();

}
