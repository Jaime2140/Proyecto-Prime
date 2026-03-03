using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject pausePanel;    // El panel visual del menú (con fondo negro/transparente)
    public GameObject worldMapPanel; // El panel estilo Persona 3 (lo haremos luego)

    void Start()
    {
        // Nos suscribimos al GameManager para saber cuándo mostrar/ocultar
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged += HandleStateChange;
        }
        
        // Asegurarnos de que empiece cerrado
        pausePanel.SetActive(false);
        if(worldMapPanel != null) worldMapPanel.SetActive(false);
    }

    void OnDestroy()
    {
        // Buena práctica: desuscribirse para evitar errores
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= HandleStateChange;
        }
    }

    void HandleStateChange(GameState newState)
    {
        if (newState == GameState.Pause)
        {
            pausePanel.SetActive(true);
        }
        else
        {
            // Si salimos de pausa (a exploración o a combate), cerramos el menú
            pausePanel.SetActive(false);
        }
    }

    // --- Funciones para los Botones ---

    public void ResumeGame()
    {
        GameManager.Instance.SetState(GameState.Exploration);
    }

    public void OpenWorldMap()
    {
        pausePanel.SetActive(false); // Cerramos el menú de pausa normal
        if(worldMapPanel != null) 
        {
            worldMapPanel.SetActive(true);
            // Opcional: GameManager.Instance.SetState(GameState.WorldMap);
        }
    }

    public void QuitToMainMenu()
    {
        Debug.Log("Volviendo al menú principal...");
        
        // 1. IMPORTANTE: Asegúrate de que el tiempo corra de nuevo. 
        // Si el juego está en pausa y el TimeScale es 0, el menú principal podría quedarse "congelado".
        Time.timeScale = 1f;

        // 2. Cambiar el estado en el GameManager si es necesario (evita conflictos al volver a empezar)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetState(GameState.Exploration); // O un estado por defecto
        }

        // 3. Carga la escena del menú. Pon aquí el nombre exacto de tu escena.
        SceneManager.LoadScene("Menu"); 
    }
}