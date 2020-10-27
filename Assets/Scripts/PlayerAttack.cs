using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator _animator;
    private int _weaponDamage = 1;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimation(AnimationClip clip)
    {
        _animator.Play(clip.name);
    }

    public void SetWeapon(int newDamageValue)
    {
        _weaponDamage = newDamageValue;
    }
}
