using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject zombie;
    public GameObject[] enemySpawn;
    private List<GameObject> enemy = new List<GameObject>();
    private int maxEnemyNumber = 10;
    private int maxMomentEnemy = 5;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnZombie());
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < enemy.Count; i++)
        {
            if (enemy[i].GetComponent<Zombie>().hp == 0) enemy.RemoveAt(i);
        }
    }

    IEnumerator spawnZombie()
    {
        while (maxEnemyNumber > 0)
        {
            if (enemy.Count < maxMomentEnemy)
            {
                int spawnIndex = Random.Range(0, enemySpawn.Length);
                enemy.Add(Instantiate(zombie, enemySpawn[spawnIndex].transform.position, Quaternion.identity));
                maxEnemyNumber--;
            }
            yield return new WaitForSeconds(1);
        }
    }
}
