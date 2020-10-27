using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    public int weaponID;
    public string weaponName;
    public int damage;
    public Sprite image;
    public AnimationClip animation;
}

//Script escalonável para criação de objetos weapon para caso aconteça aumento no escopo