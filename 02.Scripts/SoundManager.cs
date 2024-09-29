using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip backGound_Sound;               // 배경 음악
    public AudioClip[] fear_Effect_Sounds;          // 여러 공포 효과음 배열

    private AudioSource SoundManagerPlayer;

    public float minTime = 10f;  // 최소 대기 시간
    public float maxTime = 40f; // 최대 대기 시간

    private void Awake()
    {
        SoundManagerPlayer = GetComponent<AudioSource>();

        // 배경음악이 있다면 시작 시 재생
        if (backGound_Sound != null)
        {
            SoundManagerPlayer.clip = backGound_Sound;
            SoundManagerPlayer.loop = true;  // 배경음악은 반복 재생
            SoundManagerPlayer.Play();
        }

        // 랜덤 공포 효과음 재생을 시작
        StartCoroutine(PlayRandomFearEffect());
    }

    private IEnumerator PlayRandomFearEffect()
    {
        while (true)
        {
            // 랜덤 시간 대기
            float randomWaitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(randomWaitTime);

            // 랜덤 효과음 재생
            Rendom_Fear_Effect();
        }
    }

    public void Rendom_Fear_Effect()
    {
        if (fear_Effect_Sounds.Length > 0)
        {
            // 랜덤으로 배열에서 공포 효과음을 선택
            int randomIndex = Random.Range(0, fear_Effect_Sounds.Length);
            AudioClip randomClip = fear_Effect_Sounds[randomIndex];

            // 선택된 공포 효과음 재생
            SoundManagerPlayer.PlayOneShot(randomClip);
        }
        else
        {
            Debug.LogWarning("No fear effect sounds assigned.");
        }
    }
}