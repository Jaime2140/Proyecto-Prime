using UnityEngine;
using UnityEngine.InputSystem;
public class RandomEncounter : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject combatPanel; // Arrastra tu Panel de Combate aquí
    [Range(0, 100)]
    public float encounterChance = 10f; // 10% de probabilidad por paso

    [Header("Referencias")]
    public PlayerGridMovement playerMovement;

    private bool isFighting = false;

    // Esta función la llamaremos cada vez que el jugador dé un paso
    public void CheckForEncounter()
    {
        if (isFighting) return;

        // Tira un dado de 0 a 100
        float roll = Random.Range(0f, 100f);

        if (roll < encounterChance)
        {
            StartEncounter();
        }
    }

    void StartEncounter()
    {
        isFighting = true;
        
        // Mostrar el panel de combate
        if (combatPanel != null) combatPanel.SetActive(true);

        // Desactivar el movimiento del jugador para que no siga caminando
        if (playerMovement != null) playerMovement.enabled = false;

        Debug.Log("¡Combate Iniciado!");
    }

    void EndEncounter()
    {
        isFighting = false;

        // Ocultar panel
        if (combatPanel != null) combatPanel.SetActive(false);

        // Reactivar movimiento
        if (playerMovement != null) playerMovement.enabled = true;

        Debug.Log("Combate Terminado");
    }

    void Update()
    {
        // Lógica simple para "ganar" el combate
        if (isFighting && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            EndEncounter();
        }
    }
}