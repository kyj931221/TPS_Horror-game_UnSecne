using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Flash_Data", fileName = "Flash_Data")]

public class Weapon_Flash_Data : ScriptableObject
{
    public AudioClip shotClip; // 손전등 키는 소리
    public AudioClip reloadClip; // 손전등 배터리 가는 소리

    public float damage = 5; // 공격력

    public int startAmmoRemain = 100; // 처음에 주어질 전체 배터리 개수
    public int magCapacity = 25; // 배터리 용량

    public float timeBetFire = 0.12f; // 손전등 ON/OFF 간격
    public float reloadTime = 1.8f; // 배터리 장전 소요 시간
}
