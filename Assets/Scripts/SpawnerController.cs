using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerController : MonoBehaviour
{
    private GameObject[] spawners;
    private double timeWent = 0f;
    private double afterTime = 5f;
    private int remaining = 40;

    public GameObject target;
    public GameObject enemy;
    public int enemiesToGenerate = 3;
    public int maxEnemies = 40;
    public TextMeshProUGUI enemyText;
    
    // Start is called before the first frame update
    void Start()
    {
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        remaining = maxEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        timeWent += Time.deltaTime;
        if (timeWent >= afterTime)
        {
            timeWent -= afterTime;
            afterTime = Random.Range(10f, 20f);
            generateEnemies();
        }

        if (enemyText != null)
        {
            enemyText.SetText("{0}/{1}", GameObject.FindGameObjectsWithTag("Enemy").Length/2, remaining);
        }
    }

    private void generateEnemies()
    {
        var toGenerate = remaining - enemiesToGenerate >= 0 ? enemiesToGenerate : remaining;
        if (enemy == null) return;
        for (int i = 0; i < toGenerate; i++)
        {
            var spawnerNumber = Random.Range(0, spawners.Length);
            var spawner = spawners[spawnerNumber];
            var localEnemy = Instantiate(enemy, spawner.transform.position, Quaternion.LookRotation(spawner.transform.forward));
            localEnemy.GetComponent<EnemController>().target = target;
            remaining -= 1;
        }
    }
}
