using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [Header("Dialog Line")]
    public string name = "Character Name";
    [TextArea(3, 10)]
    [SerializeField] List<string> dialogueLine;
    public List<string> DialogueLine 
    { 
        get { return dialogueLine; }
        set {} 
    }
}