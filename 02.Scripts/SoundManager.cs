using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip backGound_Sound;               // ��� ����
    public AudioClip[] fear_Effect_Sounds;          // ���� ���� ȿ���� �迭

    private AudioSource SoundManagerPlayer;

    public float minTime = 10f;  // �ּ� ��� �ð�
    public float maxTime = 40f; // �ִ� ��� �ð�

    private void Awake()
    {
        SoundManagerPlayer = GetComponent<AudioSource>();

        // ��������� �ִٸ� ���� �� ���
        if (backGound_Sound != null)
        {
            SoundManagerPlayer.clip = backGound_Sound;
            SoundManagerPlayer.loop = true;  // ��������� �ݺ� ���
            SoundManagerPlayer.Play();
        }

        // ���� ���� ȿ���� ����� ����
        StartCoroutine(PlayRandomFearEffect());
    }

    private IEnumerator PlayRandomFearEffect()
    {
        while (true)
        {
            // ���� �ð� ���
            float randomWaitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(randomWaitTime);

            // ���� ȿ���� ���
            Rendom_Fear_Effect();
        }
    }

    public void Rendom_Fear_Effect()
    {
        if (fear_Effect_Sounds.Length > 0)
        {
            // �������� �迭���� ���� ȿ������ ����
            int randomIndex = Random.Range(0, fear_Effect_Sounds.Length);
            AudioClip randomClip = fear_Effect_Sounds[randomIndex];

            // ���õ� ���� ȿ���� ���
            SoundManagerPlayer.PlayOneShot(randomClip);
        }
        else
        {
            Debug.LogWarning("No fear effect sounds assigned.");
        }
    }
}