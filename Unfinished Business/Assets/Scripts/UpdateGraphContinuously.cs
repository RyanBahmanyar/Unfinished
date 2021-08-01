using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AstarPath))]
public class UpdateGraphContinuously : MonoBehaviour
{
    private AstarPath pathfinder;
    
    public bool IsSyncing
    {
        get;
        private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        pathfinder = GetComponent<AstarPath>();
        IsSyncing = false;
        InvokeRepeating("UpdateGraph", 0f, 0.1f);
    }

    private void UpdateGraph()
    {
        if (!IsSyncing)
        {
            IsSyncing = true;
            pathfinder.Scan(pathfinder.graphs[0]);
            IsSyncing = false;
        }
    }
}
