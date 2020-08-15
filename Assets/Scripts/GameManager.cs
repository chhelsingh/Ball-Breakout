using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;
    public static GameManager Instance => _instance;


    private void Awake() 
    {
        if(_instance != null )
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public GameObject gameOverScreen;

    public GameObject victoryScreen;

    public bool IsGameStarted { get; set; }

    public static event Action<int> OnLiveLost;

    public int AvailableLives = 3;

    public int Lives {get; set;}
    
    private void Start() 
    {
        this.Lives = this.AvailableLives;
        Screen.SetResolution(540,960, false);
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnBrickDestruction += OnBrickDestruction;
    }

    private void OnBrickDestruction(Brick obj)
    {
       if(BrickManager.Instance.RemainingBricks.Count <= 0)
       {
           BallsManager.Instance.ResetBalls();
           GameManager.Instance.IsGameStarted = false;
           BrickManager.Instance.LoadNextLevel();
       }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnBallDeath(Ball obj)
    {
        if(BallsManager.Instance.Balls.Count <= 0)
        {
            this.Lives--;
            if(this.Lives < 1)
            {
                gameOverScreen.SetActive(true);
            }
            else
            {
                OnLiveLost?.Invoke(this.Lives);
                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;
                BrickManager.Instance.LoadLevel(BrickManager.Instance.CurrentLevel);
            }
        }
    }

    internal void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }

    private void OnDisable() 
    {
        Ball.OnBallDeath -= OnBallDeath;    
    }
}
