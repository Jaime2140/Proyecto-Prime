using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapController : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject worldMapPanel; // El panel que contiene el fondo blanco y botones

    // Esta función la llamaremos desde el PauseMenuController
    public void OpenMap()
    {
        worldMapPanel.SetActive(true);
    }

    public void CloseMap()
    {
        worldMapPanel.SetActive(false);
    }

    // Función genérica para viajar. 
    // En el Inspector de Unity, escribiremos el nombre de la escena en cada botón.
    public void TravelTo(string sceneName)
    {
        Debug.Log("Viajando a: " + sceneName);
        
        // 1. Restaurar el tiempo por si el juego estaba pausado
        Time.timeScale = 1f;

        // 2. Cambiar el estado del juego a exploración antes de viajar
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetState(GameState.Exploration);
        }

        // 3. Cargar la escena
        SceneManager.LoadScene(sceneName);
    }
}