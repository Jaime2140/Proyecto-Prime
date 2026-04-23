using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject pausePanel;
    public GameObject worldMapPanel;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged += HandleStateChange;
        }
        
        pausePanel.SetActive(false);
        if(worldMapPanel != null) worldMapPanel.SetActive(false);
    }

    void OnDestroy()
    {
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
            pausePanel.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        GameManager.Instance.SetState(GameState.Exploration);
    }

    public void OpenWorldMap()
    {
        pausePanel.SetActive(false);
        if(worldMapPanel != null) 
        {
            worldMapPanel.SetActive(true);
        }
    }

    public void QuitToMainMenu()
    {
        Debug.Log("Volviendo al menú principal...");
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetState(GameState.Exploration);
        }

        SceneManager.LoadScene("Menu"); 
    }
}