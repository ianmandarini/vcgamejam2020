using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SummonBossAfterDialogueEvent : MonoBehaviour
{
    private DialogueManager _dialogueManager;
    [SerializeField] UnityEvent _onDialogueEnd = default;

    private void OnEnable()
    {
        this._dialogueManager.DialogueEnded += this.DialogueEndedEventHandler;
    }

    private void OnDisable()
    {
        this._dialogueManager.DialogueEnded -= this.DialogueEndedEventHandler;
    }

    private void Awake()
    {
        this._dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void DialogueEndedEventHandler(object sender, EventArgs e)
    {
        this._onDialogueEnd?.Invoke();
    }
}
