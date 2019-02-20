using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSpawnerScript : MonoBehaviour
{
    public GameObject BlueHGate;
    public GameObject BlueVGate;
    public GameObject RedHGate;
    public GameObject RedVGate;
    public GameObject FinishLine;
    private GameObject previousGate;
    public float spawnInterval = 1.0f;
    public float minSpawnX = -2.0f;
    public float maxSpawnX = 2.0f;
    public int NumberOfGatesToSpawn = 55;
    public float VerticalGateOffset = 1;
    public float PreparationTime = 5.0f;
    private int GatesSpawned = 0;
    private bool FlipFlop = false;
    private float defaultOffset;
    private bool GameFinished = false;

    Vector2 spawnLocation;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        GatesSpawned = 0;
        spawnLocation = this.transform.position;
        defaultOffset = spawnLocation.y;
        player = GameObject.Find("Player");
        
        StartCoroutine(GamePreparation(PreparationTime));
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.P) && !GameFinished)
        {
            FinishGame();
        }
    }

    void SpawnGate()
    {
        if(GatesSpawned < NumberOfGatesToSpawn)
        {
            spawnLocation.x = Random.Range(minSpawnX, maxSpawnX);
            if (FlipFlop)
            {
                FlipFlop = false;
                if (Random.Range(0.0f, 2.0f) > 0.2f)
                {
                    GameObject a = Instantiate(BlueHGate);

                    if (previousGate)
                    {
                        if (previousGate.name == "RedVGate" || previousGate.name == "BlueVGate")
                        {
                            spawnLocation.y = spawnLocation.y - VerticalGateOffset;
                        }
                        else
                        {
                            spawnLocation.y = defaultOffset;
                        }
                    }

                    previousGate = a;
                    a.transform.position = spawnLocation;
                    GatesSpawned++;
                }
                else
                {
                    GameObject a = Instantiate(BlueVGate);

                    if (previousGate)
                    {
                        if (previousGate.name == "RedVGate" || previousGate.name == "BlueVGate")
                        {
                            spawnLocation.y = spawnLocation.y - (VerticalGateOffset + 1.5f);
                        }
                        else
                        {
                            spawnLocation.y = defaultOffset;
                        }
                    }

                    previousGate = a;
                    a.transform.position = spawnLocation;
                    GatesSpawned++;
                }
            }
            else
            {
                FlipFlop = true;
                if (Random.Range(0.0f, 2.0f) > 0.2f)
                {
                    GameObject a = Instantiate(RedHGate);

                    if (previousGate)
                    {
                        if (previousGate.name == "RedVGate" || previousGate.name == "BlueVGate")
                        {
                            spawnLocation.y = spawnLocation.y - VerticalGateOffset;
                        }
                        else
                        {
                            spawnLocation.y = defaultOffset;
                        }
                    }

                    previousGate = a;
                    a.transform.position = spawnLocation;
                    GatesSpawned++;
                }
                else
                {
                    GameObject a = Instantiate(RedVGate);

                    if (previousGate)
                    {
                        if (previousGate.name == "RedVGate" || previousGate.name == "BlueVGate")
                        {
                            spawnLocation.y = spawnLocation.y - (VerticalGateOffset + 1.5f);
                        }
                        else
                        {
                            spawnLocation.y = defaultOffset;
                        }
                    }

                    previousGate = a;
                    a.transform.position = spawnLocation;
                    GatesSpawned++;
                }
            }            
            
            Debug.Log(GatesSpawned);
        }
        else
        {
            FinishGame();
        }
       
    }

    IEnumerator GamePreparation(float PrepTime)
    {
        float elapsedTime = 0.0f;

        while(elapsedTime < PrepTime)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        player.BroadcastMessage("GameStart");
        InvokeRepeating("SpawnGate", 0.0f, spawnInterval);
    }

    void FinishGame()
    {
        GameFinished = true;
        CancelInvoke("SpawnGate");
        GameObject a = Instantiate(FinishLine);
        a.transform.position = this.transform.position;
        Debug.Log("Finished!");
        // Add spawn finish line
    }
}

