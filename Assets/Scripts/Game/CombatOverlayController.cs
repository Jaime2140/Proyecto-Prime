using UnityEngine;

public class CombatOverlayController : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Arrastra aquí tu Panel_Ejecutar_Overlay")]
    public GameObject combatOverlayPanel;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged += HandleStateChange;
        }
        
        if (combatOverlayPanel != null)
        {
            combatOverlayPanel.SetActive(false);
        }
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
        if (combatOverlayPanel == null) return;

        if (newState == GameState.Combat)
        {
            combatOverlayPanel.SetActive(true);
        }
        else if (newState == GameState.Exploration)
        {
            combatOverlayPanel.SetActive(false);
        }
    }
}