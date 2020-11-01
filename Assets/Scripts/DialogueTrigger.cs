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
    
    public GameObject interactionIndicator = default;

    private void Start()
    {
        this._dialogueManager = FindObjectOfType<DialogueManager>();
        this.interactionIndicator.SetActive(false);
    }

    private void Update()
    {
        if (this._isInRange && Input.GetButtonDown("Fire2"))
        {
            if (this._hasStartedDialogue == false)
            {
                this._dialogueManager.StartDialogue(this.dialogue);
                this._hasStartedDialogue = true;
            }
            else
            {
                this._dialogueManager.DisplayNextSentence();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        this._isInRange = false;
        this.interactionIndicator.SetActive(false);
        this.animator.SetBool("IsOpen", false);
        this._hasStartedDialogue = false;
        this._isInRange = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        this._isInRange = true;
        this.interactionIndicator.SetActive(true);
    }
}
