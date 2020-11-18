using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SongInitializer : MonoBehaviour
{
    [EventRef] [SerializeField] private string _newSong = default;

    private void OnEnable()
    {
        AudioManager.PlaySong(_newSong);
    }
}
