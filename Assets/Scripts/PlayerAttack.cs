using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    private int _weaponDamage;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimation(AnimationClip clip)
    {
        _animator.Play(clip.name);
    }
}
