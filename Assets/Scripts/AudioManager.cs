using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static FMOD.Studio.EventInstance _staticMusicInstance = default;
    public static DanceSystem _staticDanceSystem = default;
    [EventRef] [SerializeField] public string _song = default;

    private void Start()
    {
        _staticDanceSystem = GetComponent<DanceSystem>();

        PlaySong(_song);
        
        if(_staticDanceSystem != null)
        {
            _staticDanceSystem.AssignBeatEvent(_staticMusicInstance);
        }
        _staticMusicInstance.release();
    }
    
    private void OnDestroy()
    {
        _staticMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public static void StopSong(string _newSong)
    {
        _staticMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public static void PlaySong(string _newSong)
    {
        if(_staticMusicInstance.ToString() != null)
        {
            _staticMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        _staticMusicInstance = FMODUnity.RuntimeManager.CreateInstance(_newSong);
        _staticMusicInstance.start();
    }
}
