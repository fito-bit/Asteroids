using System;
using System.Collections;
using Supyrb;
using UnityEngine;
using Random = UnityEngine.Random;

public class UfoSpawner : Spawner
{
    protected override void Spawn()
    {
        Vector3 spawnDirection = Random.insideUnitCircle.normalized * spawnDistance;
        Vector3 spawnPoint = transform.position + spawnDirection;
        GameObject ufo = pool.GetObject();
        ufo.SetActive(true);
        ufo.transform.position=spawnPoint;
    }
}
