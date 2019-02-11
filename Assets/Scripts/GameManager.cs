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

    void AddMissedGate()
    {
        GatesMissed++;
        Debug.Log("Gates Missed: " + GatesMissed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Gate")
        {           
            if(!collision.GetComponent<HorizontalGate>().playerPassed)
            {
                AddMissedGate();
            }
            collision.gameObject.BroadcastMessage("DestroySelf");
        }
    }
}
