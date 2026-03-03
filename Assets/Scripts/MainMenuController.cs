using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class MainMenuController : MonoBehaviour
{
    // Esta función la llamará el botón
    public void StartGame()
    {
        // IMPORTANTE: Aquí pon el nombre EXACTO de tu escena de juego
        // Por ejemplo: "SampleScene" o "Game"
        SceneManager.LoadScene("SampleScene"); 
    }
}