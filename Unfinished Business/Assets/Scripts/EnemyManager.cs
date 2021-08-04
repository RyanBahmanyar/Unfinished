using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    private PriorityQueue<GameObject> enemies;

    private Queue<EnemyInstantiater> instantiationQueue;

    private Transform player;

    [SerializeField]
    private uint enemyCap;
    [SerializeField]
    private uint aggroCap;

    private int CompareDistanceToPlayer(GameObject enemy1, GameObject enemy2)
    {
        if (enemy2 == null && enemy1 != null)
            return 1;
        else if (enemy1 == null && enemy2 != null)
            return -1;
        else if (enemy1 == null && enemy2 == null)
            return 0;


        float difference = PerspectiveUtilities.GetForeshortenedDistance(enemy1.transform.position, player.position) - PerspectiveUtilities.GetForeshortenedDistance(enemy2.transform.position, player.position);

        if (difference == 0)
            return 0;
        else if (difference > 0)
            return 1;
        else
            return -1;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemies = new PriorityQueue<GameObject>(enemyCap, CompareDistanceToPlayer);

        instantiationQueue = new Queue<EnemyInstantiater>();
    }

    public void AddToQueue(EnemyInstantiater instor)
    {
        instantiationQueue.Enqueue(instor);

        if (enemies.HasRoom())
            instantiationQueue.Dequeue().MakeInstance();
    }

    public void AddEnemy(GameObject enemy)
    {
        if (enemy.GetComponent<EnemyController>() == null)
            throw new ArgumentException("GameObject did not have an EnemyController.");

        enemies.Add(enemy);
    }

    public void EnemyDied(GameObject enemy)
    {
        IList<GameObject> enemyList = enemies.ToList();
        bool found = enemyList.Remove(enemy);

        if (!found)
            throw new ArgumentException("The killed enemy was not found." );

        enemies = new PriorityQueue<GameObject>(enemyList, enemyCap, CompareDistanceToPlayer);

        if (instantiationQueue.Count > 0)
            instantiationQueue.Dequeue().MakeInstance();
    }
}
