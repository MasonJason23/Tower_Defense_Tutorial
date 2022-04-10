using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public static List<GameObject> enemiesInScene = new List<GameObject>();
    public Transform enemyPrefab;
    public Transform spawnPoint;
    public TextMeshProUGUI waveCountDownText;
    
    public float waveInterval = 5f;
    private float countdown = 2f;

    private int waveIndex = 0;
    
    private void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = waveInterval;
        }

        countdown -= Time.deltaTime;
        waveCountDownText.text = Mathf.Floor(countdown + 1).ToString();
    }

    IEnumerator SpawnWave()
    {
        waveIndex++;
        PlayerStats.Waves++;
        for (int i = 0; i < waveIndex * 2; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation).gameObject;
        enemiesInScene.Add(enemy);
    }
}
