using Supyrb;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : Spawner
{
    [SerializeField] private float trajectoryVariance=15;
    [SerializeField] private int splittedAsteroidsAmount = 2;
    [SerializeField] private float splittedPosOffset = 0.5f;

    private void Awake()
    {
        Signals.Get<LoseSignal>().AddListener(StopSpawn);
        Signals.Get<RestartSignal>().AddListener(Restart);
        Signals.Get<SplitAsteroidSignal>().AddListener(SpawnAsteroidPieces);
    }

    void SpawnAsteroidPieces(Transform parentAsteroid,Sprite sprite)
    {
        for (int i = 0; i < splittedAsteroidsAmount; i++)
        {
            Vector2 pos = parentAsteroid.position;
            pos+= Random.insideUnitCircle * splittedPosOffset;
            AsteroidComponent asteroid=GetAsteroid(pos,parentAsteroid.rotation);
            asteroid.SetAsteroid(parentAsteroid.localScale.x/2,sprite);
        }
    }

    AsteroidComponent GetAsteroid(Vector2 pos,Quaternion rotation)
    {
        AsteroidComponent asteroid = pool.GetObject().GetComponent<AsteroidComponent>();
        asteroid.gameObject.SetActive(true);
        asteroid.transform.position=pos;
        asteroid.transform.rotation = rotation; 
        return asteroid;
    }

    protected override void Spawn()
    {
        Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
        Vector3 spawnPoint = transform.position + spawnDirection;
        float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
        Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);
        AsteroidComponent asteroid = GetAsteroid(spawnPoint,rotation);
        asteroid.SetAsteroid(rotation*-spawnDirection);
    }
}
