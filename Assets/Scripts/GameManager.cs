using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject  playerPrefab;

    [SerializeField]
    private GameObject scorePanel, startPanel, endPanel;

    [SerializeField]
    private TMP_Text scoreText, scoreEndText, highScoreText;

    private GameObject currentPillar,nextPiller, player;

    private int score,  highScore;

    public GameState currentState;

    public static GameManager instance;

    public GameObject panel_loading;

    bool start;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        
        Events.ScoreUpdate += UpdateScore;
        Events.GameOver += GameOver;
        Events.GameStart += GameStart;
        Events.CreateStartObject += CreateStartObjects;

        currentState = GameState.START;

        endPanel.SetActive(false);
        scorePanel.SetActive(false);
        startPanel.SetActive(true);

        score = 0;
        
        highScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : 0;

        scoreText.text = score.ToString();

        highScoreText.text = highScore.ToString();
    }
    private void OnDestroy()
    {
        Events.ScoreUpdate -= UpdateScore;
        Events.GameOver -= GameOver;
        Events.GameStart -= GameStart;
        Events.CreateStartObject -= CreateStartObjects;
    }
    void CreateStartObjects(GameObject _currentPillar,GameObject _nextpiller)
    {
        Events.PlatformChange?.Invoke();
        currentPillar = _currentPillar;
        nextPiller = _nextpiller;
        Vector3 playerPos = playerPrefab.transform.position;
        playerPos.x += (currentPillar.transform.localPosition.x * 0.5f - 0.35f);
        player = Instantiate(playerPrefab, playerPos, Quaternion.identity);
        player.name = "Player";
        Events.DeterminingCameraOffset?.Invoke(player);
    }

  

    void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    void GameOver()
    {
        endPanel.SetActive(true);
        scorePanel.SetActive(false);

        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        scoreEndText.text = score.ToString();
        highScoreText.text = highScore.ToString();
    }
    public void GameStart()
    {
        startPanel.SetActive(false);
        scorePanel.SetActive(true);

        Events.PlatformChange?.Invoke();
        currentState = GameState.INPUT;
        
    }

    public void GameRestart()
    {
        panel_loading.SetActive(true); 
        SceneManager.LoadScene(0);
    }

    public void SceneRestart()
    {
        Events.GameStart?.Invoke();
        SceneManager.LoadScene(0);
    }

  
}
