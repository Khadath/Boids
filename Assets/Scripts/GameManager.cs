using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public int score = 0;
    public float timer = 30f;

    public Text scores;
    public Text timerText;

    public Canvas UIGame;
    public Canvas Endgame;

    // Start is called before the first frame update
    void Start()
    {
        Endgame.enabled = false;
        timerText.text = timer.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            endGame();
        }
        scores.text = score.ToString();
        float time = float.Parse(timerText.text) - Time.deltaTime;
        timer = time;
        timerText.text = time.ToString();
    }

    void endGame()
    {
        Time.timeScale = 0;
        UIGame.enabled = false;
        Endgame.enabled = true;
    }
}
