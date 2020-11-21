using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Animator animator;
    public Dialogue dialogue;
    private DialogueManager _dialogueManager;
    public bool _hasStartedDialogue = false;
    private bool _isInRange = false;
    
    public GameObject interactionIndicator = default;

    private void Awake()
    {
        this._dialogueManager = FindObjectOfType<DialogueManager>();
    }
    
    private void Start()
    {
        this.interactionIndicator.SetActive(false);
    }
   
    private void Update()
    {
        if (this._isInRange && Input.GetButtonDown("Fire2"))
        {
            if (this._hasStartedDialogue == false)
            {
                this._dialogueManager.StartDialogue(this.dialogue, this.tag);
                this._hasStartedDialogue = true;
            }
            else
            {
                this._dialogueManager.DisplayNextSentence(this.tag);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        NotInRange();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        IsInRange();
    }

    public void IsInRange()
    {
        this._isInRange = true;
        this.interactionIndicator.SetActive(true);
    }

    public void NotInRange()
    {
        this._isInRange = false;
        this.interactionIndicator.SetActive(false);
        this.animator.SetBool("IsOpen", false);
        this._hasStartedDialogue = false;
        this._isInRange = false;
    }
}
