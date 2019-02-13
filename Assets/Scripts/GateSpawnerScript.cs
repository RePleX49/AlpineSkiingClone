using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSpawnerScript : MonoBehaviour
{
    public GameObject HGate;
    public GameObject VGate;
    public GameObject FinishLine;
    private GameObject previousGate;
    public float spawnInterval = 1.0f;
    public float minSpawnX = -4.0f;
    public float maxSpawnX = 4.0f;
    public int NumberOfGatesToSpawn = 55;
    private int GatesSpawned = 0;

    Vector2 spawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        GatesSpawned = 0;
        spawnLocation = this.transform.position;
        InvokeRepeating("SpawnGate", 0.0f, spawnInterval);
    }

    void SpawnGate()
    {
        if(GatesSpawned < NumberOfGatesToSpawn)
        {
            spawnLocation.x = Random.Range(minSpawnX, maxSpawnX);
            //Transform spawnTransform = gameObject.transform;
            //spawnTransform.position = spawnLocation;
            if(Random.Range(0.0f, 2.0f) > 0.2f)
            {
                GameObject a = Instantiate(HGate);

                if(previousGate)
                {
                    if (previousGate.name == "V_Gate")
                    {
                        spawnLocation.x = previousGate.transform.position.x;
                    }
                }             

                previousGate = a;
                a.transform.position = spawnLocation;
                GatesSpawned++;
            }
            else
            {
                GameObject a = Instantiate(VGate);             
                previousGate = a;
                a.transform.position = spawnLocation;
                GatesSpawned++;
            }            
            
            Debug.Log(GatesSpawned);
        }
        else
        {
            CancelInvoke("SpawnGate");
            GameObject a = Instantiate(FinishLine);
            a.transform.position = this.transform.position;
            Debug.Log("Finished!");
            // Add spawn finish line
        }
       
    }
}
