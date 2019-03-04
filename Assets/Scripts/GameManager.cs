using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameObject gateSpawner;

    int GatesHit;
    public Text ScoreText;
    public Text TimerText;
    public Text GameOverText;
    public bool GameFinished = false;
    public Button RestartButton;
    public Button MainMenuButton;

    bool bShowedEndScreen;

    private void Awake()
    {
        bShowedEndScreen = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        gateSpawner = GameObject.Find("GateSpawner");
        GatesHit = 0;
        GameOverText.gameObject.SetActive(false);
        RestartButton.gameObject.SetActive(false);
        MainMenuButton.gameObject.SetActive(false);
    }

    void Update()
    {
        ScoreText.text = "-" + GatesHit + "-";
        if(!GameFinished)
        {
            TimerText.text = Time.timeSinceLevelLoad.ToString();
        }
        else
        {
            if(!bShowedEndScreen)
            {
                GameOverText.text = "Game Over!" + "\n" + "Tricks Made: " + GatesHit + "/" + gateSpawner.GetComponent<GateSpawnerScript>().NumberOfGatesToSpawn;
                GameOverText.gameObject.SetActive(true);
                RestartButton.gameObject.SetActive(true);
                MainMenuButton.gameObject.SetActive(true);
            }
        }
    }

    public void AddGate()
    {
        GatesHit++;
        Debug.Log("Gates Missed: " + GatesHit);
    }
}
