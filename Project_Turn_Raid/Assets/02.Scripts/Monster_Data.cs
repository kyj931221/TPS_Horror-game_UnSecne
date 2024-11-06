using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Monster_Data", fileName = "Monster_Data")]
public class Monster_Data : ScriptableObject
{
    public float currentHP = 100f;
    public float damage = 0f;
    public float speed = 0.5f;
    public Color skinColor = Color.white;
}
