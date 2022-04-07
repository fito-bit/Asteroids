using System.Collections;
using Supyrb;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float respawnTime;
    [SerializeField] protected float spawnDistance=12;
    [SerializeField] protected Pool pool;
    
    private void Awake()
    {
        Signals.Get<LoseSignal>().AddListener(StopSpawn);
        Signals.Get<RestartSignal>().AddListener(Restart);
    }

    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }
    
    public void Restart()
    {
        StartCoroutine(SpawnObjects());
    }
    
    public void StopSpawn()
    {
        StopAllCoroutines();
        pool.DeactivateObjects();
    }

    IEnumerator SpawnObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);
            Spawn();
        }
    }

    protected virtual void Spawn()
    {
        
    }
}
