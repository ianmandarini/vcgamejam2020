using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _player = default;
    [SerializeField] private GameObject _dracula = default;
    [SerializeField] private GameObject _gameOverCanvas = default;
    [SerializeField] private GameObject[] _gameOverChildren = default;
    [SerializeField] private GameObject _draculaFangs = default;
    private bool _hasEnded = false;
    private bool _fadeComplete = false;
    private DialogueTrigger _dialogueTrigger = default;
    private DialogueManager _dialogueManager = default;

    [EventRef] [SerializeField] private string _draculaBiteEffect = default;

    void Start()
    {
        _dialogueTrigger = this.GetComponent<DialogueTrigger>();
        _dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
    }

    void FixedUpdate()
    {
        if (_dracula == null && _hasEnded == false)
        {
            _hasEnded = true;

            StartCoroutine(WaitToFadeIn());
        }

        else if (_hasEnded == true &&
                _dialogueTrigger._hasStartedDialogue == false &&
                _fadeComplete == true)
        {
            _player.SetActive(false);
            StartEndingDialogue();
        }

        if (_hasEnded == true &&
            _dialogueTrigger._hasStartedDialogue == true &&
            _fadeComplete == true &&
            _dialogueTrigger.animator.GetBool("IsOpen") == false)
        {
            _draculaFangs.SetActive(true);
        }
    }

    IEnumerator WaitToFadeIn()
    {
        yield return new WaitForSeconds(1.5f);

        _gameOverCanvas.SetActive(true);

        for (int i = 0; i < _gameOverChildren.Length; i++)
        {
            if (_gameOverChildren[i].name != "GameOverBG")
            {
                _gameOverChildren[i].SetActive(false);
            }
        }
        yield return new WaitForSeconds(3.0f);

        _fadeComplete = true;
    }

    IEnumerator GoToTitleScreen()
    {
        yield return new WaitForSeconds(13.0f);
        SceneManager.LoadScene("TitleScreen");
    }

    private void StartEndingDialogue()
    {
        _dialogueTrigger.IsInRange();
        _dialogueManager.StartDialogue(this._dialogueTrigger.dialogue, this.tag);
        _dialogueTrigger._hasStartedDialogue = true;

        if(_draculaBiteEffect != null)
        {
            RuntimeManager.PlayOneShot(_draculaBiteEffect, this.transform.position);
            StartCoroutine(GoToTitleScreen());
        }
    }
}
