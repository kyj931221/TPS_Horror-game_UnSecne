using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Monsters monsterPrefab;

    public Monster_Data[] monsterDatas;
    public Transform[] spawnPoints;

    private List<Monsters> monsters = new List<Monsters>();
    private int wave; // 현재 웨이브

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance != null && GameManager.Instance.isGameover)
        {
            return;
        }

        if(monsters.Count <= 0)
        {
            SpawnWave();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        UIManager.Instance.UpdateWaveText(wave,monsters.Count);
    }

    private void SpawnWave()
    {
        wave++;

        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        for (int i = 0; i < spawnCount; i++)
        {
            CreateMonster();
        }
    }

    private void CreateMonster()
    {
        Monster_Data monsterData = monsterDatas[Random.Range(0,monsterDatas.Length)];

        Transform spawnPoint = spawnPoints[Random.Range(0,spawnPoints.Length)];

        Monsters monster = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);

        monster.Setup(monsterData);

        monsters.Add(monster);

        monster.onDeath += () => monsters.Remove(monster);

        monster.onDeath += () => Destroy(monster.gameObject, 10f);
    }
}
