using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private DialogueManager _dialogueManager;
    private bool _hasStartedDialogue = false;

    private void Start()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>();
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if(other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.Z) && _hasStartedDialogue == false)
            {
                _dialogueManager.StartDialogue(dialogue);
                _hasStartedDialogue = true;
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Z))
                { 
                    _dialogueManager.DisplayNextSentence();
                }
            }
        }
    }
}
