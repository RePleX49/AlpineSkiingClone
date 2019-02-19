using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int GatesMissed;
    public Text ScoreText;
    public Text TimerText;

    // Start is called before the first frame update
    void Start()
    {
        GatesMissed = 0;
    }

    void Update()
    {
        ScoreText.text = "-" + GatesMissed + "-";
        TimerText.text = Time.timeSinceLevelLoad.ToString();
    }

    public void AddMissedGate()
    {
        GatesMissed++;
        Debug.Log("Gates Missed: " + GatesMissed);
    }
}
