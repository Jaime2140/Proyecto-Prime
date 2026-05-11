using UnityEngine;

public enum GameState
{
    Exploration,
    Combat,
    Pause,
    Dialog,
    WorldMap
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estado Actual")]
    public GameState currentState;

    public System.Action<GameState> OnStateChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetState(GameState.Exploration);
    }

    public void SetState(GameState newState)
    {
        currentState = newState;
        
        switch (newState)
        {
            case GameState.Pause:
            case GameState.WorldMap:
                Time.timeScale = 0f;
                break;
            
            case GameState.Exploration:
            case GameState.Combat:
                Time.timeScale = 1f;
                break;
        }

        Debug.Log("Nuevo Estado: " + newState);
        OnStateChanged?.Invoke(newState);
    }

    public void TogglePause()
    {
        if (currentState == GameState.Exploration)
            SetState(GameState.Pause);
        else if (currentState == GameState.Pause)
            SetState(GameState.Exploration);
    }
}