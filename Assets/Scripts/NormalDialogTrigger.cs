using UnityEngine;

public class NormalDialogTrigger : MonoBehaviour, IIinteract
{
    [SerializeField] private Dialog dialog;
    public void Interaksi()
    {
        DialogueManager.Instance.StartDialogue(dialog);
        Debug.Log("Dialog: "+dialog.DialogueLine.Count);
    }
}