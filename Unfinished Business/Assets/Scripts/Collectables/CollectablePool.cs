using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectablePool : MonoBehaviour
{
    [SerializeField] int poolSize;
    [SerializeField] GameObject prefab;
    [SerializeField] bool test = false;

    private GameObject[] pool;

    private void Start ()
    {
        // Create objects in the pool
        pool = new GameObject[poolSize];
        for (int i = 0; i < pool.Length; i++)
        {
            GameObject newObj = Instantiate(prefab);
            newObj.transform.parent = transform;
            newObj.SetActive(false);
            pool[i] = newObj;
        }

        if (test)
        {
            StartCoroutine(Test());
        }
    }

    private IEnumerator Test ()
    {
        yield return new WaitForSeconds(2f);
        SpawnCollectables(3, new Vector2(0f, 1f), 3f, 0.1f);
    }

    //Use this to spawn collectables when an enemy dies.
    public void SpawnCollectables (int n, Vector2 position, float distance, float time)
    {
        int count = 0;
        for (int i = 0; i < pool.Length && count < n; i++)
        {
            GameObject g = pool[i];
            if (!g.activeSelf)
            {
                Collectable c = g.GetComponent<Collectable>();
                if (c != null)
                {
                    g.SetActive(true);
                    c.EnableCollectable(position, distance, time);
                    count++;
                }
            }
        }
    }
}
