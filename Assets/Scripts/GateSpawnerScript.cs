using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSpawnerScript : MonoBehaviour
{
    public GameObject HGate;
    public GameObject VGate;
    public float spawnInterval = 1.0f;
    public float minSpawnX = -4.0f;
    public float maxSpawnX = 4.0f;

    Vector2 spawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = this.transform.position;
        InvokeRepeating("SpawnGate", 0.0f, spawnInterval);
    }

    void SpawnGate()
    {
        spawnLocation.x = Random.Range(minSpawnX, maxSpawnX);
        //Transform spawnTransform = gameObject.transform;
        //spawnTransform.position = spawnLocation;
        GameObject a = Instantiate(HGate);
        a.transform.position = spawnLocation;
    }
}
