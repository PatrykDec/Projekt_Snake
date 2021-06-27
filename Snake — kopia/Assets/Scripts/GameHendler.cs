using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHendler : MonoBehaviour
{
    private static GameHendler instance;

    private static int score;

    [SerializeField] private Snake snake;

    private LevelGrid levelGrid;

    private void Awake()
    {
        instance = this;
        InitializeStatic();
        Time.timeScale = 1f;
    }

    void Start()
    {
        Debug.Log("GameHendler.Start");

        levelGrid = new LevelGrid(20, 20);

        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused())
            {
                GameHendler.ResumeGame();
            }
            else
            {
                GameHendler.PauseGame();
            }
        }
    }

    private static void InitializeStatic()
    {
        score = 0;
    }
    public static int GetScore()
    {
        return score;
    }
    public static void AddScore()
    {
        score += 100;
    }
    public static void SnakeDied()
    {
        GameOverWindow.ShowStatic();
    }

    public static void ResumeGame()
    {
        PauseWindow.HideStatic();
        Time.timeScale = 1f;
    }

    public static void PauseGame()
    {
        PauseWindow.ShowStatic();
        Time.timeScale = 0f;
    }

    public static bool IsGamePaused()
    {
        return Time.timeScale == 0f;
    }
}
