using UnityEngine;
using System;

public enum GameState { Ready, Playing, Pause, GameOver }

public class GameManager : Singleton<GameManager>
{
    public GameState CurrentState { get; private set; } = GameState.Ready;
    
    // 해커톤 단골 데이터
    public int Score { get; private set; }
    public int BestScore { get; private set; }
    
    // 상태 변경 시 UI나 이펙트를 켜기 위한 이벤트
    public event Action<GameState> OnStateChanged;

    void Start()
    {
        BestScore = PlayerPrefs.GetInt("BestScore", 0);
        ChangeState(GameState.Ready);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        
        switch (newState)
        {
            case GameState.Ready: Time.timeScale = 1f; Score = 0; break;
            case GameState.Playing: Time.timeScale = 1f; break;
            case GameState.Pause: Time.timeScale = 0f; break;
            case GameState.GameOver: Time.timeScale = 0f; CheckBestScore(); break;
        }

        OnStateChanged?.Invoke(newState);
    }

    public void AddScore(int amount)
    {
        if (CurrentState != GameState.Playing) return;
        Score += amount;
    }

    private void CheckBestScore()
    {
        if (Score > BestScore)
        {
            BestScore = Score;
            PlayerPrefs.SetInt("BestScore", BestScore);
        }
    }
}
