using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyInstantiater : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;

    private Camera cam;

    private Transform player;

    [SerializeField]
    private float enemiesPerSecond;

    private float timer = 0;

    private float spawnHorizontalMargin;
    private float spawnVerticalMargin;

    [SerializeField]
    private float initialDelay;

    [SerializeField]
    private float heightMax;

    [SerializeField]
    private float heightMin;

    private float camWidth;

    private Func<float, float> updateRate = SimpleUpdateRate;

    [SerializeField]
    private EnemyManager manager;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        cam = Camera.main;
        float camHeight = 2f * cam.orthographicSize;
        camWidth = cam.aspect * camHeight;

        SpriteRenderer renderer = enemy.GetComponentInChildren<SpriteRenderer>();

        spawnHorizontalMargin = renderer.bounds.size.x / 2;
        spawnVerticalMargin = renderer.bounds.size.y / 2;
    }

    public void SetUpdateRate(Func<float, float> _updateRate)
    {
        updateRate = _updateRate;
    }

    private static Vector2 RandomVectorInBox(Vector2 bottomLeftCorner, Vector2 bounds)
    {
        return new Vector2(bottomLeftCorner.x + bounds.x * UnityEngine.Random.value, bottomLeftCorner.y + bounds.y * UnityEngine.Random.value);
    }

    private Vector2 GetRandomEnemySpawnVector()
    {
        Vector2 corner = Vector2.zero;
        corner.x = cam.transform.position.x - (camWidth / 2 + spawnHorizontalMargin);
        corner.y = heightMin + spawnVerticalMargin;

        Vector2 randomPos = RandomVectorInBox( corner
            , new Vector2(0, (heightMax - heightMin) - 2 * spawnVerticalMargin));

        if (UnityEngine.Random.value > 0.5f)
        {
            randomPos += new Vector2(camWidth + spawnHorizontalMargin * 2, 0);
        }

        return randomPos;
    }

    private GameObject InstantiateEnemy()
    {
        GameObject enemyInstance = Instantiate(enemy, GetRandomEnemySpawnVector(), Quaternion.Euler(Vector3.zero));
        EnemyController controller = enemyInstance.GetComponent<EnemyController>();
        controller.SetTarget(player);

        EnemyHealth health = enemyInstance.GetComponent<EnemyHealth>();
        health.SetDeathCallback(manager.EnemyDied);

        return enemyInstance;
    }

    private static float SimpleUpdateRate(float rate)
    {
        rate += 0.01f;
        return Mathf.Min(rate, 0.1f);
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if ((timer - initialDelay) * enemiesPerSecond > 1 && manager.CanAdd())
        {
            timer = initialDelay;
            GameObject instance = InstantiateEnemy();
            manager.AddEnemy(instance);
            enemiesPerSecond = updateRate(enemiesPerSecond);
        }


    }

}
