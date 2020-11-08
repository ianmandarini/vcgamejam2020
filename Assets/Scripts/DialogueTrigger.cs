using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    public Animator animator;
    public Dialogue dialogue;
    private DialogueManager _dialogueManager;
    private bool _hasStartedDialogue = false;
    private bool _isInRange = false;
    [SerializeField] UnityEvent onDialogueEnd = default;
    
    public GameObject interactionIndicator = default;

    private void Awake()
    {
        this._dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void Start()
    {
        this.interactionIndicator.SetActive(false);
        this._dialogueManager.DialogueEnded += this.DialogueEndedEventHandler;
    }

    private void OnEnable()
    {
        this._dialogueManager.DialogueEnded += this.DialogueEndedEventHandler;
    }

    private void OnDisable()
    {
        this._dialogueManager.DialogueEnded -= this.DialogueEndedEventHandler;
    }

    private void DialogueEndedEventHandler(object sender, EventArgs e)
    {
        this.onDialogueEnd?.Invoke();
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
