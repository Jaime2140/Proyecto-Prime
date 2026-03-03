using UnityEngine;

public enum GameState
{
    Exploration, // Caminando por la mazmorra
    Combat,      // En batalla
    Pause,       // Menú abierto
    Dialog,      // Hablando con NPC
    WorldMap     // Seleccionando destino (Estilo Persona 3)
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton para acceso global

    [Header("Estado Actual")]
    public GameState currentState;

    // Eventos para que otros scripts reaccionen (Opcional pero recomendado)
    public System.Action<GameState> OnStateChanged;

    void Awake()
    {
        // Configuración Singleton básica
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Para que persista entre escenas si quieres
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Empezamos explorando
        SetState(GameState.Exploration);
    }

    public void SetState(GameState newState)
    {
        currentState = newState;
        
        switch (newState)
        {
            case GameState.Pause:
            case GameState.WorldMap:
                Time.timeScale = 0f; // Congela el tiempo
                break;
            
            case GameState.Exploration:
            case GameState.Combat:
                Time.timeScale = 1f; // Tiempo normal
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