using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerPosition
{
    Left, Right
}

public class GameManager : MonoBehaviour
{
    public float basicPositionX;
    public float basicPositionY;
    public float basicPositionZ;

    public float progressMinimum;
    public float progressMaximum;
    public bool gameActive { get; private set; } = true;
    public PlayerPosition playerPosition;

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Image progressValue;
    [SerializeField] Text scoreText;
    
    private SpawnManager _spawnManager;
    private PlayerController _playerController;
    
    private int _score = 0;
    private float _progress = 50;
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameActive)
        {
            StartCoroutine(nameof(DecreaseProgress));
            if (_progress <= 0) { GameOver(); }
        }
    }

    private void StartGame()
    {
        gameActive = true;
    }
    public void GameOver()
    {
        gameActive = false;
        gameOverPanel.SetActive(true);
        _playerController.KillPlayer();
    }
    public void RestartGame()
    {
        gameOverPanel.SetActive(false);
        _playerController.ResetPosition();
        _playerController.RespawnPlayer();
        _spawnManager.RespawnBranches();
        gameActive = true;
        ResetProgress();
        ResetScore();
    }

    private void ResetScore()
    {
        _score = 0;
        SyncScore();
    }
    private void IncreaseScore()
    {
        _score += 1;
        SyncScore();
    }

    private void ResetProgress()
    {
        _progress = 50;
        SyncProgress();
    }
    private void IncreaseProgress()
    {
        _progress += 5;
        SyncProgress();
    }
    IEnumerator DecreaseProgress()
    {
        yield return new WaitForSeconds(0.1f);
        if (_progress > 0)
        {
            _progress -= 0.1f;
            SyncProgress();
        }
    }

    private void SyncScore()
    {
        scoreText.text = "Score: " + _score;
    }
    private void SyncProgress()
    {
        var offsetR = (100 - _progress) / 100 * progressMaximum;
        offsetR = Math.Min(Math.Max(offsetR, progressMinimum), progressMaximum);
        progressValue.rectTransform.offsetMax = new Vector2(-offsetR, -progressMinimum);
    }

    public void OnPlayerSwingSword(PlayerPosition position)
    {
        _spawnManager.SpawnWoodCube(position);
        _spawnManager.SpawnBranches();
        IncreaseProgress();
        IncreaseScore();
    }
}
