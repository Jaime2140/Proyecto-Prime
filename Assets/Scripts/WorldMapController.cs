using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapController : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject worldMapPanel;

    public void OpenMap()
    {
        worldMapPanel.SetActive(true);
    }

    public void CloseMap()
    {
        worldMapPanel.SetActive(false);
    }

    public void TravelTo(string sceneName)
    {
        Debug.Log("Viajando a: " + sceneName);
        
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetState(GameState.Exploration);
        }

        SceneManager.LoadScene(sceneName);
    }
}