using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
   public Text TargetText;

   public Text ScoreText;

   public Text LivesText;

   public int Score {get; set;}

    private void Awake() 
    {
       Brick.OnBrickDestruction += OnBrickDestruction;
       BrickManager.OnLevelLoaded += OnLevelLoaded;
       GameManager.OnLiveLost += OnLiveLost;
    }
   private void Start() 
   {
       OnLiveLost(GameManager.Instance.AvailableLives);
   }

    private void OnLiveLost(int remainingLives)
    {
        LivesText.text = $"LIVES: {remainingLives}";
    }

    private void OnLevelLoaded()
    {
        UpdateTargetText();
        UpdateScoreText(0);
    }

    private void UpdateScoreText(int increment)
    {
        this.Score += increment;
        string scoreString = this.Score.ToString().PadLeft(5, '0');
        ScoreText.text = $"SCORE:{Environment.NewLine}{scoreString}";
    }

    private void OnBrickDestruction(Brick obj)
    {
        UpdateTargetText();
        UpdateScoreText(10);
    }

    private void UpdateTargetText()
    {
        TargetText.text = $"TARGET:{Environment.NewLine}{BrickManager.Instance.RemainingBricks.Count.ToString()} / {BrickManager.Instance.InitialBricksCount}";
    }

    private void OnDisable()
    {
        Brick.OnBrickDestruction -= OnBrickDestruction;
        BrickManager.OnLevelLoaded -= OnLevelLoaded;    
    }
}
