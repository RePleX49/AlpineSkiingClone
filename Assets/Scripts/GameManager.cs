using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    int GatesMissed;

    // Start is called before the first frame update
    void Start()
    {
        GatesMissed = 0;
    }

    public void AddMissedGate()
    {
        GatesMissed++;
        Debug.Log("Gates Missed: " + GatesMissed);
    }
}
