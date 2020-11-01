using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Animator animator;
    public Dialogue dialogue;
    private DialogueManager _dialogueManager;
    private bool _hasStartedDialogue = false;
    private bool _isInRange = false;

    private void Start()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void Update()
    {
        if (_isInRange && Input.GetButtonDown("Fire2"))
        {
            if (_hasStartedDialogue == false)
            {
                _dialogueManager.StartDialogue(dialogue);
                _hasStartedDialogue = true;
            }
            else
            {
                _dialogueManager.DisplayNextSentence();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            animator.SetBool("IsOpen", false);
            _hasStartedDialogue = false;
            _isInRange = false;
        }
    }
}
