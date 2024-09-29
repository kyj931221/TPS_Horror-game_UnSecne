using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Flash_Data", fileName = "Flash_Data")]

public class Weapon_Flash_Data : ScriptableObject
{
    public AudioClip shotClip; // ������ Ű�� �Ҹ�
    public AudioClip reloadClip; // ������ ���͸� ���� �Ҹ�

    public float damage = 5; // ���ݷ�

    public int startAmmoRemain = 100; // ó���� �־��� ��ü ���͸� ����
    public int magCapacity = 25; // ���͸� �뷮

    public float timeBetFire = 0.12f; // ������ ON/OFF ����
    public float reloadTime = 1.8f; // ���͸� ���� �ҿ� �ð�
}
