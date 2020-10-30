using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int _weaponDamage = 1;
    private Animator _animator;

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

    public int GetDamage()
    {
        return _weaponDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.TakeDamage(_weaponDamage);
        }
    }
}
