using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public Animator animator;

    public Queue<string> sentences;

    private void Start()
    {
        sentences = new Queue<string>();   
    }

    public void StartDialogue (Dialogue dialogue, string NPCTag)
    {
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(NPCTag);
    }

    public void DisplayNextSentence(string NPCTag)
    {
        if(sentences.Count == 0)
        {
            EndDialogue(NPCTag);
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public event EventHandler DialogueEnded;

    void EndDialogue(string NPCTag)
    {
        animator.SetBool("IsOpen", false);
        if(NPCTag == "Boss")
        {
            this.OnDialogueEnded();
        }
    }

    protected virtual void OnDialogueEnded()
    {
        this.DialogueEnded?.Invoke(this, EventArgs.Empty);
    }
}
