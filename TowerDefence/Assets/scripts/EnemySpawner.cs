using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    public Transform startPoint;
    public List<Wave> waveList;

    private int enemyCount = 0;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(SpawnEnemyWithDelay());
    }

    IEnumerator SpawnEnemyWithDelay()
    {
        yield return new WaitForSeconds(8f);   // ��һ������ǰ 10 �뻺��
        yield return SpawnEnemy();              // ԭ�����߼�����
    }

    IEnumerator SpawnEnemy()
    {
        foreach (Wave wave in waveList)
        {
            for (int i = 0; i < wave.count; i++)
            {
                GameObject.Instantiate(wave.enemyPrefab, startPoint.position, Quaternion.identity);
                enemyCount++;
                if (i < wave.count - 1)
                {  
                    yield return new WaitForSeconds(wave.rate);
                }

            }
            float timer = 0f;
            while (enemyCount > 0 && timer < 10f)
            {
                timer += Time.deltaTime;
                yield return null;                      // ÿ֡���
            }
            yield return null;
        }
    }

    public void DecreaseEnemyCount()
    {
        if (enemyCount > 0)
        {
            enemyCount--;
        }
    }
}
