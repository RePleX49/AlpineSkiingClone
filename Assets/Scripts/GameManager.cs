using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int GatesMissed;
    public Text ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        GatesMissed = 0;
    }

    private void Update()
    {
        ScoreText.text = "-" + GatesMissed + "-";
    }

    public void AddMissedGate()
    {
        GatesMissed++;
        Debug.Log("Gates Missed: " + GatesMissed);
    }
}
