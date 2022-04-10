using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("Спиоск настроек для врагов")]
    [SerializeField] private List<EnemyData> enemySettings;

    [Tooltip("Список объектов в пуле")]
    [SerializeField] private int poolCount;

    [Tooltip("Ссылка на префаб")]
    [SerializeField] private GameObject enemyPrefab;

    [Tooltip("Время между спауном врагов")]
    [SerializeField] private float spawnTime;

    public static Dictionary<GameObject, Zombie> Enemies;

    private Queue<GameObject> currentEnemies;

    private void Start()
    {
        Enemies = new Dictionary<GameObject, Zombie>();
        currentEnemies = new Queue<GameObject>();

        for (int i = 0; i < poolCount; i++)
        {
            var prefab = Instantiate(enemyPrefab);
            prefab.transform.position = transform.position;
            var script = prefab.GetComponent<Zombie>();
            prefab.SetActive(false);
            Enemies.Add(prefab, script);
            currentEnemies.Enqueue(prefab);
        }

        Zombie.OnEnemyDeath += ReturnEnemy;

        StartCoroutine(Spawn());
    }

    // Возврат врага в пул
    private void ReturnEnemy(GameObject _enemy)
    {
        _enemy.transform.position = transform.position;
        var _script = _enemy.GetComponent<Zombie>();
        _script.hp = enemyPrefab.GetComponent<Zombie>().hp;
        _script.attackStage = 0;
        _enemy.SetActive(false);
        currentEnemies.Enqueue(_enemy);
    }

    private IEnumerator Spawn()
    {
        if (spawnTime == 0) spawnTime = 0.5f;
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            if (currentEnemies.Count > 0)
            {
                var enemy = currentEnemies.Dequeue();
                var script = Enemies[enemy];
                enemy.SetActive(true);

                script.Init(enemySettings[0]);
            }
        }
    }
}
